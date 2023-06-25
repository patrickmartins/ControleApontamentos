using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Repositorios.Contexto;
using CA.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios.Channel
{
    public class RepositorioProjetos : RepositorioBase, IRepositorioProjetos
    {
        public RepositorioProjetos(ContextoDadosCA contexto) : base(contexto) {  }

        public Task InserirProjetoAsync(ProjetoChannel projeto)
        {
            return InserirAsync(projeto);
        }

        public Task InserirProjetosAsync(IEnumerable<ProjetoChannel> projetos)
        {
            return InserirAsync(projetos);
        }

        public void AtualizarProjeto(ProjetoChannel projeto)
        {
            Atualizar(projeto);
        }

        public void AtualizarProjetos(IEnumerable<ProjetoChannel> projetos)
        {
            Atualizar(projetos);
        }

        public Task InserirAtividadeAsync(AtividadeChannel atividade)
        {
            return InserirAsync(atividade);
        }

        public Task InserirAtividadesAsync(IEnumerable<AtividadeChannel> atividades)
        {
            return InserirAsync(atividades);
        }

        public void AtualizarAtividade(AtividadeChannel atividade)
        {
            Atualizar(atividade);
        }

        public void AtualizarAtividades(IEnumerable<AtividadeChannel> atividades)
        {
            Atualizar(atividades);
        }

        public Task<IEnumerable<ProjetoChannel>> ObterTodosProjetosAsync()
        {
            return Set<ProjetoChannel>().Include(c => c.Atividades).ToIListAsync();
        }

        public Task<IEnumerable<ProjetoChannel>> ObterProjetosPorIdsAsync(params int[] idsProjetos)
        {
            return Set<ProjetoChannel>().Include(c => c.Atividades).Where(c => idsProjetos.Contains(c.Id)).ToIListAsync();
        }

        public Task<IEnumerable<ProjetoChannel>> ObterTodosProjetosAtivosAsync()
        {
            return Set<ProjetoChannel>().Where(c => c.Status != StatusProjeto.Finalizado).ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterTodasAtividadesAsync()
        {
            return Set<AtividadeChannel>().ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterAtividadesPorIdsAsync(params int[] ids)
        {
            return Set<AtividadeChannel>().Where(c => ids.Contains(c.Id)).Include(c => c.Projeto).ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterAtividadesPorCodigoAsync(string codigo)
        {
            return Set<AtividadeChannel>().Where(c => c.Codigo == codigo).ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterAtividadesPorProjetoAsync(params int[] idsProjetos)
        {
            return Set<AtividadeChannel>().Where(c => idsProjetos.Contains(c.Projeto.Id)).Include(c => c.Projeto).ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterAtividadesApontadasPorPorUsuarioPorDiaAsync(int idUsuario, DateOnly data)
        {
            return Set<AtividadeChannel>()
                        .Include(c => c.Projeto)
                        .Include(c => c.Apontamentos.Where(a => a.Usuario.Id == idUsuario
                                        && a.Data == data.ToDateTime(new TimeOnly(0))))
                        .ThenInclude(c => c.Usuario)
                        .ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterAtividadesApontadasPorPorUsuarioPorPeriodoAsync(int idUsuario, DateOnly inicio, DateOnly fim)
        {
            return Set<AtividadeChannel>()
                        .Include(c => c.Projeto)
                        .Include(c => c.Apontamentos.Where(a => a.Usuario.Id == idUsuario
                                        && a.Data >= inicio.ToDateTime(new TimeOnly(0))
                                        && a.Data <= fim.ToDateTime(new TimeOnly(0))))
                        .ThenInclude(c => c.Usuario)
                        .ToIListAsync();
        }
    }
}
