import { Component, OnInit } from '@angular/core';
import { forkJoin, of } from 'rxjs';
import { Tarefa } from 'src/app/core/models/tarefa';
import { GrupoTarefas } from '../../models/grupos-tarefas';
import { TarefaService } from '../../../core/services/tarefa.service';
import { TarefaHelper } from 'src/app/helpers/tarefa.helper';
import { BaseComponent } from 'src/app/common/components/base.component';
import { ContaService } from 'src/app/core/services/conta.service';

@Component({
	selector: 'app-minhas-terefas',
	templateUrl: './minhas-terefas.component.html',
	styleUrls: ['./minhas-terefas.component.scss']
})

export class MinhasTerefasComponent extends BaseComponent implements OnInit {

	public carregando: boolean = true;
	
	public grupos: GrupoTarefas[] = [];
	public tarefasFixadas: Tarefa[] = [];
	
	public get possuiTarefas(): boolean {
		return this.grupos.some(c => c.tarefas.length > 0);
	}

	constructor(servicoConta: ContaService, private servicoTarefa: TarefaService) { 
		super(servicoConta);
	}

	public ngOnInit(): void {
		this.obterTarefasAtivas();
	}

	private obterTarefasAtivas(): void {
		if(this.usuarioLogado?.possuiContaTfs) {
			let tarefasFixadas = TarefaHelper.obterTarefasFixadas(this.usuarioLogado!.nomeUsuario);
			let tarefasAgrupadas = TarefaHelper.agruparTarefasFixadas(tarefasFixadas);
			
			forkJoin({
				tarefasFixadas: tarefasAgrupadas.length == 0 ? of([]) : forkJoin(tarefasAgrupadas.map((tarefasPorColecao: any) => this.servicoTarefa.obterTarefasPorIds(tarefasPorColecao.key, tarefasPorColecao.values.map((c: any) => c.idTarefa)))),
				grupoTarefasAtivas: this.servicoTarefa.obterTarefasAtivas()
			})		
			.subscribe({ 
				next: (resultado: any) => {
					this.tarefasFixadas = resultado.tarefasFixadas.flatMap((c: any) => c);
					this.grupos = resultado.grupoTarefasAtivas;
				},
				complete: () => this.carregando = false
			});
		}
		else {
			this.carregando = false
		}
	}
}
