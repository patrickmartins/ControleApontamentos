using CA.Core.Configuracoes;
using CA.Servicos.Channel.Extensions;
using CA.Servicos.Channel.Helpers;
using CA.Servicos.Channel.Interfaces;
using CA.Servicos.Channel.Models;
using CA.Servicos.Channel.Models.Requests;
using CA.Servicos.Channel.Models.Responses;
using CA.Util.Extensions;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Registry;
using System.IdentityModel.Tokens.Jwt;

namespace CA.Servicos.Channel
{
    public class ServicoChannelHttp : IServicoChannelHttp
    {
        private static ChannelJwt? _token;
        private static IEnumerable<ChannelCookie>? _cookies;

        private readonly ConfiguracaoClienteChannel _configuracoes;

        private readonly IAsyncPolicy _policy;

        private object _lock = new object();

        public ServicoChannelHttp(ConfiguracaoClienteChannel configuracoes, IPolicyRegistry<string> registry)
        {
            _configuracoes = configuracoes;

            _policy = registry.Get<IAsyncPolicy>(nameof(IServicoChannelHttp));
        }

        private ChannelJwt ObterTokenJwt()
        {
            lock (_lock)
            {
                if (_token is null || _token.Expirado())
                {
                    _token = ObterTokenAcesso();
                }

                return _token;
            }
        }

        private IEnumerable<ChannelCookie> ObterCookies(ChannelJwt token)
        {
            lock (_lock)
            {
                if (_cookies is null)
                {
                    _cookies = ObterCookiesAcesso(token);
                }

                return _cookies;
            }
        }

        private void RemoverCookies()
        {
            _cookies = null;
        }

        private IEnumerable<ChannelCookie> ObterCookiesAcesso(ChannelJwt token)
        {
            var result = _policy.ExecuteAsync(() =>
            {
                return token.ObterUrlAutenticacao()
                                .WithAutoRedirect(false)
                                .GetAsync();
            }).Result;  

            return result.Cookies.Select(c => new ChannelCookie
            {
                Nome = c.Name,
                Valor = c.Value,
                UrlOrigem = c.OriginUrl
            })
            .ToList();
        }

        private ChannelJwt ObterTokenAcesso()
        {
            var token = _policy.ExecuteAsync(async () =>
            {
                return await _configuracoes.UrlCloud
                                            .AppendPathSegment("api/auth")
                                            .PostJsonAsync(new LoginRequest
                                            {
                                                Usuario = _configuracoes.Usuario,
                                                Senha = _configuracoes.Senha
                                            })
                                            .ReceiveJson<ChannelJwt>();
            }).Result;

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(token.TokenAcesso);
            var claimExpiracao = jwtToken.Claims.First(c => c.Type == "exp");

            token.DataExpiracao = DateTimeOffset.FromUnixTimeSeconds(int.Parse(claimExpiracao.Value)).LocalDateTime.ConverterParaFusoBrasil();

            return token;
        }

        public async Task<IEnumerable<UsuarioResponse>> ObterTodosUsuariosAsync()
        {
            var resultado = await _policy.ExecuteAsync(() =>
            {
                var token = ObterTokenJwt();

                return _configuracoes.UrlCloud
                                    .AppendPathSegment("api/iam/users")
                                    .SetQueryParam("page", 0)
                                    .SetQueryParam("query", "query")
                                    .SetQueryParam("limit", 100000)
                                    .WithOAuthBearerToken(token.TokenAcesso)
                                    .OnError(c =>
                                    {
                                        RemoverCookies();
                                    })
                                    .GetJsonAsync<ConsultaUsuariosResponse>();
            });

            return resultado.Resultados.Where(c => c.Id > 0);
        }

