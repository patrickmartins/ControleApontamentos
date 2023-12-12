import { Injectable } from '@angular/core';
import { IColecaoApontamentosTfs, IColecaoApontamentosChannel } from 'src/app/apontamento/models/colecao-apontamentos';
import { StatusApontamento } from '../models/status-apontamento';

@Injectable({
    providedIn: 'root'
})
export class ConsolidacaoService {

    constructor() { }

    public consolidarTarefasEAtividades(apontamentosTfs: IColecaoApontamentosTfs | undefined, apontamentosChannel: IColecaoApontamentosChannel | undefined, dataUltimaSincronizacaoChannel: Date | undefined): void {
        if (apontamentosTfs && apontamentosChannel) {
            let todosApontamentosChannelTfs = apontamentosChannel.obterApontamentosTfs();
            let todosApontamentosTfs = apontamentosTfs.obterTodosApontamentos();

            for (let apontamentoTfs of todosApontamentosTfs) {
                let apontamentoChannelTfs = todosApontamentosChannelTfs.find(c => c.idTfs == apontamentoTfs.idTfs);

                if (apontamentoChannelTfs) {
                    if (apontamentoChannelTfs.status == StatusApontamento.Inserido) {
                        apontamentosChannel.removerApontamentoPorIdTfs(apontamentoChannelTfs.idTfs);
                    }

                    if (apontamentoChannelTfs.status == StatusApontamento.Alterado || apontamentoChannelTfs.status == StatusApontamento.Excluido) {
                        apontamentosTfs.removerApontamentoPorIdTfs(apontamentoChannelTfs.idTfs);
                    }
                }
                else {
                    if (apontamentoTfs.sincronizadoChannel && (!dataUltimaSincronizacaoChannel || (apontamentoTfs.dataApropriacao && apontamentoTfs.dataApropriacao < dataUltimaSincronizacaoChannel)))
                        apontamentosTfs.removerApontamentoPorIdTfs(apontamentoTfs.idTfs);
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
