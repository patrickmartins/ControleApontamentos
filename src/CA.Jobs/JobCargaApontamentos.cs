using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Core.Valores;
using CA.Jobs.Channel.Extensions;
using CA.Jobs.Channel.Interfaces;
using CA.Servicos.Channel.Interfaces;

namespace CA.Jobs.Channel
{
    public class JobCargaApontamentos : IJobChannel<ApontamentoChannel>
    {
        private readonly IRepositorioApontamentos _repositorioApontamentos;
        private readonly IRepositorioUsuariosChannel _repositorioUsuarios;
        private readonly IRepositorioProjetos _repositorioProjetos;

        private readonly IServicoChannelHttp _servico;

        public JobCargaApontamentos(IRepositorioApontamentos repositorioApontamentos, 
                                    IRepositorioUsuariosChannel repositorioUsuarios, 
                                    IRepositorioProjetos repositorioProjetos, 
                                    IServicoChannelHttp servico)
        {
            _repositorioApontamentos = repositorioApontamentos;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioProjetos = repositorioProjetos;

            _servico = servico;
        }

        public async Task ExecutarAsync()
        {
            var resultado = Resultado.DeSucesso();

            //var dataFim = DateOnly.FromDateTime(DateTime.Now);
            //var dataInicio = new DateOnly(dataFim.Year, dataFim.Month, 1);

            var ano = 2023;
            var mes = 1;

            var dataFim = new DateOnly(ano, mes, DateTime.DaysInMonth(ano, mes));
            var dataInicio = new DateOnly(dataFim.Year, dataFim.Month, 1);

            var apontamentosServico = await _servico.ObterApontamentosPorPeriodoAsync(dataInicio, dataFim);

            var usuariosBanco = await _repositorioUsuarios.ObterTodosUsuariosAsync();            
            var apontamentosBanco = await _repositorioApontamentos.ObterTodosApontamentosPorPeriodoAsync(dataInicio, dataFim);            
            var atividadesBanco = await _repositorioProjetos.ObterAtividadesPorProjetoAsync(apontamentosServico.Select(c => c.IdProjeto).Distinct().ToArray());

            var novosApontamentos = apontamentosServico.Where(c => c.IdProjeto > 0 && !c.EhApontamentoTfs() && !apontamentosBanco.Any(x => x.Id == c.Id)).ParaApontamentosChannel(atividadesBanco, usuariosBanco);

            var apontamentosServicoAtualizados = apontamentosServico.Where(c => apontamentosBanco.Any(x => x.Id == c.Id))
                                                                    .ParaApontamentosChannel(atividadesBanco, usuariosBanco)
                                                                    .Where(apontamentoServico => apontamentosBanco.Any(apontamentoBanco => apontamentoBanco.Id == apontamentoServico.Id 
                                                                                                                                                && apontamentoBanco != apontamentoServico)).ToList();

            var apontamentosExcluidos = apontamentosBanco.Where(c => !apontamentosServico.Any(x => x.Id == c.Id)).ToList();

            var apontamentosBancoAtualizados = apontamentosBanco.Where(c => apontamentosServicoAtualizados.Any(x => x.Id == c.Id)).ToList();

            foreach (var apontamento in novosApontamentos)
            {
                resultado = apontamento.Validar();

                if (!resultado.Sucesso)
                    return;
            }

            foreach (var apontamentoServico in apontamentosServicoAtualizados)
            {
                var apontamentoBanco = apontamentosBancoAtualizados.First(c => c.Id == apontamentoServico.Id);

                apontamentoBanco.Atualizar(apontamentoServico);

                resultado = apontamentoBanco.Validar();

                if (!resultado.Sucesso)
                    return;
            }

            _repositorioApontamentos.AtualizarApontamentos(apontamentosBancoAtualizados);
            _repositorioApontamentos.RemoverApontamentos(apontamentosExcluidos);

            await _repositorioApontamentos.InserirApontamentosAsync(novosApontamentos);
            await _repositorioApontamentos.SalvarAlteracoesAsync();
        }
    }
}
