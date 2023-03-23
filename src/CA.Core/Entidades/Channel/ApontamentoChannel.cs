﻿using CA.Core.Valores;
using System.Globalization;

namespace CA.Core.Entidades.Channel
{
    public enum TipoApontamento
    {
        Avulso = 0,
        Atividade,
        Projeto
    }

    public enum StatusApontamento
    {
        Inserido = 0,
        Alterado,
        Excluido
    }

    public class ApontamentoChannel
    {
        public int Id { get; set; }
        public int? IdTarefaTfs { get; set; }
        public TipoApontamento Tipo { get; set; }
        public bool ApontamentoTfs { get; set; }
        public DateTime Data { get; set; }
        public string Comentario { get; set; }
        public StatusApontamento Status { get; set; }
        public TimeSpan TempoApontado { get; set; }
        public UsuarioChannel Usuario { get; set; }
        public ProjetoChannel? Projeto { get; set; }
        public AtividadeChannel? Atividade { get; set; }

        public string Hash { get; set; }

        public Resultado Validar()
        {
            var erros = new List<Erro>();

            if (Id <= 0)
                erros.Add(new Erro("O id não foi informado.", nameof(Id)));

            if (Usuario == null)
                erros.Add(new Erro("O usuário não foi informado.", nameof(Usuario)));

            if (Tipo == TipoApontamento.Atividade && Atividade is null)
                erros.Add(new Erro("A atividade não foi informada.", nameof(Atividade)));

            if (Tipo == TipoApontamento.Projeto && Projeto is null)
                erros.Add(new Erro("O projeto não foi informada.", nameof(Projeto)));

            if (Comentario.Length > 2000)
                erros.Add(new Erro("O comentário contém mais de 1000 caracteres.", nameof(Comentario)));

            if (TempoApontado.Ticks == 0)
                erros.Add(new Erro("O tempo apontado não foi informado.", nameof(TempoApontado)));

            return Resultado.DeErros(erros);
        }

        public void Excluir()
        {
            Status = StatusApontamento.Excluido;
        }

        public void Restaurar()
        {
            Status = StatusApontamento.Inserido;
        }

        public override bool Equals(object? obj)
        {            
            if (obj is null || GetType() != obj.GetType()) 
                return false;
            
            var apontamento = (ApontamentoChannel)obj;
            
            if (ReferenceEquals(this, apontamento)) 
                return true;

            return apontamento.Id == Id &&
                    apontamento.Data == Data &&
                    apontamento.ApontamentoTfs == ApontamentoTfs &&
                    apontamento.TempoApontado.Ticks == TempoApontado.Ticks &&
                    apontamento.Tipo == Tipo &&
                    apontamento.Projeto == Projeto &&
                    apontamento.Atividade == Atividade &&
                    apontamento.Usuario == Usuario &&
                    string.Compare(apontamento.Comentario, Comentario, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0;
        }

        public static bool operator == (ApontamentoChannel apontamento1, ApontamentoChannel apontamento2)
        {
            if (apontamento1 is null && apontamento2 is null)
                return true;

            if ((apontamento1 is null && apontamento2 is not null) || (apontamento1 is not null && apontamento2 is null))
                return false;

            return apontamento1.Equals(apontamento2);
        }

        public static bool operator != (ApontamentoChannel apontamento1, ApontamentoChannel apontamento2)
        {
            if (apontamento1 is null && apontamento2 is null)
                return true;

            if ((apontamento1 is null && apontamento2 is not null) || (apontamento1 is not null && apontamento2 is null))
                return false;

            return !apontamento1.Equals(apontamento2);
        }
    }
}