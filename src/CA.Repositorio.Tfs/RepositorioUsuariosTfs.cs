using CA.Core.Configuracoes;
using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.Tfs;
using CA.Repositorios.Tfs.Extensions;
using CA.Repositorios.Tfs.Interfaces;
using CA.Repositorios.Tfs.ServicosSoap.Models;
using CA.Repositorios.Tfs.ServicosSoap.Models.Requests;
using Polly;
using Polly.Registry;

namespace CA.Repositorios.Tfs
{
    public class RepositorioUsuariosTfs : IRepositorioUsuariosTfs
    {
        private readonly IServicoIdentidadeTfsSoap _servicoIdentidade;
        private readonly IServicoColecaoTfsHttp _servicoColecao;
        private readonly IAsyncPolicy _politicaPolly;

        public RepositorioUsuariosTfs(ConfiguracaoClienteTfs configuracoes, IServicoIdentidadeTfsSoap servicoIdentidade, IServicoColecaoTfsHttp servicoColecao, IPolicyRegistry<string> registry)
        {
            configuracoes.Validar();

            _servicoIdentidade = servicoIdentidade;
            _servicoColecao = servicoColecao;

            _politicaPolly = registry.Get<IAsyncPolicy>(nameof(IRepositorioTfs));
        }

        public async Task<UsuarioTfs?> ObterUsuarioAsync(string colecao, string usuario)
        {
            var resultado = await _politicaPolly.ExecuteAsync(() => 
            {
                return _servicoIdentidade.ObterIdentidadeDeUsuarioTfs(new ReadIdentitiesRequest(0, new string[] { usuario }, 0, 4, 1, null, 0), colecao);
            });

            var identidade = resultado.ReadIdentitiesResult.Any() && resultado.ReadIdentitiesResult[0].Any() ? resultado.ReadIdentitiesResult[0][0] : null;

            if (identidade is null)
                return null;

            return await IdentidadeParaUsuarioTfsAsync(identidade);
        }

        private async Task<UsuarioTfs> IdentidadeParaUsuarioTfsAsync(TeamFoundationIdentity identidade)
        {
            var usuario = new UsuarioTfs
            {
                Ativo = identidade.IsActive,
                NomeUsuario = identidade.ObterPropriedade<string>("Account"),
                NomeCompleto = identidade.DisplayName,
                Dominio = identidade.ObterPropriedade<string>("Domain"),
                Identidade = new IdentidadeTfs
                {
                    Id = identidade.Descriptor.identifier,
                    Tipo = identidade.Descriptor.identityType
                },
                Email = !string.IsNullOrEmpty(identidade.ObterPropriedade<string>("Mail")) ?
                                    identidade.ObterPropriedade<string>("Mail") :
                                    identidade.ObterPropriedade<string>("ConfirmedNotificationAddress")
            };

            usuario.Colecoes = await _servicoColecao.ObterColecoesPorUsuarioAsync(usuario);

            return usuario;
        }
    }
}
