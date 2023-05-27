using CA.Aplicacao.Extensions;
using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Models;
using CA.Core.Entidades.Ponto;
using CA.Core.Interfaces.Ponto;
using CA.Core.Valores;
using CA.Util.Extensions;

namespace CA.Aplicacao.Servicos
{
    public class ServicoSecullumApp : IServicoPontoApp
    {
        private readonly IServicoPonto _servico;

        public ServicoSecullumApp(IServicoPonto servico)
        {
            _servico = servico;
        }

        public async Task<Resultado<BatidasPontoDiaModel>> ObterBatidasPorDiaAsync(string pisFuncionario, DateOnly data)
        {
            var resultado = await _servico.ObterBatidasPorDataAsync(pisFuncionario, data);

            if (!resultado.Sucesso)
                return new Resultado<BatidasPontoDiaModel>(resultado.Erros);

            var batidas = resultado.Valor;

            return batidas != null ? Resultado.DeValor(batidas.BatidasPontoParaBatidasPontoModel()) : Resultado.DeValor(new BatidasPontoDiaModel());
        }

        public async Task<Resultado<BatidasPontoMesModel>> ObterBatidasPorMesAsync(string pisFuncionario, int mes, int ano)
        {
            if (mes < 1 || mes > 12)
                return new Resultado<BatidasPontoMesModel>(new Erro("O mês informado é inválido.", nameof(mes)));

            if (ano < 2000 || ano > DateTime.Today.Year)
                return new Resultado<BatidasPontoMesModel>(new Erro("O ano informado é inválido.", nameof(ano)));

            var hoje = DateTime.Now.ConverterParaFusoBrasil();
            var dataInicio = new DateOnly(ano, mes, 1);
            var dataFim = hoje.Month == mes ? new DateOnly(ano, mes, hoje.Day) : new DateOnly(ano, mes, DateTime.DaysInMonth(ano, mes));
            
            var resultado = await _servico.ObterBatidasPorPeriodoAsync(pisFuncionario, dataInicio, dataFim);

            if (!resultado.Sucesso)
                return new Resultado<BatidasPontoMesModel>(resultado.Erros);

            var batidas = AdequarBatidasPorPeriodo(resultado.Valor, dataInicio, dataFim);

            return batidas != null ? Resultado.DeValor(batidas.BatidasPontoParaBatidasPontoMesModel(mes, ano)) : Resultado.DeValor(new BatidasPontoMesModel());
        }

        public Task<Resultado<Funcionario?>> ObterFuncionarioPorNomeAsync(string nome)
        {
            return _servico.ObterFuncionarioPorNomeAsync(nome);
        }

        public async Task<IEnumerable<Funcionario>> ObterTodosFuncionariosAsync()
        {
            var funcionarios = await _servico.ObterTodosFuncionariosAsync();

            return funcionarios.Select(c => new Funcionario
            {
                Id = c.Id,
                Nome = c.Nome,
                NumeroFolha = c.NumeroFolha
            });
        }

        private IEnumerable<BatidasPontoDia> AdequarBatidasPorPeriodo(IEnumerable<BatidasPontoDia> batidas, DateOnly dataInicio, DateOnly dataFim)
        {
            var batidasPeriodo = batidas.OrderBy(c => c.Data).ToList();

            for (var dia = dataInicio; dia <= dataFim; dia = dia.AddDays(1))
            {
                if (!batidasPeriodo.Any(c => c.Data.Day == dia.Day && c.Data.Month == dia.Month && c.Data.Year == dia.Year))
                {
                    batidasPeriodo.Add(new BatidasPontoDia
                    {
                        Data = dia.ToDateTime(new TimeOnly())
                    });
                }
            }

            return batidasPeriodo.OrderBy(c => c.Data).ToList();
        }
    }
}
