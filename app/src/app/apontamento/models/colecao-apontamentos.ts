import { ApontamentoChannel } from "src/app/core/models/apontamento-channel";
import { ApontamentoTfs } from "src/app/core/models/apontamento-tfs";

export interface IColecaoApontamentos {
    removerApontamentoPorIdTfs(idTfs: string): void;
    recalcularTempoTotalApontado(): void;    
}

export interface IColecaoApontamentosChannel extends IColecaoApontamentos {
    obterApontamentosTfs(): ApontamentoChannel[];
    removerAtividadesSemApontamentos(): void;
    removerApontamentosExcluidos(): void;
    obterTodosApontamentos(): ApontamentoChannel[];
}

export interface IColecaoApontamentosTfs extends IColecaoApontamentos {
    removerTarefasSemApontamentos(): void;
    obterTodosApontamentos(): ApontamentoTfs[];
}