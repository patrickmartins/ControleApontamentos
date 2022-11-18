using CA.Core.Entidades.Tfs;

namespace CA.Core.Interfaces.Tfs
{
    public interface IRepositorioUsuariosTfs : IRepositorioTfs
    {
        Task<UsuarioTfs?> ObterUsuarioAsync(string colecao, string usuario);
    }
}