﻿using CA.Core.Configuracoes;
using CA.Core.Entidades.Ponto;
using CA.Core.Interfaces.Ponto;
using CA.Repositorios.Ponto.Entidades;
using CA.Repositorios.Ponto.Interfaces;
using CA.Util.Extensions;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Registry;
using System.Net;

namespace CA.Repositorios.Ponto.ServicosHttp
{
    public class ServicoSecullumHttp : IServicoSecullumHttp
    {
        private static SecullumJwt? _token;

        private readonly ConfiguracaoClienteSecullum _configuracoes;
        private readonly IAsyncPolicy _policy;

        private object _lock = new object();

        public ServicoSecullumHttp(ConfiguracaoClienteSecullum configuracoes, IPolicyRegistry<string> registry)
        {
            configuracoes.Validar();

            _configuracoes = configuracoes;

            _policy = registry.Get<IAsyncPolicy>(nameof(IRepositorioPonto));
        }

        public async Task<ICollection<Funcionario>> ObterFuncionariosAsync()
        {
            var functionarios = await _policy.ExecuteAsync(() =>
            {
                var token = ObterTokenJwt();

                return _configuracoes.UrlIntegracao
                                        .AppendPathSegment("Funcionarios")
                                        .WithOAuthBearerToken(token.TokenAcesso)
                                        .OnError(c =>
                                        {
                                            if (c.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                                                _token = null;
                                        })
                                        .GetJsonAsync<ICollection<Funcionario>>();
            });

            return functionarios;
        }

        public Task<ICollection<BatidasPontoDia>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim)
        {
            var lista = _policy.ExecuteAsync(() =>
            {
                var token = ObterTokenAcesso();

                return _configuracoes.UrlIntegracao
                                            .AppendPathSegment("Batidas")
                                            .WithOAuthBearerToken(token.TokenAcesso)
                                            .SetQueryParam("DataInicio", inicio.ToString("yyyy-MM-dd"))
                                            .SetQueryParam("DataFim", fim.ToString("yyyy-MM-dd"))
                                            .SetQueryParam("FuncionarioPis", pisFuncionario)
                                            .OnError(c =>
                                            {
                                                if (c.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                                                    _token = null;
                                            })
                                            .GetJsonAsync<ICollection<BatidasPontoDia>>();
            });

            return lista;
        }

        private SecullumJwt ObterTokenJwt()
        {
            lock (_lock)
            {
                if (_token is null)
                {
                    _token = ObterTokenAcesso();
                }
                else
                {
                    if (_token.Expirado())
                    {
                        _token = RenovarTokenAcesso(_token);
                    }
                }

                return _token;
            }
        }

        private SecullumJwt ObterTokenAcesso()
        {
            var token = _policy.ExecuteAsync(() =>
            {
                return _configuracoes.UrlAutenticador
                                            .AppendPathSegment("Token")
                                            .PostMultipartAsync(c =>
                                            {
                                                c.AddString("grant_type", "password");
                                                c.AddString("username", _configuracoes.Usuario);
                                                c.AddString("password", _configuracoes.Senha);
                                                c.AddString("client_id", _configuracoes.IdCliente);
                                            })
                                            .ReceiveJson<SecullumJwt>();
            }).Result;

            token.DataExpiracao = DateTime.Now.ConverterParaFusoBrasil().AddSeconds(token.Validade);

            return token;
        }

        private SecullumJwt RenovarTokenAcesso(SecullumJwt token)
        {
            var novoToken = _policy.ExecuteAsync(() =>
            {
                return _configuracoes.UrlAutenticador
                                    .AppendPathSegment("Token")
                                    .WithOAuthBearerToken(token.TokenAcesso)
                                    .PostMultipartAsync(c =>
                                    {
                                        c.AddString("grant_type", "refresh_token");
                                        c.AddString("refresh_token", token.TokenAtualizacao);
                                        c.AddString("client_id", _configuracoes.IdCliente);
                                    })
                                    .ReceiveJson<SecullumJwt>();
            }).Result;

            novoToken.DataExpiracao = DateTime.Now.ConverterParaFusoBrasil().AddSeconds(token.Validade);

            return novoToken;
        }
    }
}
