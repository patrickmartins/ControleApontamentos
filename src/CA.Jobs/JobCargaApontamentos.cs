using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Core.Valores;
using CA.Jobs.Channel.Extensions;
using CA.Jobs.Channel.Interfaces;
using CA.Servicos.Channel.Interfaces;
using CA.Servicos.Channel.Models.Responses;
using Microsoft.Extensions.Logging;

namespace CA.Jobs.Channel
{
    public class JobCargaApontamentos : JobChannel<ApontamentoChannel>
    {
        private readonly IRepositorioApontamentos _repositorioApontamentos;
        private readonly IRepositorioUsuariosChannel _repositorioUsuarios;
        private readonly IRepositorioProjetos _repositorioProjetos;

        private readonly IServicoChannelHttp _servico;

        public JobCargaApontamentos(IRepositorioApontamentos repositorioApontamentos, 
                                    IRepositorioUsuariosChannel repositorioUsuarios, 
                                    IRepositorioProjetos repositorioProjetos, 
                                    IServicoChannelHttp servico,
                                    ILogger<ApontamentoChannel> logger) : base(logger)
        {
            _repositorioApontamentos = repositorioApontamentos;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioProjetos = repositorioProjetos;

            _servico = servico;
        }

        public override async Task ExecutarAsync()
        {
            LogarInformacao("Iniciando a execução do Job de Carga de Apontamentos.");

            var resultado = Resultado.DeSucesso();

            var dataFim = DateOnly.FromDateTime(DateTime.Now);
            var dataInicio = new DateOnly(dataFim.Year, dataFim.Month, 1);

            LogarInformacao("Obtendo apontamentos no Channel.");

            var apontamentosServico = await _servico.ObterApontamentosPorPeriodoAsync(dataInicio, dataFim);
            var apontamentosBanco = await _repositorioApontamentos.ObterTodosApontamentosPorPeriodoAsync(dataInicio, dataFim);

            LogarInformacao($"{apontamentosServico.Count()} apontamentos obtidos no Channel.");

            var usuarios = await _repositorioUsuarios.ObterTodosUsuariosAsync();
            var projetos = await _repositorioProjetos.ObterProjetosPorIdsAsync(apontamentosServico.Select(c => c.IdProjeto).Distinct().ToArray());
                        
            var apontamentosInseridos = ExtrairApontamentosInseridos(apontamentosServico, apontamentosBanco, projetos, usuarios);
            var apontamentoAtualizados = ExtrairApontamentosAtualizados(apontamentosServico, apontamentosBanco, projetos, usuarios);
            var apontamentosExcluidos = ExtrairApontamentosExcluidos(apontamentosServico, apontamentosBanco);

            LogarInformacao($@"{apontamentosInseridos.Count()} apontamentos serão inseridos.");
            LogarInformacao(@$"{apontamentoAtualizados.Count()} apontamentos serão atualizados.");
            LogarInformacao($@"{apontamentosExcluidos.Count()} apontamentos serão excluídos.");

            foreach (var apontamento in apontamentosExcluidos)
            {
                apontamento.Excluir();
            }

            _repositorioApontamentos.AtualizarApontamentos(apontamentosExcluidos);

            foreach (var apontamento in apontamentosInseridos)
            {
                resultado = apontamento.Validar();

                if (resultado.Sucesso)
                {
                    await _repositorioApontamentos.InserirApontamentoAsync(apontamento);
                }
                else
                {
                    LogarInformacao(@$"Não foi possível inserir o apontamento {apontamento.Id}. Devido aos erros abaixo:");

                    LogarErros(resultado.Erros.ToArray());
                }
            }
                        
            foreach (var apontamentoServico in apontamentoAtualizados)
            {
                var apontamentoBanco = apontamentosBanco.First(c => c.Id == apontamentoServico.Id);

                resultado = apontamentoServico.Validar();

                if (resultado.Sucesso)
                {
                    apontamentoBanco.Atualizar(apontamentoServico);

                    _repositorioApontamentos.AtualizarApontamento(apontamentoBanco);
                }
                else
                {
                    LogarErros(@$"Não foi possível atualizar o apontamento {apontamentoServico.Id}. Devido aos erros abaixo:");

                    LogarErros(resultado.Erros.ToArray());
                }
            }

            await _repositorioApontamentos.SalvarAlteracoesAsync();

            LogarInformacao("Finalizando a execução do Job de Carga de Apontamentos.");
        }

        private IEnumerable<ApontamentoChannel> ExtrairApontamentosInseridos(IEnumerable<ApontamentoResponse> apontamentosServico, IEnumerable<ApontamentoChannel> apontamentosBanco, IEnumerable<ProjetoChannel> projetos, IEnumerable<UsuarioChannel> usuarios)
        {
            return apontamentosServico.Where(c => !apontamentosBanco.Any(x => x.Id == c.Id)).ParaApontamentosChannel(projetos, usuarios);
        }

        private IEnumerable<ApontamentoChannel> ExtrairApontamentosAtualizados(IEnumerable<ApontamentoResponse> apontamentosServico, IEnumerable<ApontamentoChannel> apontamentosBanco, IEnumerable<ProjetoChannel> projetos, IEnumerable<UsuarioChannel> usuarios)
        {
            return apontamentosServico.Where(c => apontamentosBanco.Any(x => x.Id == c.Id))
                                                                .ParaApontamentosChannel(projetos, usuarios)
                                                                .Where(apontamentoServico => apontamentosBanco.Any(apontamentoBanco => apontamentoBanco.Id == apontamentoServico.Id && apontamentoBanco != apontamentoServico))
                                                                .ToList();
        }

        private IEnumerable<ApontamentoChannel> ExtrairApontamentosExcluidos(IEnumerable<ApontamentoResponse> apontamentosServico, IEnumerable<ApontamentoChannel> apontamentosBanco)
        {
            return apontamentosBanco.Where(c => !apontamentosServico.Any(x => x.Id == c.Id)).ToList();
        }
    }
}
