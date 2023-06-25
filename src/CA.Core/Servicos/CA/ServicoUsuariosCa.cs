using CA.Core.Entidades.CA;
using CA.Core.Interfaces.CA;
using CA.Core.Interfaces.Channel;
using CA.Core.Valores;

namespace CA.Core.Servicos.CA
{
    public class ServicoUsuariosCa : IServicoUsuariosCa
    {
        private readonly IRepositorioUsuariosCa _repositorio;

        public ServicoUsuariosCa(IRepositorioUsuariosCa repositorioCa)
        {
            _repositorio = repositorioCa;
        }

        public async Task<Resultado<UsuarioCA>> AtualizarUsuarioAsync(UsuarioCA usuario)
        {
            var resultado = usuario.Validar();
            
            if (!resultado.Sucesso)
                return Resultado.DeErros<UsuarioCA>(resultado.Erros);

            _repositorio.AtualizarUsuario(usuario);
            await _repositorio.SalvarAlteracoesAsync();

            return Resultado.DeValor(usuario);
        }

        public async Task<Resultado<UsuarioCA>> AdicionarUsuarioAsync(UsuarioCA usuario)
        {
            var resultado = usuario.Validar();

            if (!resultado.Sucesso)
                return Resultado.DeErros<UsuarioCA>(resultado.Erros);

            await _repositorio.InserirUsuarioAsync(usuario);
            await _repositorio.SalvarAlteracoesAsync();

            return Resultado.DeValor(usuario);
        }

        public Task<IEnumerable<UsuarioCA>> ObterTodosUsuariosAsync()
        {
            return _repositorio.ObterTodosUsuariosAsync();
        }

        public Task<IEnumerable<UsuarioCA>> ObterTodosGerentesAsync()
        {
            return _repositorio.ObterTodosGerentesAsync();
        }

        public Resultado<UsuarioCA?> ObterUsuarioPorId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Resultado.DeErros<UsuarioCA?>(new Erro("O id do usuário não foi informado.", nameof(id)));

            var usuario = _repositorio.ObterUsuarioPorId(id);

            if (usuario is null)
                return Resultado.DeErros<UsuarioCA?>(new Erro("O usuário informado não foi encontrada.", nameof(id)));

            return Resultado.DeValor<UsuarioCA?>(usuario);
        }

        public Resultado<UsuarioCA?> ObterUsuarioPorEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Resultado.DeErros<UsuarioCA?>(new Erro("O email do usuário não foi informado.", nameof(email)));

            var usuario = _repositorio.ObterUsuarioPorEmail(email);

            if (usuario is null)
                return Resultado.DeErros<UsuarioCA?>(new Erro("O usuário informado não foi encontrada.", nameof(email)));

            return Resultado.DeValor<UsuarioCA?>(usuario);
        }

        public Task<IEnumerable<UsuarioCA>> ObterUsuariosPorUnidadeAsync(string idUnidade)
        {
            return _repositorio.ObterUsuarioPorUnidadeAsync(idUnidade);
        }

        public async Task<Resultado> ExcluirUsuarioPorIdAsync(string id)
        {
            var usuario = _repositorio.ObterUsuarioPorId(id);

            if (usuario is null)
                return Resultado.DeErros<UsuarioCA>(new Erro("O usuário informado não existe.", nameof(id)));

            _repositorio.ExcluirUsuario(usuario);
            await _repositorio.SalvarAlteracoesAsync();

            return Resultado.DeSucesso();
        }
    }
}
