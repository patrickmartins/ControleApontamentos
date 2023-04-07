using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Jobs.Extensions;
using CA.Jobs.Interfaces;
using CA.Servicos.Channel.Interfaces;
using CA.Util.Extensions;
using Microsoft.Extensions.Logging;

namespace CA.Jobs
{
    public class JobCargaProjetos : Job<ProjetoChannel>
    {
        private readonly IRepositorioProjetos _repositorioProjetos;

        private readonly IServicoChannelHttp _servico;

        public JobCargaProjetos(IRepositorioProjetos repositorioProjetos, IServicoChannelHttp servico, ILogger<ProjetoChannel> logger) : base(logger)
        {
            _repositorioProjetos = repositorioProjetos;

            _servico = servico;
        }

        public override async Task ExecutarAsync()
        {
            LogarInformacao("===> Iniciando a execução do Job de Carga de Projetos. <===");

            var dataFim = DateOnly.FromDateTime(DateTime.Now.ConverterParaFusoBrasil());
            var dataInicio = dataFim.AddDays(-40);

            LogarInformacao($"Data de início: {dataInicio:d}.");
            LogarInformacao($"Data de fim: {dataFim:d}.");

            LogarInformacao("Obtendo projetos no Channel.");

            var projetosBanco = await _repositorioProjetos.ObterTodosProjetosAsync();
            var projetosServico = await _servico.ObterApontamentosPorPeriodoAsync(dataInicio, dataFim);

            var idsProjetosServico = projetosServico.Select(c => c.IdProjeto).Where(c => c > 0).Distinct().ToList();

            LogarInformacao($"{idsProjetosServico.Count} projetos obtidos no Channel.");

            var idsProjetosInserir = idsProjetosServico.Where(c => !projetosBanco.Any(p => p.Id == c)).ToList();
            var idsProjetosAtualizar = idsProjetosServico.Where(c => projetosBanco.Any(p => p.Id == c)).ToList();

            LogarInformacao($"{idsProjetosInserir.Count} projetos serão inseridos.");
            LogarInformacao($"{idsProjetosAtualizar.Count} projetos serão atualizados.");

            foreach (var id in idsProjetosInserir)
            {
                await ImportarProjetoPorIdAsync(id);
            }

            foreach (var id in idsProjetosAtualizar)
            {
                var projetobanco = projetosBanco.First(c => c.Id == id);

                await AtualizarProjetoAsync(projetobanco);
            }

            await _repositorioProjetos.SalvarAlteracoesAsync();

            LogarInformacao("===> Finalizando a execução do Job de Carga de Projetos. <====");
        }

        private async Task ImportarProjetoPorIdAsync(int id)
        {
            var projetoServico = await _servico.ObterProjetoPorIdAsync(id);

            if (projetoServico == null)
                return;

            var projetoNovo = projetoServico.ParaProjetoChannel();

            var resultado = projetoNovo.Validar();

            if (!resultado.Sucesso)
            {
                LogarErros($"Não foi possível atualizar o projeto {projetoNovo.Id}. Devido aos erros abaixo:");

                LogarErros(resultado.Erros.ToArray());
            }
            else
            {
                await _repositorioProjetos.InserirProjetoAsync(projetoNovo);
            }
        }

        private async Task AtualizarProjetoAsync(ProjetoChannel projetoBanco)
        {
            var response = await _servico.ObterProjetoPorIdAsync(projetoBanco.Id);

            if (response == null)
                return;

            var projetoServico = response.ParaProjetoChannel();

            var resultado = projetoServico.Validar();

            if (!resultado.Sucesso)
            {
                LogarErros($"Não foi possível atualizar o projeto {projetoServico.Id}. Devido aos erros abaixo:");

                LogarErros(resultado.Erros.ToArray());

                return;
            }

            await AtualizarAtividadesPorProjetoAsync(projetoBanco, projetoServico);

            if (projetoServico == projetoBanco)
                return;

            projetoBanco.Atualizar(projetoServico);

            _repositorioProjetos.AtualizarProjeto(projetoBanco);
        }

        private async Task AtualizarAtividadesPorProjetoAsync(ProjetoChannel projetoBanco, ProjetoChannel projetoServico)
        {
            LogarInformacao($"Iniciando à atualização das atividades do projeto {projetoBanco.Id}.");

            var atividadesServico = projetoServico.Atividades;
            var atividadesBanco = await _repositorioProjetos.ObterAtividadesPorIdsAsync(atividadesServico.Select(c => c.Id).ToArray());

            var atividadesInseridas = atividadesServico.Where(c => !atividadesBanco.Any(a => a.Id == c.Id)).ToList();
            var atividadesAtualizadas = atividadesServico.Where(atividadeServico => atividadesBanco.Any(atividadeBanco => atividadeBanco.Id == atividadeServico.Id && atividadeServico != atividadeBanco)).ToList();

            LogarInformacao($"{atividadesInseridas.Count} atividades serão inseridas no projeto {projetoBanco.Id}.");
            LogarInformacao($"{atividadesAtualizadas.Count} atividades serão atualizadas no projeto {projetoBanco.Id}.");

            foreach (var atividade in atividadesInseridas)
            {
                var resultado = atividade.Validar();

                if (resultado.Sucesso)
                {
                    projetoBanco.AdicionarAtividade(atividade);

                    LogarInformacao($"Atividade {atividade.Id} inserida no projeto {projetoBanco.Id}.");
                }
                else
                {
                    LogarInformacao($"Não foi possível inserir à atividade {atividade.Id}. Devido aos erros abaixo:");

                    LogarErros(resultado.Erros.ToArray());
                }
            }

            foreach (var atividadeServico in atividadesAtualizadas)
            {
                var atividadeBanco = atividadesBanco.First(c => c.Id == atividadeServico.Id);

                var resultado = atividadeServico.Validar();

                if (resultado.Sucesso)
                {
                    atividadeBanco.Atualizar(atividadeServico);
                    atividadeBanco.AlterarProjeto(projetoBanco);

                    LogarInformacao($"Atividade {atividadeBanco.Id} atualizada.");
                }
                else
                {
                    LogarErros($"Não foi possível atualizar à atividade {atividadeServico.Id}. Devido aos erros abaixo:");

                    LogarErros(resultado.Erros.ToArray());
                }
            }
        }
    }
}
