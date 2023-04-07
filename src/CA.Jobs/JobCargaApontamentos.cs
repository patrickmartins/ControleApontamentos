using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Jobs.Extensions;
using CA.Jobs.Interfaces;
using CA.Servicos.Channel.Interfaces;
using CA.Servicos.Channel.Models.Responses;
using CA.Util.Extensions;
using Microsoft.Extensions.Logging;

namespace CA.Jobs
{
    public class JobCargaApontamentos : Job<ApontamentoChannel>
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

            var dataFim = DateOnly.FromDateTime(DateTime.Now.ConverterParaFusoBrasil());
            var dataInicio = dataFim.AddDays(-40);

            LogarInformacao($"Data de início: {dataInicio:d}.");
            LogarInformacao($"Data de fim: {dataFim:d}.");

            LogarInformacao("Obtendo apontamentos no Channel.");

            var apontamentosServico = await _servico.ObterApontamentosPorPeriodoAsync(dataInicio, dataFim);
            var apontamentosBanco = await _repositorioApontamentos.ObterTodosApontamentosPorPeriodoAsync(dataInicio, dataFim);

            LogarInformacao($"{apontamentosServico.Count()} apontamentos obtidos no Channel.");

            var usuarios = await _repositorioUsuarios.ObterTodosUsuariosAsync();
            var projetos = await _repositorioProjetos.ObterProjetosPorIdsAsync(apontamentosServico.Select(c => c.IdProjeto).Distinct().ToArray());

            var apontamentosInseridos = ExtrairApontamentosInseridos(apontamentosServico, apontamentosBanco, projetos, usuarios);
            var apontamentosRestaurados = ExtrairApontamentosRestaurados(apontamentosServico, apontamentosBanco, projetos, usuarios);
            var apontamentoAtualizados = ExtrairApontamentosAtualizados(apontamentosServico, apontamentosBanco, projetos, usuarios);
            var apontamentosExcluidos = ExtrairApontamentosExcluidos(apontamentosServico, apontamentosBanco);

            LogarInformacao($"{apontamentosInseridos.Count()} apontamentos serão inseridos.");
            LogarInformacao($"{apontamentosRestaurados.Count()} apontamentos serão restaurados.");
            LogarInformacao($"{apontamentoAtualizados.Count()} apontamentos serão atualizados.");
            LogarInformacao($"{apontamentosExcluidos.Count()} apontamentos serão excluídos.");

            if (apontamentosExcluidos.Any())
            {
                LogarInformacao($"===> Excluindo apontamentos <===");

                foreach (var apontamento in apontamentosExcluidos)
                {
                    apontamento.Excluir();

                    LogarInformacao($"Apontamento {apontamento.Id} excluído.");
                }

                _repositorioApontamentos.AtualizarApontamentos(apontamentosExcluidos);
            }

            if (apontamentosRestaurados.Any())
            {
                LogarInformacao($"===> Restaurando apontamentos <===");

                foreach (var apontamento in apontamentosRestaurados)
                {
                    apontamento.Restaurar();

                    LogarInformacao($"Apontamento {apontamento.Id} restaurado.");
                }

                _repositorioApontamentos.AtualizarApontamentos(apontamentosRestaurados);
            }

            if (apontamentosInseridos.Any())
            {
                LogarInformacao($"===> Inserindo apontamentos <===");

                foreach (var apontamento in apontamentosInseridos)
                {
                    var resultado = apontamento.Validar();

                    if (resultado.Sucesso)
                    {
                        await _repositorioApontamentos.InserirApontamentoAsync(apontamento);

                        LogarInformacao($"Apontamento {apontamento.Id} inserido.");
                    }
                    else
                    {
                        LogarInformacao($"Não foi possível inserir o apontamento {apontamento.Id}. Devido aos erros abaixo:");

                        LogarErros(resultado.Erros.ToArray());
                    }
                }
            }

            if (apontamentoAtualizados.Any())
            {
                LogarInformacao($"===> Atualizando apontamentos <===");

                foreach (var apontamentoServico in apontamentoAtualizados)
                {
                    var apontamentoBanco = apontamentosBanco.First(c => c.Id == apontamentoServico.Id);

                    var resultado = apontamentoServico.Validar();

                    if (resultado.Sucesso)
                    {
                        apontamentoBanco.Atualizar(apontamentoServico);

                        _repositorioApontamentos.AtualizarApontamento(apontamentoBanco);

                        LogarInformacao($"Apontamento {apontamentoBanco.Id} atualizado.");
                    }
                    else
                    {
                        LogarErros($"Não foi possível atualizar o apontamento {apontamentoServico.Id}. Devido aos erros abaixo:");

                        LogarErros(resultado.Erros.ToArray());
                    }
                }
            }

            await _repositorioApontamentos.SalvarAlteracoesAsync();

            LogarInformacao("Finalizando a execução do Job de Carga de Apontamentos.");
        }

        private IEnumerable<ApontamentoChannel> ExtrairApontamentosInseridos(IEnumerable<ApontamentoResponse> apontamentosServico, IEnumerable<ApontamentoChannel> apontamentosBanco, IEnumerable<ProjetoChannel> projetos, IEnumerable<UsuarioChannel> usuarios)
        {
            return apontamentosServico.Where(c => !apontamentosBanco.Any(x => x.Id == c.Id)).ParaApontamentosChannel(projetos, usuarios);
        }

        private IEnumerable<ApontamentoChannel> ExtrairApontamentosRestaurados(IEnumerable<ApontamentoResponse> apontamentosServico, IEnumerable<ApontamentoChannel> apontamentosBanco, IEnumerable<ProjetoChannel> projetos, IEnumerable<UsuarioChannel> usuarios)
        {
            return apontamentosBanco.Where(c => apontamentosServico.Any(x => x.Id == c.Id) && c.Status == StatusApontamento.Excluido).ToList();
        }

        private IEnumerable<ApontamentoChannel> ExtrairApontamentosAtualizados(IEnumerable<ApontamentoResponse> apontamentosServico, IEnumerable<ApontamentoChannel> apontamentosBanco, IEnumerable<ProjetoChannel> projetos, IEnumerable<UsuarioChannel> usuarios)
        {
            return apontamentosServico.Where(c => apontamentosBanco.Any(x => x.Id == c.Id && x.Status != StatusApontamento.Excluido))
                                                                .ParaApontamentosChannel(projetos, usuarios)
                                                                .Where(apontamentoServico => apontamentosBanco.Any(apontamentoBanco => apontamentoBanco.Id == apontamentoServico.Id && apontamentoBanco != apontamentoServico))
                                                                .ToList();
        }

        private IEnumerable<ApontamentoChannel> ExtrairApontamentosExcluidos(IEnumerable<ApontamentoResponse> apontamentosServico, IEnumerable<ApontamentoChannel> apontamentosBanco)
        {
            return apontamentosBanco.Where(c => !apontamentosServico.Any(x => x.Id == c.Id) && c.Status != StatusApontamento.Excluido).ToList();
        }
    }
}
