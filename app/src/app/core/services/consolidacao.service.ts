import { Injectable } from '@angular/core';
import { IColecaoApontamentosTfs, IColecaoApontamentosChannel } from 'src/app/apontamento/models/colecao-apontamentos';
import { StatusApontamento } from '../models/status-apontamento';

@Injectable({
    providedIn: 'root'
})
export class ConsolidacaoService {

    constructor() { }

    public consolidarTarefasEAtividades(apontamentosTfs: IColecaoApontamentosTfs | undefined, apontamentosChannel: IColecaoApontamentosChannel | undefined): void {
        if (apontamentosTfs && apontamentosChannel) {
            let todosApontamentosChannelTfs = apontamentosChannel.obterApontamentosTfs();
            let todosApontamentosTfs = apontamentosTfs.obterTodosApontamentos();

            for (let apontamentoTfs of todosApontamentosTfs) {
                let apontamentoChannelTfs = todosApontamentosChannelTfs.find(c => c.hash == apontamentoTfs.hash);

                if (apontamentoChannelTfs) {
                    if (apontamentoChannelTfs.status == StatusApontamento.Inserido) {
                        apontamentosChannel.removerApontamentoPorHash(apontamentoChannelTfs.hash);
                    }

                    if (apontamentoChannelTfs.status == StatusApontamento.Alterado || apontamentoChannelTfs.status == StatusApontamento.Excluido) {
                        apontamentosTfs.removerApontamentoPorHash(apontamentoChannelTfs.hash);
                    }
                }
            }

            apontamentosChannel.removerApontamentosExcluidos();
            apontamentosChannel.removerAtividadesSemApontamentos();
            apontamentosChannel.recalcularTempoTotalApontado();

            apontamentosTfs.removerTarefasSemApontamentos();
            apontamentosTfs.recalcularTempoTotalApontado();
        }
        else if (apontamentosChannel) {
            apontamentosChannel.removerApontamentosExcluidos();
            apontamentosChannel.removerAtividadesSemApontamentos();
            apontamentosChannel.recalcularTempoTotalApontado();
        }
    }
}