        public async Task<IEnumerable<ProjetoResponse>> ObterProjetosAsync()
        {
            var resultado = await _policy.ExecuteAsync(() =>
            {
                var token = ObterTokenJwt();
                var cookies = ObterCookies(token).ParaCookieJar();

                var parametros = "callCount=1" + Environment.NewLine;
                parametros += "windowName=c0-e13" + Environment.NewLine;
                parametros += "c0-scriptName=ProjetoAjax" + Environment.NewLine;
                parametros += "c0-methodName=listar" + Environment.NewLine;
                parametros += "c0-id=0" + Environment.NewLine;
                parametros += "c0-param0=number:1" + Environment.NewLine;
                parametros += "c0-e1=string:" + Environment.NewLine;
                parametros += "c0-e2=string:-1" + Environment.NewLine;
                parametros += "c0-e3=string:" + Environment.NewLine;
                parametros += "c0-e4=string:" + Environment.NewLine;
                parametros += "c0-e5=string:0" + Environment.NewLine;
                parametros += "c0-e6=string:0" + Environment.NewLine;
                parametros += "c0-e7=string:-1" + Environment.NewLine;
                parametros += "c0-e8=boolean:true" + Environment.NewLine;
                parametros += "c0-e9=null:null" + Environment.NewLine;
                parametros += "c0-e10=null:null" + Environment.NewLine;
                parametros += "c0-e11=boolean:false" + Environment.NewLine;
                parametros += "c0-e12=string:false" + Environment.NewLine;
                parametros += "c0-e13=array:[]" + Environment.NewLine;
                parametros += "c0-e14=boolean:false" + Environment.NewLine;
                parametros += "c0-param1=Object_Object:{nome:reference:c0-e1, status:reference:c0-e2, nomeGerente:reference:c0-e3, nomePatrocinador:reference:c0-e4, tipoProjeto:reference:c0-e5, idArea:reference:c0-e6, idCliente:reference:c0-e7, comSubareas:reference:c0-e8, pmoAcc:reference:c0-e9, pmoAa:reference:c0-e10, subProjetos:reference:c0-e11, aprovacoesPendentes:reference:c0-e12, filtrosCampoAdicional:reference:c0-e13, somenteProjetosIntegrados:reference:c0-e14}" + Environment.NewLine;
                parametros += "c0-param2=string:nome" + Environment.NewLine;
                parametros += "c0-param3=string:ASC" + Environment.NewLine;
                parametros += "c0-param4=number:0" + Environment.NewLine;
                parametros += "c0-param5=string:-1" + Environment.NewLine;
                parametros += "c0-param6=boolean:false" + Environment.NewLine;
                parametros += "batchId=1" + Environment.NewLine;
                parametros += "instanceId=0" + Environment.NewLine;
                parametros += "page=%2Fchannel%2Fprojeto.do%3Faction%3DlistarPorStatus" + Environment.NewLine;
                parametros += "scriptSessionId=jScbUi07ZNiZhPE1ciy$HP8cLno/ZAoVMno-mKsCPfiIb" + Environment.NewLine;

                return _configuracoes.UrlBase
                                    .AppendPathSegment("dwr/call/plaincall/ProjetoAjax.listar.dwr")
                                    .WithCookies(cookies)
                                    .OnRedirect(c =>
                                    {
                                        RemoverCookies();

                                        throw new FlurlHttpException(c);
                                    })
                                    .OnError(c =>
                                    {
                                        RemoverCookies();
                                    })
                                    .PostStringAsync(parametros)
                                    .ReceiveString();
            });

            return ChannelStringResponseHelper.DesserializarResultado<ProjetoResponse>(resultado);
        }

        public async Task<ProjetoResponse?> ObterProjetoPorIdAsync(int idProjeto)
        {
            var resultado = await _policy.ExecuteAsync(() =>
            {
                var token = ObterTokenJwt();
                var cookies = ObterCookies(token).ParaCookieJar();

                return _configuracoes.UrlBase
                                    .AppendPathSegment($"api/escopo/{idProjeto}")
                                    .WithCookies(cookies)
                                    .OnRedirect(c =>
                                    {
                                        RemoverCookies();

                                        throw new FlurlHttpException(c);
                                    })
                                    .OnError(c =>
                                    {
                                        RemoverCookies();
                                    })
                                    .GetJsonAsync<EscopoResponse>();
            });

            if (resultado is null || resultado.Projeto is null)
                return null;

            return resultado.Projeto;
        }

        public async Task<IEnumerable<AtividadeResponse>> ObterAtividadesPorProjetoAsync(int idProjeto)
        {
            var resultado = await _policy.ExecuteAsync(() =>
            {
                var token = ObterTokenJwt();
                var cookies = ObterCookies(token).ParaCookieJar();

                return _configuracoes.UrlBase
                                    .AppendPathSegment($"api/escopo/{idProjeto}")
                                    .WithCookies(cookies)
                                    .OnRedirect(c =>
                                    {
                                        RemoverCookies();

                                        throw new FlurlHttpException(c);
                                    })
                                    .OnError(c =>
                                    {
                                        RemoverCookies();
                                    })
                                    .GetJsonAsync<EscopoResponse>();
            });

            if (resultado is null || resultado.Projeto is null)
                return new List<AtividadeResponse>();

            var atividades = resultado.Projeto.Atividades.ToList();

            atividades.ForEach(c => c.IdProjeto = idProjeto);

            return atividades;
        }

        public async Task<IEnumerable<ApontamentoResponse>> ObterApontamentosPorPeriodoAsync(DateOnly inicio, DateOnly fim)
        {
            var stream = await _policy.ExecuteAsync(() =>
            {
                var token = ObterTokenJwt();
                var cookies = ObterCookies(token).ParaCookieJar();

                return _configuracoes.UrlBase
                                    .AppendPathSegment("apontamento.do")
                                    .SetQueryParam("action", "exportarExcelRelatorioGeral")
                                    .SetQueryParam("area", -1)
                                    .SetQueryParam("dataInicio", inicio.ToString("d"))
                                    .SetQueryParam("dataFim", fim.ToString("d"))
                                    .SetQueryParam("idGrupo", 0)
                                    .SetQueryParam("tipoRelatorio", 0)
                                    .SetQueryParam("idColaborador", 0)
                                    .SetQueryParam("comFilhosAreaColaborador", true)
                                    .SetQueryParam("comFilhosArea", true)
                                    .SetQueryParam("isFuncional", true)
                                    .SetQueryParam("horaExtra", 0)
                                    .SetQueryParam("direcaoOrdenacao", "ASC")
                                    .SetQueryParam("colunaOrdenacao", "area")
                                    .SetQueryParam("colunaOrdenacao", 0)
                                    .WithCookies(cookies)
                                    .OnRedirect(c =>
                                    {
                                        RemoverCookies();

                                        throw new FlurlHttpException(c);
                                    })
                                    .OnError(c =>
                                    {
                                        RemoverCookies();
                                    })
                                    .GetStreamAsync();
            });

            return ChannelStreamResponseHelper.ExtrairApontamentosDeRelatorio(stream);
        }
    }
}
