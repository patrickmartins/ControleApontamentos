import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { BaseService } from 'src/app/common/services/base.service';
import { Tarefa } from 'src/app/core/models/tarefa';
import { environment } from 'src/environments/environment';
import { GrupoTarefas } from '../../tarefa/models/grupos-tarefas';
import { NovoApontamento } from '../models/novo-apontamento';
import { TarefaHelper } from 'src/app/helpers/tarefa.helper';
import { FiltroBusca } from '../models/filtro-busca';
import { PaginaBusca } from '../models/pagina-busca';

@Injectable({
	providedIn: 'root'
})

export class TarefaService extends BaseService {

	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	public obterTarefasAtivas(): Observable<GrupoTarefas[]> {
		return this.get<GrupoTarefas[]>(`${environment.urlApiBase}tarefa/ativas`, GrupoTarefas);
	}

	public obterTarefasPorIds(colecao: string, ids: number[]): Observable<Tarefa[]> {
		return this.get<Tarefa[]>(`${environment.urlApiBase}tarefa/por-ids`, Tarefa, {}, { colecao: colecao, ids: ids }).pipe(
			map(tarefas => this.configurarTarefas(tarefas))
		);
	}

	public buscarTarefas(filtro: FiltroBusca): Observable<PaginaBusca> {
		return this.get<PaginaBusca>(`${environment.urlApiBase}tarefa/buscar`, PaginaBusca, {}, filtro).pipe(
			map(pagina => this.configurarTarefasBusca(pagina)));
	}

	public salvarApontamento(apontamento: NovoApontamento): Observable<any> {
		return this.post<any>(`${environment.urlApiBase}tarefa/adicionar-apontamento`, apontamento);
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
