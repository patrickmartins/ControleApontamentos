﻿using CA.Core.Configuracoes;
using CA.Core.Entidades.Ponto;
using CA.Core.Interfaces.Ponto;
using CA.Core.Valores;
using CA.Servicos.Secullum.Entidades;
using CA.Servicos.Secullum.Interfaces;
using CA.Util.Extensions;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Registry;
using System.Net;

namespace CA.Servicos.Secullum
{
    public class ServicoSecullumHttp : IServicoSecullumHttp
    {
        private static SecullumJwt? _token;

        private readonly ConfiguracaoClienteSecullum _configuracoes;
        private readonly IAsyncPolicy _policy;

        private object _lock = new object();

        public ServicoSecullumHttp(ConfiguracaoClienteSecullum configuracoes, IPolicyRegistry<string> registry)
        {
            _configuracoes = configuracoes;

            _policy = registry.Get<IAsyncPolicy>(nameof(IRepositorioPonto));
        }

        public async Task<IEnumerable<Funcionario>> ObterFuncionariosAsync()
        {
            var token = ObterTokenJwt();

            var funcionarios = await _policy.ExecuteAsync(() =>
            {
                return _configuracoes.UrlSecullum
                                        .AppendPathSegment("Funcionarios")
                                        .WithOAuthBearerToken(token.TokenAcesso)
                                        .OnError(c =>
                                        {
                                            if (c.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                                                RemoverTokenJwt();
                                        })
                                        .GetJsonAsync<IEnumerable<Funcionario>>();
            });

            return funcionarios;
        }

        public async Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim)
        {
            var token = ObterTokenJwt();

            var resposta = await _policy.ExecuteAsync(() =>
            {
                return _configuracoes.UrlIntegracao
                                            .AppendPathSegment("Batidas")
                                            .WithOAuthBearerToken(token.TokenAcesso)
                                            .SetQueryParam("DataInicio", inicio.ToString("yyyy-MM-dd"))
                                            .SetQueryParam("DataFim", fim.ToString("yyyy-MM-dd"))
                                            .SetQueryParam("FuncionarioPis", pisFuncionario)
                                            .AllowHttpStatus("400")
                                            .OnError(c =>
                                            {
                                                if (c.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                                                    RemoverTokenJwt();
                                            }).GetAsync();
            });

            if (resposta.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                var erros = await resposta.GetJsonAsync<IEnumerable<ErroRequisicao>>();

                return Resultado.DeErros<IEnumerable<BatidasPontoDia>>(erros.Select(c => new Erro(c.Mensagem, c.Propriedade)).ToArray());
            }

            return Resultado.DeValor((await resposta.GetJsonAsync<IEnumerable<BatidasPontoDia>>()).OrderBy(c => c.Data).AsEnumerable());
        }

        public async Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterTodasBatidasPorPeriodoAsync(DateOnly inicio, DateOnly fim)
        {
            var token = ObterTokenJwt();

            var resposta = await _policy.ExecuteAsync(() =>
            {
                return _configuracoes.UrlIntegracao
                                            .AppendPathSegment("Batidas")
                                            .WithOAuthBearerToken(token.TokenAcesso)
                                            .SetQueryParam("DataInicio", inicio.ToString("yyyy-MM-dd"))
                                            .SetQueryParam("DataFim", fim.ToString("yyyy-MM-dd"))
                                            .AllowHttpStatus("400")
                                            .OnError(c =>
                                            {
                                                if (c.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                                                    RemoverTokenJwt();
                                            }).GetAsync();
            });

            if (resposta.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                var erros = await resposta.GetJsonAsync<IEnumerable<ErroRequisicao>>();

                return Resultado.DeErros<IEnumerable<BatidasPontoDia>>(erros.Select(c => new Erro(c.Mensagem, c.Propriedade)).ToArray());
            }

            return Resultado.DeValor((await resposta.GetJsonAsync<IEnumerable<BatidasPontoDia>>()).OrderBy(c => c.Data).AsEnumerable());
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

        private void RemoverTokenJwt()
        {
            _token = null;
        }
    }
}
