using CA.Aplicacao.Enums;
using CA.Aplicacao.Models;
using CA.Core.Valores;

namespace CA.Aplicacao.Interfaces
{
    public interface IServicoRelatorioApp
    {
        Task<Resultado<IEnumerable<RelatorioApontamentosUsuarioPorMes>>> ObterRelatorioDeApontamentosPorMesAsync(int mes, int ano, TipoOrdenacaoRelatorio ordenacao, bool somenteApontamentosSincronizados = false, bool somenteUsuariosComCadastroNoPonto = false, bool somenteUsuariosComTempoTrabalhado = false);
    }
}