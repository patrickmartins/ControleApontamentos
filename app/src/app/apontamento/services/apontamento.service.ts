import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { BaseService } from 'src/app/common/services/base.service';
import { TarefaHelper } from 'src/app/helpers/tarefa.helper';
import { environment } from 'src/environments/environment';
import { ApontamentosDia } from '../models/apontamentos-dia';
import { ApontamentosMes } from '../models/apontamentos-mes';

@Injectable({
	providedIn: 'root'
})
export class ApontamentoService extends BaseService {

	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	public obterApontamentosPorDia(data: Date): Observable<ApontamentosDia> {
        return this.get<any>(`${environment.urlApiBase}apontamento/por-dia`, ApontamentosDia, { }, { data: `${data.getFullYear()}-${data.getMonth()+1}-${data.getDate()}` }).pipe(
			map(apontamento => this.configurarApontamentosPorDia(apontamento))
		);
    }

	public obterApontamentosPorMes(mes: number, ano: number): Observable<ApontamentosMes> {
        return this.get<any>(`${environment.urlApiBase}apontamento/por-mes`, ApontamentosMes, { }, { mes: mes, ano: ano }).pipe(
			map(apontamento => this.configurarApontamentosPorMes(apontamento))
		);
    }
	
	private configurarApontamentosPorDia(apontamento: ApontamentosDia): ApontamentosDia {
		const tarefasFixadas = TarefaHelper.obterTarefasFixadas();

		apontamento.tarefas.forEach(tarefa => {
			tarefa.fixada = tarefasFixadas.some(tarefaFixada => tarefaFixada.idTarefa == tarefa.id && tarefaFixada.colecao == tarefa.colecao);
		});	

		return apontamento;
	}

	private configurarApontamentosPorMes(apontamento: ApontamentosMes): ApontamentosMes {
		const tarefasFixadas = TarefaHelper.obterTarefasFixadas();

		apontamento.apontamentosDiarios.forEach(apontamentos => {
			apontamentos.tarefas.forEach(tarefa => {
				tarefa.fixada = tarefasFixadas.some(tarefaFixada => tarefaFixada.idTarefa == tarefa.id && tarefaFixada.colecao == tarefa.colecao);
			});	
		})

		return apontamento;
	}
}
