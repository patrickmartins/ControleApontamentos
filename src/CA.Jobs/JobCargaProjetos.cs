using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Core.Valores;
using CA.Jobs.Channel.Extensions;
using CA.Jobs.Channel.Interfaces;
using CA.Servicos.Channel.Interfaces;

namespace CA.Jobs.Channel
{
    public class JobCargaProjetos : IJobChannel<ProjetoChannel>
    {
        private readonly IRepositorioProjetos _repositorioProjetos;

        private readonly IServicoChannelHttp _servico;

        public JobCargaProjetos(IRepositorioProjetos repositorioProjetos, IServicoChannelHttp servico)
        {
            _repositorioProjetos = repositorioProjetos;

            _servico = servico;
        }

        public async Task ExecutarAsync()
        {
            var resultado = Resultado.DeSucesso();

            var projetosServico = await _servico.ObterProjetosAsync();
            var projetosBanco = await _repositorioProjetos.ObterTodosProjetosAsync();

            var projetosNovos = projetosServico.Where(c => !projetosBanco.Any(x => x.Id == c.Id)).ParaProjetosChannel();
                        
            var projetosServicoAtualizados = projetosServico.Where(c => projetosBanco.Any(x => x.Id == c.Id)).ParaProjetosChannel()
                                                     .Where(projetoServico => projetosBanco.Any(projetoBanco => projetoBanco.Id == projetoServico.Id && projetoBanco != projetoServico)).ToList();

            var projetosBancoAtualizados = projetosBanco.Where(c => projetosServicoAtualizados.Any(x => x.Id == c.Id)).ToList();

            foreach (var projeto in projetosNovos)
            {
                resultado = projeto.Validar();

                if (!resultado.Sucesso)
                    return;
            }

            foreach (var projetoServico in projetosServicoAtualizados)
            {
                var projetoBanco = projetosBancoAtualizados.First(c => c.Id== projetoServico.Id);

                projetoBanco.Atualizar(projetoServico);

                resultado = projetoBanco.Validar();

                if (!resultado.Sucesso)
                    return;
            }

            _repositorioProjetos.AtualizarProjetos(projetosBancoAtualizados);

            await _repositorioProjetos.InserirProjetosAsync(projetosNovos);            
            await _repositorioProjetos.SalvarAlteracoesAsync();
        }
    }
}
