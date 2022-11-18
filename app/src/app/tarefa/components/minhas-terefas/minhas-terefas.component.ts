import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import { Tarefa } from 'src/app/core/models/tarefa';
import { GrupoTarefas } from '../../models/grupos-tarefas';
import { TarefaService } from '../../../core/services/tarefa.service';
import { TarefaHelper } from 'src/app/helpers/tarefa.helper';

@Component({
	selector: 'app-minhas-terefas',
	templateUrl: './minhas-terefas.component.html',
	styleUrls: ['./minhas-terefas.component.scss']
})

export class MinhasTerefasComponent implements OnInit {

	public carregando: boolean = true;

	public grupos?: GrupoTarefas[];
	public tarefasFixadas?: Tarefa[];
	
	constructor(private servicoTarefa: TarefaService) { }

	public ngOnInit(): void {
		this.obterTarefasAtivas();
	}

	private obterTarefasAtivas(): void {

		let tarefasFixadas = TarefaHelper.obterTarefasFixadas();
		let tarefasAgrupadas = TarefaHelper.agruparTarefasFixadas(tarefasFixadas);

		forkJoin({
			tarefasFixadas: forkJoin(tarefasAgrupadas.map((tarefasPorColecao: any) => this.servicoTarefa.obterTarefasPorIds(tarefasPorColecao.key, tarefasPorColecao.values.map((c: any) => c.idTarefa)))),
			tarefasAtivas: this.servicoTarefa.obterTarefasAtivas()
		})		
		.subscribe({ 
			next: (resultado: any) => {
				this.tarefasFixadas = resultado.tarefasFixadas.flatMap((c: any) => c);
				this.grupos = resultado.tarefasAtivas;
			},
			complete: () => this.carregando = false
		});
	}
}
