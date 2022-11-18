using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.Tfs;
using CA.Repositorios.Tfs.Interfaces;

namespace CA.Repositorios.Tfs
{
    public class RepositorioColecoesCache : IRepositorioColecoes
    {
        private readonly IServicoColecaoTfsHttp _servicoColecao;

        public RepositorioColecoesCache(IServicoColecaoTfsHttp servicoColecao)
        {
            _servicoColecao = servicoColecao;            
        }

        public Task<string[]> ObterColecoesPorUsuarioAsync(UsuarioTfs usuario)
        {
            return _servicoColecao.ObterColecoesPorUsuarioAsync(usuario);
        }

        public Task<string[]> ObterTodasColecoesAsync()
        {
            return _servicoColecao.ObterTodasColecoesAsync();
        }
    }
}
