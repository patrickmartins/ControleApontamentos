using CA.Core.Entidades.Channel;

namespace CA.Jobs.Extensions
{
    internal static class EntidadesChannelExtensions
    {
        public static void Atualizar(this ProjetoChannel projeto1, ProjetoChannel projeto2)
        {
            if (projeto1 is null || projeto2 is null)
                return;

            projeto1.Nome = projeto2.Nome;
            projeto1.Status = projeto2.Status;
        }

        public static void Atualizar(this UsuarioChannel usuario1, UsuarioChannel usuario2)
        {
            if (usuario1 is null || usuario1 is null)
                return;

            usuario1.Email = usuario2.Email;
            usuario1.NomeUsuario = usuario2.NomeUsuario; ;
            usuario1.NomeCompleto = usuario2.NomeCompleto;
            usuario1.Ativo = usuario2.Ativo;
        }

        public static void Atualizar(this AtividadeChannel atividade1, AtividadeChannel atividade2)
        {
            if (atividade1 is null || atividade1 is null)
                return;

            atividade1.Nome = atividade2.Nome;
            atividade1.Codigo = atividade2.Codigo;
            atividade1.Projeto = atividade2.Projeto;
        }

        public static void Atualizar(this ApontamentoChannel apontamento1, ApontamentoChannel apontamento2)
        {
            if (apontamento1 is null || apontamento1 is null)
                return;

            if (apontamento1.IdApontamentoTfs != apontamento2.IdApontamentoTfs)
                apontamento1.Status = StatusApontamento.Alterado;

            apontamento1.Data = apontamento2.Data;
            apontamento1.Comentario = apontamento2.Comentario;
            apontamento1.TempoApontado = apontamento2.TempoApontado;
            apontamento1.Tipo = apontamento2.Tipo;
            apontamento1.ProjetoId = apontamento2.ProjetoId;
            apontamento1.Projeto = apontamento2.Projeto;
            apontamento1.AtividadeId = apontamento2.AtividadeId;
            apontamento1.Atividade = apontamento2.Atividade;
            apontamento1.UsuarioId = apontamento2.UsuarioId;
            apontamento1.Usuario = apontamento2.Usuario;
        }
    }
}
