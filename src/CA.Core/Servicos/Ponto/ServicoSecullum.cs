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
            var resultado = await ObterFuncionarioPorPisAsync(pisFuncionario);

            if (!resultado.Sucesso)
                return Resultado.DeErros<BatidasPontoDia?>(resultado.Erros);

            if (string.IsNullOrEmpty(pisFuncionario))
                return Resultado.DeErros<BatidasPontoDia?>(new Erro("O pis do funcionário não foi informado.", nameof(pisFuncionario)));

            var batidas = await _repositorio.ObterBatidasPorDataAsync(pisFuncionario, data);

            return Resultado.DeValor(batidas);
        }

        public async Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim)
        {
            var resultado = await ObterFuncionarioPorPisAsync(pisFuncionario);

            if (!resultado.Sucesso)
                return Resultado.DeErros<IEnumerable<BatidasPontoDia>>(resultado.Erros);

            if (string.IsNullOrEmpty(pisFuncionario))
                return Resultado.DeErros<IEnumerable<BatidasPontoDia>>(new Erro("O pis do funcionário não foi informado.", nameof(pisFuncionario)));

            var batidas = (await _repositorio.ObterBatidasPorPeriodoAsync(pisFuncionario, inicio, fim)).OrderBy(c => c.Data).ToList();

            return Resultado.DeValor<IEnumerable<BatidasPontoDia>>(batidas);
        }

        public async Task<Resultado<Funcionario?>> ObterFuncionarioPorNomeAsync(string nome)
        {
            if (string.IsNullOrEmpty(nome))
                return Resultado.DeErros<Funcionario?>(new Erro("O nome do funcionário não foi informado.", nameof(nome)));

            var funcionario = await _repositorio.ObterFuncionarioPorNomeAsync(nome);

            return Resultado.DeValor(funcionario);
        }

        public async Task<Resultado<Funcionario?>> ObterFuncionarioPorPisAsync(string pisFuncionario)
        {
            if (string.IsNullOrEmpty(pisFuncionario))
                return Resultado.DeErros<Funcionario?>(new Erro("O PIS do funcionário não foi informado.", nameof(pisFuncionario)));

            var funcionario = await _repositorio.ObterFuncionarioPorPisAsync(pisFuncionario);

            if (funcionario is null)
                return Resultado.DeErros<Funcionario?>(new Erro("O funcionário informado não foi encontrado no sistema.", nameof(funcionario)));

            if (funcionario.FoiDemitido())
                return Resultado.DeErros<Funcionario?>(new Erro("O funcionário informado não faz mais parte do quadro de funcionários da empresa.", nameof(funcionario)));

            return Resultado.DeValor(funcionario);
        }
    }
}