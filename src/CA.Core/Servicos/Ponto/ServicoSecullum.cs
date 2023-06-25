using CA.Core.Entidades.Ponto;
using CA.Core.Interfaces.Ponto;
using CA.Core.Valores;

namespace CA.Core.Servicos.Ponto
{
    public class ServicoSecullum : IServicoPonto
    {
        private readonly IRepositorioPonto _repositorio;

        public ServicoSecullum(IRepositorioPonto repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Resultado<BatidasPontoDia?>> ObterBatidasPorDataAsync(string pisFuncionario, DateOnly data)
        {
            var resultadoFuncionario = await ObterFuncionarioPorPisAsync(pisFuncionario);

            if (!resultadoFuncionario.Sucesso)
                return Resultado.DeErros<BatidasPontoDia?>(resultadoFuncionario.Erros);

            if (string.IsNullOrEmpty(pisFuncionario))
                return Resultado.DeErros<BatidasPontoDia?>(new Erro("O pis do funcionário não foi informado.", nameof(pisFuncionario)));

            var resultadoBatidas = await _repositorio.ObterBatidasPorDataAsync(pisFuncionario, data);

            if (!resultadoBatidas.Sucesso)
                return Resultado.DeErros<BatidasPontoDia?>(resultadoBatidas.Erros);

            return Resultado.DeValor(resultadoBatidas.Valor);
        }

        public async Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim)
        {
            var resultado = await ObterFuncionarioPorPisAsync(pisFuncionario);

            if (!resultado.Sucesso)
                return Resultado.DeErros<IEnumerable<BatidasPontoDia>>(resultado.Erros);

            if (string.IsNullOrEmpty(pisFuncionario))
                return Resultado.DeErros<IEnumerable<BatidasPontoDia>>(new Erro("O pis do funcionário não foi informado.", nameof(pisFuncionario)));

            var resultadoBatidas = await _repositorio.ObterBatidasPorPeriodoAsync(pisFuncionario, inicio, fim);

            if (!resultadoBatidas.Sucesso)
                return Resultado.DeErros<IEnumerable<BatidasPontoDia>>(resultadoBatidas.Erros);

            return resultadoBatidas;
        }

        public async Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterTodasBatidasPorPeriodoAsync(DateOnly inicio, DateOnly fim)
        {
            var resultadoBatidas = await _repositorio.ObterTodasBatidasPorPeriodoAsync(inicio, fim);

            if (!resultadoBatidas.Sucesso)
                return Resultado.DeErros<IEnumerable<BatidasPontoDia>>(resultadoBatidas.Erros);

            return resultadoBatidas;
        }

        public async Task<Resultado<Funcionario?>> ObterFuncionarioPorIdAsync(int id)
        {
            if (id <= 0)
                return Resultado.DeErros<Funcionario?>(new Erro("O id do funcionário não foi informado.", nameof(id)));

            var funcionario = await _repositorio.ObterFuncionarioPorIdAsync(id);

            if (funcionario is null)
                return Resultado.DeErros<Funcionario?>(new Erro("O funcionário informado não foi encontrado no sistema de ponto.", nameof(funcionario)));

            return Resultado.DeValor<Funcionario?>(funcionario);
        }

        public async Task<Resultado<Funcionario?>> ObterFuncionarioPorNomeAsync(string nome)
        {
            if (string.IsNullOrEmpty(nome))
                return Resultado.DeErros<Funcionario?>(new Erro("O nome do funcionário não foi informado.", nameof(nome)));

            var funcionario = await _repositorio.ObterFuncionarioPorNomeAsync(nome);

            if (funcionario is null)
                return Resultado.DeErros<Funcionario?>(new Erro("O funcionário informado não foi encontrado no sistema de ponto.", nameof(funcionario)));

            return Resultado.DeValor<Funcionario?>(funcionario);
        }

        public async Task<Resultado<Funcionario?>> ObterFuncionarioPorPisAsync(string pisFuncionario)
        {
            if (string.IsNullOrEmpty(pisFuncionario))
                return Resultado.DeErros<Funcionario?>(new Erro("O PIS do funcionário não foi informado.", nameof(pisFuncionario)));

            var funcionario = await _repositorio.ObterFuncionarioPorPisAsync(pisFuncionario);

            if (funcionario is null)
                return Resultado.DeErros<Funcionario?>(new Erro("O funcionário informado não foi encontrado no sistema de ponto.", nameof(funcionario)));

            return Resultado.DeValor<Funcionario?>(funcionario);
        }

        public Task<IEnumerable<Funcionario>> ObterTodosFuncionariosAsync()
        {
            return _repositorio.ObterTodosFuncionariosAsync();
        }
    }
}