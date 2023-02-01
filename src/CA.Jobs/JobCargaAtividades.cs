using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Core.Valores;
using CA.Jobs.Channel.Extensions;
using CA.Jobs.Channel.Interfaces;
using CA.Servicos.Channel.Interfaces;

namespace CA.Jobs.Channel
{
    public class JobCargaAtividades : IJobChannel<AtividadeChannel>
    {
        private readonly IRepositorioProjetos _repositorioProjetos;

        private readonly IServicoChannelHttp _servico;

        public JobCargaAtividades(IRepositorioProjetos repositorioProjetos, IServicoChannelHttp servico)
        {
            _repositorioProjetos = repositorioProjetos;

            _servico = servico;
        }

        public async Task ExecutarAsync()
        {
            var resultado = Resultado.DeSucesso();

            var projetosBanco = await _repositorioProjetos.ObterTodosProjetosAsync();

            foreach (var projeto in projetosBanco)
            {
                var atividadesBanco = await _repositorioProjetos.ObterAtividadesPorProjetoAsync(projeto.Id);
                var atividadesServico = await _servico.ObterAtividadesPorProjetoAsync(projeto.Id);

                var atividadesNovas = atividadesServico.Where(c => !atividadesBanco.Any(x => x.Id == c.Id)).ParaAtividadesChannel(projeto);

                var atividadesServicoAtualizados = atividadesServico.Where(c => atividadesBanco.Any(x => x.Id == c.Id)).ParaAtividadesChannel(projeto)
                                                                    .Where(atividadeServico => atividadesBanco.Any(atividadeBanco => atividadeBanco.Id == atividadeServico.Id && atividadeBanco != atividadeServico)).ToList();

                var atividadesBancoAtualizados = atividadesBanco.Where(c => atividadesServicoAtualizados.Any(x => x.Id == c.Id)).ToList();

                foreach (var atividade in atividadesNovas)
                {
                    resultado = atividade.Validar();

                    if (!resultado.Sucesso)
                        return;
                }

                foreach (var atividadeServico in atividadesServicoAtualizados)
                {
                    var atividadeBanco = atividadesBancoAtualizados.First(c => c.Id == atividadeServico.Id);

                    atividadeBanco.Atualizar(atividadeServico);

                    resultado = atividadeBanco.Validar();

                    if (!resultado.Sucesso)
                        return;
                }

                _repositorioProjetos.AtualizarAtividades(atividadesBancoAtualizados);

                await _repositorioProjetos.InserirAtividadesAsync(atividadesNovas);
                await _repositorioProjetos.SalvarAlteracoesAsync();
            }
        }
    }
}
