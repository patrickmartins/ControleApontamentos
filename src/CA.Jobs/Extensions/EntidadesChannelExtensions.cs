﻿using CA.Core.Entidades.Channel;

namespace CA.Jobs.Channel.Extensions
{
    internal static class EntidadesChannelExtensions
    {
        public static void Atualizar(this ProjetoChannel projeto1, ProjetoChannel projeto2)
        {
            if(projeto1 is null || projeto2 is null) 
                return;

            projeto1.Nome = projeto2.Nome;
            projeto1.Status = projeto2.Status;
        }

        public static void Atualizar(this AtividadeChannel atividade1, AtividadeChannel atividade2)
        {
            if (atividade1 is null || atividade1 is null)
                return;

            atividade1.Nome = atividade2.Nome;
            atividade1.Codigo = atividade2.Codigo;            
        }

        public static void Atualizar(this ApontamentoChannel apontamento1, ApontamentoChannel apontamento2)
        {
            if (apontamento1 is null || apontamento1 is null)
                return;
                        
            apontamento1.Data = apontamento2.Data;
            apontamento1.Comentario = apontamento2.Comentario;
            apontamento1.TempoApontado = apontamento2.TempoApontado;
            apontamento1.Atividade= apontamento2.Atividade;
        }
    }
}
