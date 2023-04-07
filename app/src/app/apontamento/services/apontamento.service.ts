import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { BaseService } from 'src/app/common/services/base.service';
import { TarefaHelper } from 'src/app/helpers/tarefa.helper';
import { environment } from 'src/environments/environment';
import { ApontamentosChannelDia } from '../models/apontamentos-channel-dia';
import { ApontamentosChannelMes } from '../models/apontamentos-channel-mes';
import { ApontamentosTfsDia } from '../models/apontamentos-tfs-dia';
import { ApontamentosTfsMes } from '../models/apontamentos-tfs-mes';
import { StatusApontamento } from 'src/app/core/models/status-apontamento';
import { IColecaoApontamentosChannel, IColecaoApontamentosTfs } from '../models/colecao-apontamentos';

@Injectable({
    providedIn: 'root'
})
export class ApontamentoService extends BaseService {

    constructor(httpClient: HttpClient) {
        super(httpClient);
    }

    public obterApontamentosTfsPorDia(data: Date): Observable<ApontamentosTfsDia> {
        return this.get<any>(`${environment.urlApiBase}apontamento/tfs/por-dia`, ApontamentosTfsDia, {}, { data: `${data.getFullYear()}-${data.getMonth() + 1}-${data.getDate()}` }).pipe(
            map(apontamento => this.configurarApontamentosTfsPorDia(apontamento))
        );
    }

    public obterApontamentosTfsDeUsuarioPorDia(id: string, data: Date): Observable<ApontamentosTfsDia> {
        return this.get<any>(`${environment.urlApiBase}apontamento/tfs/${id}/por-dia`, ApontamentosTfsDia, {}, { data: `${data.getFullYear()}-${data.getMonth() + 1}-${data.getDate()}` }).pipe(
            map(apontamento => this.configurarApontamentosTfsPorDia(apontamento))
        );
    }

    public obterApontamentosTfsPorMes(mes: number, ano: number): Observable<ApontamentosTfsMes> {
        return this.get<any>(`${environment.urlApiBase}apontamento/tfs/por-mes`, ApontamentosTfsMes, {}, { mes: mes, ano: ano }).pipe(
            map(apontamento => this.configurarApontamentosTfsPorMes(apontamento))
        );
    }

    public obterApontamentosTfsDeUsuarioPorMes(id: string, mes: number, ano: number): Observable<ApontamentosTfsMes> {
        return this.get<any>(`${environment.urlApiBase}apontamento/tfs/${id}/por-mes`, ApontamentosTfsMes, {}, { mes: mes, ano: ano }).pipe(
            map(apontamento => this.configurarApontamentosTfsPorMes(apontamento))
        );
    }

    public obterApontamentosChannelPorDia(data: Date): Observable<ApontamentosChannelDia> {
        return this.get<any>(`${environment.urlApiBase}apontamento/channel/por-dia`, ApontamentosChannelDia, {}, { data: `${data.getFullYear()}-${data.getMonth() + 1}-${data.getDate()}` });
    }

    public obterApontamentosChannelDeUsuarioPorDia(id: string, data: Date): Observable<ApontamentosChannelDia> {
        return this.get<any>(`${environment.urlApiBase}apontamento/channel/${id}/por-dia`, ApontamentosChannelDia, {}, { data: `${data.getFullYear()}-${data.getMonth() + 1}-${data.getDate()}` });
    }

    public obterApontamentosChannelPorMes(mes: number, ano: number): Observable<ApontamentosChannelMes> {
        return this.get<any>(`${environment.urlApiBase}apontamento/channel/por-mes`, ApontamentosChannelMes, {}, { mes: mes, ano: ano });
    }

    public obterApontamentosChannelDeUsuarioPorMes(id: string, mes: number, ano: number): Observable<ApontamentosChannelMes> {
        return this.get<any>(`${environment.urlApiBase}apontamento/channel/${id}/por-mes`, ApontamentosChannelMes, {}, { mes: mes, ano: ano });
    }

    public consolidarTarefasEAtividades(apontamentosTfs: IColecaoApontamentosTfs | undefined, apontamentosChannel: IColecaoApontamentosChannel | undefined): void {
        if (apontamentosTfs && apontamentosChannel) {
            let todosApontamentosChannelTfs = apontamentosChannel.obterApontamentosTfs().distinct(c => c.hash);
            let todosApontamentosTfs = apontamentosTfs.obterTodosApontamentos().distinct(c => c.hash);

            for (let apontamentoTfs of todosApontamentosTfs) {
                let apontamentoChannelTfs = todosApontamentosChannelTfs.find(c => c.hash == apontamentoTfs.hash);

                if(apontamentoChannelTfs) {
                    if(apontamentoChannelTfs.status == StatusApontamento.Inserido) {
                        apontamentosChannel.removerApontamentoPorHash(apontamentoChannelTfs.hash);
                    }

                    if(apontamentoChannelTfs.status == StatusApontamento.Alterado || apontamentoChannelTfs.status == StatusApontamento.Excluido) {
                        apontamentosTfs.removerApontamentoPorHash(apontamentoChannelTfs.hash);
                    }
                }
                else {
                    if(apontamentoTfs.data >= new Date(2023, 1, 15))
                        apontamentosTfs.removerApontamentoPorHash(apontamentoTfs.hash);
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

    private configurarApontamentosTfsPorDia(apontamento: ApontamentosTfsDia): ApontamentosTfsDia {
        const tarefasFixadas = TarefaHelper.obterTarefasFixadas();

        apontamento.tarefas.forEach(tarefa => {
            tarefa.fixada = tarefasFixadas.some(tarefaFixada => tarefaFixada.idTarefa == tarefa.id && tarefaFixada.colecao == tarefa.colecao);
        });

        return apontamento;
    }

    private configurarApontamentosTfsPorMes(apontamento: ApontamentosTfsMes): ApontamentosTfsMes {
        const tarefasFixadas = TarefaHelper.obterTarefasFixadas();

        apontamento.apontamentosDiarios.forEach(apontamentos => {
            apontamentos.tarefas.forEach(tarefa => {
                tarefa.fixada = tarefasFixadas.some(tarefaFixada => tarefaFixada.idTarefa == tarefa.id && tarefaFixada.colecao == tarefa.colecao);
            });
        });

        return apontamento;
    }
}
