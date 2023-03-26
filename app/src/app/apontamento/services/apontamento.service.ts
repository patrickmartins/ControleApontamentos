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

@Injectable({
	providedIn: 'root'
})
export class ApontamentoService extends BaseService {

	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	public obterApontamentosTfsPorDia(data: Date): Observable<ApontamentosTfsDia> {
        return this.get<any>(`${environment.urlApiBase}apontamento/tfs/por-dia`, ApontamentosTfsDia, { }, { data: `${data.getFullYear()}-${data.getMonth()+1}-${data.getDate()}` }).pipe(
			map(apontamento => this.configurarApontamentosTfsPorDia(apontamento))
		);
    }

    public obterApontamentosTfsDeUsuarioPorDia(id: string, data: Date): Observable<ApontamentosTfsDia> {
        return this.get<any>(`${environment.urlApiBase}apontamento/tfs/${id}/por-dia`, ApontamentosTfsDia, { }, { data: `${data.getFullYear()}-${data.getMonth()+1}-${data.getDate()}` }).pipe(
			map(apontamento => this.configurarApontamentosTfsPorDia(apontamento))
		);
    }

	public obterApontamentosTfsPorMes(mes: number, ano: number): Observable<ApontamentosTfsMes> {
        return this.get<any>(`${environment.urlApiBase}apontamento/tfs/por-mes`, ApontamentosTfsMes, { }, { mes: mes, ano: ano }).pipe(
			map(apontamento => this.configurarApontamentosTfsPorMes(apontamento))
		);
    }

    public obterApontamentosTfsDeUsuarioPorMes(id: string, mes: number, ano: number): Observable<ApontamentosTfsMes> {
        return this.get<any>(`${environment.urlApiBase}apontamento/tfs/${id}/por-mes`, ApontamentosTfsMes, { }, { mes: mes, ano: ano }).pipe(
			map(apontamento => this.configurarApontamentosTfsPorMes(apontamento))
		);
    }

	public obterApontamentosChannelPorDia(data: Date): Observable<ApontamentosChannelDia> {
        return this.get<any>(`${environment.urlApiBase}apontamento/channel/por-dia`, ApontamentosChannelDia, { }, { data: `${data.getFullYear()}-${data.getMonth()+1}-${data.getDate()}` });
    }

    public obterApontamentosChannelDeUsuarioPorDia(id: string, data: Date): Observable<ApontamentosChannelDia> {
        return this.get<any>(`${environment.urlApiBase}apontamento/channel/${id}/por-dia`, ApontamentosChannelDia, { }, { data: `${data.getFullYear()}-${data.getMonth()+1}-${data.getDate()}` });
    }

	public obterApontamentosChannelPorMes(mes: number, ano: number): Observable<ApontamentosChannelMes> {
        return this.get<any>(`${environment.urlApiBase}apontamento/channel/por-mes`, ApontamentosChannelMes, { }, { mes: mes, ano: ano });
    }

    public obterApontamentosChannelDeUsuarioPorMes(id: string, mes: number, ano: number): Observable<ApontamentosChannelMes> {
        return this.get<any>(`${environment.urlApiBase}apontamento/channel/${id}/por-mes`, ApontamentosChannelMes, { }, { mes: mes, ano: ano });
    }

    public consolidarTarefasEAtividadesDia(apontamentosTfs: ApontamentosTfsDia | undefined, apontamentosChannel: ApontamentosChannelDia | undefined): void {
		if(!apontamentosTfs || !apontamentosChannel)
			return;

        let apontamentosChannelTfs = apontamentosChannel.obterApontamentosTfs();
		
		apontamentosChannelTfs.forEach(apontamento => {
			let tarefas = apontamentosTfs.obterTarefasPorId(apontamento.idTarefaTfs);

			tarefas.forEach(tarefa => {
				tarefa.removerApontamentoPorHash(apontamento.hash);
			});
		})
		
		apontamentosChannel.removerApontamentosExcluidos();
		apontamentosChannel.removerTarefasSemApontamentos();
		apontamentosChannel.recalcularTempoTotalApontado();

		apontamentosTfs.removerTarefasSemApontamentos();
		apontamentosTfs.recalcularTempoTotalApontado();	
	}

    public consolidarTarefasEAtividadesMes(apontamentosTfs: ApontamentosTfsMes | undefined, apontamentosChannel: ApontamentosChannelMes | undefined): void {
		if (apontamentosTfs && apontamentosChannel) {
            let apontamentosChannelTfs = apontamentosChannel.obterApontamentosTfs();
            
            apontamentosChannelTfs = apontamentosChannelTfs.filter((apontamento, i) => apontamentosChannelTfs.findIndex(c => c.hash === apontamento.hash) === i);

            apontamentosChannelTfs.forEach(apontamento => {
                let tarefas = apontamentosTfs.obterTarefasPorId(apontamento.idTarefaTfs);

                tarefas.forEach(tarefa => {
                    tarefa.removerApontamentoPorHash(apontamento.hash);
                });
            });

            apontamentosChannel.removerApontamentosExcluidos();
            apontamentosChannel.removerTarefasSemApontamentos();
            apontamentosChannel.recalcularTempoTotalApontado();

            apontamentosTfs.removerTarefasSemApontamentos();
            apontamentosTfs.recalcularTempoTotalApontado();
        }
        else if (apontamentosChannel) {
            apontamentosChannel.removerApontamentosExcluidos();
            apontamentosChannel.removerTarefasSemApontamentos();
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
		})

		return apontamento;
	}
}
