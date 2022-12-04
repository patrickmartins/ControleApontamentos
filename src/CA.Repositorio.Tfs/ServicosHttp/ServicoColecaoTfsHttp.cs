using CA.Core.Configuracoes;
using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.Tfs;
using CA.FlurlSoap;
using CA.Repositorios.Tfs.Entidades;
using CA.Repositorios.Tfs.Interfaces;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Registry;

namespace CA.Repositorios.Tfs.ServicosHttp
{
    public class ServicoColecaoTfsHttp : IServicoColecaoTfsHttp
    {
        private readonly ConfiguracaoClienteTfs _configuracoes;
        private readonly IAsyncPolicy _politicaPolly;

        public ServicoColecaoTfsHttp(ConfiguracaoClienteTfs configuracoes, IPolicyRegistry<string> registry)
        {
            configuracoes.Validar();

            _configuracoes = configuracoes;

            _politicaPolly = registry.Get<IAsyncPolicy>(nameof(IRepositorioTfs));
        }

        public async Task<string[]> ObterTodasColecoesAsync()
        {
            var url = $"{_configuracoes.UrlTfs}{_configuracoes.UrlServicoColecao}";

            var resultado = await _politicaPolly.ExecuteAsync(() =>
            {
                return url.WithWindowsAuth(_configuracoes.Usuario, _configuracoes.Senha)
                           .GetJsonAsync<ArrayColecaoTfs>();
            });

            return resultado.Colecoes.Select(c => c.Nome).ToArray();
        }

        public async Task<string[]> ObterColecoesPorUsuarioAsync(UsuarioTfs usuario)
        {
            var url = $"{_configuracoes.UrlTfs}{_configuracoes.UrlServicoColecao}";

            var resultado = await _politicaPolly.ExecuteAsync(() =>
            {
                return url.WithHeader("X-TFS-Impersonate", $"{usuario.Identidade.Tipo};{usuario.Identidade.Id}")
                            .WithWindowsAuth(_configuracoes.Usuario, _configuracoes.Senha)
                            .GetJsonAsync<ArrayColecaoTfs>();
            });

            return resultado.Colecoes.Select(c => c.Nome).ToArray();
        }
    }
}
