﻿using System.Collections.Generic;

namespace CA.Aplicacao.Models
{
    public record GrupoTarefasModel
    {
        public string Nome { get; set; } = string.Empty;
        public ICollection<TarefaModel> Tarefas { get; set; }

        public GrupoTarefasModel()
        {
            Tarefas = new HashSet<TarefaModel>();
        }
    }
}