import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';

import { BaseService } from 'src/app/common/services/base.service';
import { TarefaHelper } from 'src/app/helpers/tarefa.helper';
import { GrupoTarefas } from 'src/app/tarefa/models/grupos-tarefas';
import { environment } from 'src/environments/environment';
import { FiltroBusca } from '../models/filtro-busca';
import { NovoApontamento } from '../models/novo-apontamento';
import { PaginaBusca } from '../models/pagina-busca';
import { Tarefa } from '../models/tarefa';
import { ApontamentosTfsDia } from 'src/app/apontamento/models/apontamentos-tfs-dia';
import { ApontamentosTfsMes } from 'src/app/apontamento/models/apontamentos-tfs-mes';
import { UsuarioTfs } from '../models/usuarioTfs';
import { ApontamentoTfs } from '../models/apontamento-tfs';

@Injectable({
  providedIn: 'root'
})
export class TfsService extends BaseService {

	constructor(httpClient: HttpClient) {
		super(httpClient);
	}
    
	public obterTarefasAtivas(): Observable<GrupoTarefas[]> {
		return this.get<GrupoTarefas[]>(`${environment.urlApiBase}tfs/tarefa/ativas`, GrupoTarefas);
	}

	public obterTarefasPorIds(colecao: string, ids: number[]): Observable<Tarefa[]> {
		return this.get<Tarefa[]>(`${environment.urlApiBase}tfs/tarefa/por-ids`, Tarefa, {}, { colecao: colecao, ids: ids }).pipe(
			map(tarefas => this.configurarTarefas(tarefas))
		);
	}

	public buscarTarefas(filtro: FiltroBusca): Observable<PaginaBusca> {
		return this.get<PaginaBusca>(`${environment.urlApiBase}tfs/tarefa/buscar`, PaginaBusca, {}, filtro).pipe(
			map(pagina => this.configurarTarefasBusca(pagina)));
	}

	public salvarApontamento(apontamento: NovoApontamento): Observable<ApontamentoTfs> {
		return this.post<ApontamentoTfs>(`${environment.urlApiBase}tfs/tarefa/adicionar-apontamento`, apontamento, {}, ApontamentoTfs);
	}
    
    public obterApontamentosTfsPorDia(data: Date): Observable<ApontamentosTfsDia> {
        return this.get<any>(`${environment.urlApiBase}tfs/apontamento/por-dia`, ApontamentosTfsDia, {}, { data: `${data.getFullYear()}-${data.getMonth() + 1}-${data.getDate()}` }).pipe(
            map(apontamento => this.configurarApontamentosTfsPorDia(apontamento))
        );
    }

    public obterApontamentosTfsDeUsuarioPorDia(id: string, data: Date): Observable<ApontamentosTfsDia> {
        return this.get<any>(`${environment.urlApiBase}tfs/apontamento/${id}/por-dia`, ApontamentosTfsDia, {}, { data: `${data.getFullYear()}-${data.getMonth() + 1}-${data.getDate()}` }).pipe(
            map(apontamento => this.configurarApontamentosTfsPorDia(apontamento))
        );
    }

    public obterApontamentosTfsPorMes(mes: number, ano: number): Observable<ApontamentosTfsMes> {
        return this.get<any>(`${environment.urlApiBase}tfs/apontamento/por-mes`, ApontamentosTfsMes, {}, { mes: mes, ano: ano }).pipe(
            map(apontamento => this.configurarApontamentosTfsPorMes(apontamento))
        );
    }

    public obterApontamentosTfsDeUsuarioPorMes(id: string, mes: number, ano: number): Observable<ApontamentosTfsMes> {
        return this.get<any>(`${environment.urlApiBase}tfs/apontamento/${id}/por-mes`, ApontamentosTfsMes, {}, { mes: mes, ano: ano }).pipe(
            map(apontamento => this.configurarApontamentosTfsPorMes(apontamento))
        );
    }

    public obterTodosUsuarios(): Observable<UsuarioTfs[]> {
		return this.get<UsuarioTfs[]>(`${environment.urlApiBase}tfs/usuarios`, UsuarioTfs);
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

	private configurarTarefas(tarefas: Tarefa[]): Tarefa[] {
		const tarefasFixadas = TarefaHelper.obterTarefasFixadas();

		tarefas.forEach(tarefa => {
			tarefa.fixada = tarefasFixadas.some(tarefaFixada => tarefaFixada.idTarefa == tarefa.id && tarefaFixada.colecao == tarefa.colecao);
		});

		return tarefas;
	}

	private configurarTarefasBusca(pagina: PaginaBusca): PaginaBusca {
		pagina.tarefas = this.configurarTarefas(pagina.tarefas);

		return pagina;
	}
}
