using CA.Core.Entidades.Ponto;
using CA.Core.Interfaces.Ponto;
using CA.Core.Valores;
using CA.Servicos.Secullum.Interfaces;
using CA.Util.Extensions;

namespace CA.Repositorios.Ponto
{
    public class RepositorioPonto : IRepositorioPonto
    {
        private readonly IServicoSecullumHttp _servico;

        public RepositorioPonto(IServicoSecullumHttp servico)
        {
            _servico = servico;
        }

        public async Task<Resultado<BatidasPontoDia?>> ObterBatidasPorDataAsync(string pisFuncionario, DateOnly data)
        {
            var resultado = await _servico.ObterBatidasPorPeriodoAsync(pisFuncionario, data, data);

            if(!resultado.Sucesso)            
                return Resultado.DeErros<BatidasPontoDia?>(resultado.Erros);            

            return Resultado.DeValor(resultado.Valor.FirstOrDefault());
        }

        public Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim)
        {
            return _servico.ObterBatidasPorPeriodoAsync(pisFuncionario, inicio, fim);
        }

        public Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterTodasBatidasPorPeriodoAsync(DateOnly inicio, DateOnly fim)
        {
            return _servico.ObterTodasBatidasPorPeriodoAsync(inicio, fim);
        }

        public async Task<Funcionario?> ObterFuncionarioPorIdAsync(int id)
        {
            var funcionarios = await _servico.ObterFuncionariosAsync();

            return funcionarios.OrderBy(c => c.Id).LastOrDefault(c => c.Id == id);
        }

        public async Task<Funcionario?> ObterFuncionarioPorNomeAsync(string nome)
        {
            var funcionarios = await _servico.ObterFuncionariosAsync();

            return funcionarios.OrderBy(c => c.Id).LastOrDefault(c => c.Nome.Trim().RemoverEspacosDuplicados().ToLower().Equals(nome.ToLower()));
        }

        public async Task<Funcionario?> ObterFuncionarioPorPisAsync(string pisFuncionario)
        {
            var funcionarios = await _servico.ObterFuncionariosAsync();

            return funcionarios.OrderBy(c => c.Id).LastOrDefault(c => c.NumeroPis.Equals(pisFuncionario));
        }

        public async Task<IEnumerable<Funcionario>> ObterTodosFuncionariosAsync()
        {
            return (await _servico.ObterFuncionariosAsync()).Where(c => !string.IsNullOrEmpty(c.NumeroPis)).ToList();
        }
    }
}
