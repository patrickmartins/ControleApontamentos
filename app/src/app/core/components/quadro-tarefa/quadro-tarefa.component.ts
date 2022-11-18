import { Component, Input, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BaseComponent } from 'src/app/common/components/base.component';
import { Usuario } from 'src/app/conta/models/usuario';

import { ContaService } from 'src/app/core/services/conta.service';
import { TarefaHelper } from 'src/app/helpers/tarefa.helper';
import { Apontamento } from '../../models/apontamento';
import { NovoApontamento } from '../../models/novo-apontamento';
import { Tarefa } from '../../models/tarefa';
import { TarefaService } from '../../services/tarefa.service';
import { ContadorTarefaComponent } from '../contador-tarefa/contador-tarefa.component';

@Component({
	selector: 'quadro-tarefa',
	templateUrl: './quadro-tarefa.component.html',
	styleUrls: ['./quadro-tarefa.component.scss']
})

export class QuadroTarefaComponent extends BaseComponent {

	@Input()
	public tarefa!: Tarefa;

	@Input()
	public habilitarApontamento: boolean = true;

	@Input()
	public permiteFixar: boolean = true;

	@ViewChild('contador')
	public contador!: ContadorTarefaComponent

	public apontamentosExpandido: boolean = false;
	public salvandoApontamento: boolean = false;	
	
	constructor(servicoConta: ContaService, private tarefaService: TarefaService, private snackBar: MatSnackBar) {
		super(servicoConta);
	}

	public fixarTarefa(): void {
		if (this.permiteFixar) {
			this.tarefa.fixada = true;

			TarefaHelper.fixarTarefa(this.tarefa);
		}
	}

	public desafixarTarefa(): void {
		if (this.permiteFixar) {
			this.tarefa.fixada = false;

			TarefaHelper.desafixarTarefa(this.tarefa);
		}
	}

	public onApontamentoSalvo(novoApontamento: NovoApontamento): void {
		this.salvandoApontamento = true;

		this.tarefaService
			.salvarApontamento(novoApontamento).subscribe({
				next: () => {

					const apontamento = new Apontamento().criarNovo(
						{
							usuario: this.usuarioLogado?.nomeUsuario.split('@')[0],
							comentario: novoApontamento.comentario,
							tempo: novoApontamento.tempoTotal,
							sincronizadoChannel: false,
							data: new Date()
						});

					this.tarefa.adicionarApontamento(apontamento!);

					this.salvandoApontamento = false;

					this.contador.resetarContador();

					this.snackBar.open("Apontamento salvo com sucesso!", "OK", {
						duration: 5000,
						verticalPosition: "bottom",
						horizontalPosition: "center",
						panelClass: "sucesso"
					});
				},
				error: () => {
					this.salvandoApontamento = false;

					this.snackBar.open("Ocorreu um erro interno. Atualize a página e tente novamente.", "OK", {
						duration: 5000,
						verticalPosition: "top",
						horizontalPosition: "center",
						panelClass: "erro"
					});
				}
			});
	}
}