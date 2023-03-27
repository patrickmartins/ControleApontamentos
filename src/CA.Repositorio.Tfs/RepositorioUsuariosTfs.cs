using CA.Core.Configuracoes;
using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.Tfs;
using CA.Repositorios.Tfs.Extensions;
using CA.Servicos.Tfs.Interfaces;
using CA.Servicos.Tfs.Models;
using CA.Servicos.Tfs.Models.Requests;
using Polly;
using Polly.Registry;

namespace CA.Repositorios.Tfs
{
    public class RepositorioUsuariosTfs : IRepositorioUsuariosTfs
    {
        private readonly ConfiguracaoClienteTfs _configuracoes;
        private readonly IServicoIdentidadeTfsSoap _servicoIdentidade;
        private readonly IServicoColecaoTfsHttp _servicoColecao;
        private readonly IAsyncPolicy _politicaPolly;

        public RepositorioUsuariosTfs(ConfiguracaoClienteTfs configuracoes, IServicoIdentidadeTfsSoap servicoIdentidade, IServicoColecaoTfsHttp servicoColecao, IPolicyRegistry<string> registry)
        {
            _configuracoes = configuracoes;
            _servicoIdentidade = servicoIdentidade;
            _servicoColecao = servicoColecao;

            _politicaPolly = registry.Get<IAsyncPolicy>(nameof(IRepositorioTfs));
        }

        public async Task<IEnumerable<UsuarioTfs>> ObterTodosUsuariosAsync(string colecao)
        {
            var usuarios = new List<UsuarioTfs>();

            var resultado = await _politicaPolly.ExecuteAsync(() =>
            {
                return _servicoIdentidade.BuscarUsuariosTfs(new ReadIdentitiesRequest(7, new string[] { _configuracoes.Dominio }, 0, 4, 1, null, 0), colecao);
            });

            var identidades = resultado.ReadIdentitiesResult.Any() && resultado.ReadIdentitiesResult[0].Any() ? resultado.ReadIdentitiesResult[0] : null;

            if (identidades is null)
                return usuarios;

            foreach (var identidade in identidades)
            {
                usuarios.Add(await IdentidadeParaUsuarioTfsAsync(identidade, false));
            }

            return usuarios;
        }

        public async Task<UsuarioTfs?> ObterUsuarioAsync(string colecao, string usuario)
        {
            var resultado = await _politicaPolly.ExecuteAsync(() => 
            {
                return _servicoIdentidade.BuscarUsuariosTfs(new ReadIdentitiesRequest(0, new string[] { usuario }, 0, 4, 1, null, 0), colecao);
            });

            var identidade = resultado.ReadIdentitiesResult.Any() && resultado.ReadIdentitiesResult[0].Any() ? resultado.ReadIdentitiesResult[0][0] : null;

            if (identidade is null)
                return null;

            return await IdentidadeParaUsuarioTfsAsync(identidade, true);
        }

        private async Task<UsuarioTfs> IdentidadeParaUsuarioTfsAsync(TeamFoundationIdentity identidade, bool obterColecoes)
        {
            var usuario = new UsuarioTfs
            {
                Ativo = identidade.IsActive,
                NomeUsuario = identidade.ObterPropriedade<string>("Account"),
                NomeCompleto = identidade.DisplayName,
                Dominio = identidade.ObterPropriedade<string>("Domain"),
                UltimoAcesso = identidade.ObterPropriedade<DateTime>("LastAccessedTime"),
                Identidade = new IdentidadeTfs
                {
                    Id = identidade.Descriptor.identifier,
                    Tipo = identidade.Descriptor.identityType
                },
                Email = !string.IsNullOrEmpty(identidade.ObterPropriedade<string>("Mail")) ?
                                    identidade.ObterPropriedade<string>("Mail") :
                                    identidade.ObterPropriedade<string>("ConfirmedNotificationAddress")
            };

            if(obterColecoes)
                usuario.Colecoes = await _servicoColecao.ObterColecoesPorUsuarioAsync(usuario);

            return usuario;
        }
    }
}
