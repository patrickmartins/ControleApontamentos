using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Core.Valores;

namespace CA.Core.Servicos.Channel
{
    public class ServicoChannel : IServicoChannel
    {
        private readonly IRepositorioUsuariosChannel _repositorioUsuario;
        private readonly IRepositorioApontamentos _repositorioApontamentos;
        private readonly IRepositorioProjetos _repositorioProjetos;

        public ServicoChannel(IRepositorioApontamentos repositorioApontamentos, IRepositorioProjetos repositorioProjetos, IRepositorioUsuariosChannel repositorioUsuario)
        {
            _repositorioApontamentos = repositorioApontamentos;
            _repositorioProjetos = repositorioProjetos;
            _repositorioUsuario = repositorioUsuario;            
        }

        public async Task<Resultado<IEnumerable<ApontamentoChannel>>> ObterTodosApontamentosPorPeriodoAsync(DateOnly inicio, DateOnly fim)
        {
            if (fim < inicio)
                return Resultado.DeErros<IEnumerable<ApontamentoChannel>>(new Erro("A data de fim não pode ser menor que a data de ínicio.", nameof(fim)));

            var apontamentos = await _repositorioApontamentos.ObterTodosApontamentosPorPeriodoAsync(inicio, fim);

            return Resultado.DeValor(apontamentos);
        }

        public async Task<Resultado<IEnumerable<ApontamentoChannel>>> ObterApontamentosPorDataAsync(int idUsuario, DateOnly data)
        {
            if(idUsuario <= 0)
                return Resultado.DeErros<IEnumerable<ApontamentoChannel>>(new Erro("O id do usuário não foi informado.", nameof(idUsuario)));

            var apontamentos = await _repositorioApontamentos.ObterApontamentosPorDataAsync(idUsuario, data);

            return Resultado.DeValor(apontamentos);
        }

        public async Task<Resultado<IEnumerable<ApontamentoChannel>>> ObterApontamentosPorPeriodoAsync(int idUsuario, DateOnly inicio, DateOnly fim)
        {
            if (idUsuario <= 0)
                return Resultado.DeErros<IEnumerable<ApontamentoChannel>>(new Erro("O id do usuário não foi informado.", nameof(idUsuario)));

            if(fim < inicio)
                return Resultado.DeErros<IEnumerable<ApontamentoChannel>>(new Erro("A data de fim não pode ser menor que a data de ínicio.", nameof(fim)));

            var apontamentos = await _repositorioApontamentos.ObterApontamentosPorPeriodoAsync(idUsuario, inicio, fim);

            return Resultado.DeValor(apontamentos);
        }

        public Task<IEnumerable<UsuarioChannel>> ObterTodosUsuariosAsync()
        {
            return _repositorioUsuario.ObterUsuariosAtivosAsync();
        }

        public Resultado<UsuarioChannel?> ObterUsuarioPorId(int id)
        {
            if (id <= 0)
                return Resultado.DeErros<UsuarioChannel?>(new Erro("O id do usuário não foi informado.", nameof(id)));

            var usuario = _repositorioUsuario.ObterUsuarioPorId(id);

            if(usuario is null)
                return Resultado.DeErros<UsuarioChannel?>(new Erro("O funcionário informado não foi encontrado.", nameof(id)));

            return Resultado.DeValor<UsuarioChannel?>(usuario);
        }

        public Resultado<UsuarioChannel?> ObterUsuarioPorEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Resultado.DeErros<UsuarioChannel?>(new Erro("O email do usuário não foi informado.", nameof(email)));

            var usuario = _repositorioUsuario.ObterUsuarioPorEmail(email);

            if(usuario is null)
                return Resultado.DeErros<UsuarioChannel?>(new Erro("O usuário do Channel informado não foi encontrado.", nameof(email)));

            return Resultado.DeValor<UsuarioChannel?>(usuario);
        }

        public Resultado<UsuarioChannel?> ObterUsuarioPorNomeCompleto(string nomeCompleto)
        {
            if (string.IsNullOrEmpty(nomeCompleto))
                return Resultado.DeErros<UsuarioChannel?>(new Erro("O nome completo do usuário não foi informado.", nameof(nomeCompleto)));

            var usuario = _repositorioUsuario.ObterUsuarioPorNomeCompleto(nomeCompleto);

            if (usuario is null)
                return Resultado.DeErros<UsuarioChannel?>(new Erro("O usuário do Channel informado não foi encontrado.", nameof(nomeCompleto)));

            return Resultado.DeValor<UsuarioChannel?>(usuario);
        }

        public async Task<Resultado<IEnumerable<AtividadeChannel>>> ObterAtividadesApontadasPorUsuarioPorDiaAsync(int idUsuario, DateOnly data)
        {
            if (idUsuario <= 0)
                return Resultado.DeErros<IEnumerable<AtividadeChannel>>(new Erro("O id do usuário não foi informado.", nameof(idUsuario)));

            var apontamentos = await _repositorioProjetos.ObterAtividadesApontadasPorPorUsuarioPorDiaAsync(idUsuario, data);

            return Resultado.DeValor(apontamentos);
        }

        public async Task<Resultado<IEnumerable<AtividadeChannel>>> ObterAtividadesApontadasPorUsuarioPorPeriodoAsync(int idUsuario, DateOnly inicio, DateOnly fim)
        {
            if (idUsuario <= 0)
                return Resultado.DeErros<IEnumerable<AtividadeChannel>>(new Erro("O id do usuário não foi informado.", nameof(idUsuario)));

            if (fim < inicio)
                return Resultado.DeErros<IEnumerable<AtividadeChannel>>(new Erro("A data de fim não pode ser menor que a data de ínicio.", nameof(fim)));

            var apontamentos = await _repositorioProjetos.ObterAtividadesApontadasPorPorUsuarioPorPeriodoAsync(idUsuario, inicio, fim);

            return Resultado.DeValor(apontamentos);
        }
    }
}
