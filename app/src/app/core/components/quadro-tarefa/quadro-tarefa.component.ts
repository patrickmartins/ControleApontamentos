import { Component, Input, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BaseComponent } from 'src/app/common/components/base.component';

import { ContaService } from 'src/app/core/services/conta.service';
import { TarefaHelper } from 'src/app/helpers/tarefa.helper';
import { ApontamentoTfs } from '../../models/apontamento-tfs';
import { NovoApontamento } from '../../models/novo-apontamento';
import { Tarefa } from '../../models/tarefa';
import { ContadorTarefaComponent } from '../contador-tarefa/contador-tarefa.component';
import { TfsService } from '../../services/tfs.service';

@Component({
	selector: 'quadro-tarefa',
	templateUrl: './quadro-tarefa.component.html',
	styleUrls: ['./quadro-tarefa.component.scss']
})

export class QuadroTarefaComponent extends BaseComponent {

	@Input()
	public tarefa!: Tarefa;

	@Input()
	public permiteApontar: boolean = true;

	@Input()
	public permiteFixar: boolean = true;

	@ViewChild('contador')
	public contador!: ContadorTarefaComponent

	public apontamentosExpandido: boolean = false;
	public salvandoApontamento: boolean = false;	
	
	constructor(servicoConta: ContaService, snackBar: MatSnackBar, private servicoTfs: TfsService) {
		super(servicoConta, snackBar);
	}

	public fixarTarefa(): void {
		if (this.permiteFixar) {
			this.tarefa.fixada = true;

			TarefaHelper.fixarTarefa(this.usuarioLogado!.nomeUsuario, this.tarefa);
		}
	}

	public desafixarTarefa(): void {
		if (this.permiteFixar) {
			this.tarefa.fixada = false;

			TarefaHelper.desafixarTarefa(this.usuarioLogado!.nomeUsuario, this.tarefa);
		}
	}

	public onApontamentoSalvo(novoApontamento: NovoApontamento): void {
		this.salvandoApontamento = true;

		this.servicoTfs
			.salvarApontamento(novoApontamento).subscribe({
				next: () => {
					let usuario = this.usuarioLogado?.nomeUsuario.split('@')[0];

					const apontamento = new ApontamentoTfs().criarNovo(
					{
						usuario: usuario,
						comentario: novoApontamento.comentario,
						tempo: novoApontamento.tempoTotal,
						sincronizadoChannel: false,
						data: novoApontamento.data
					});

					this.tarefa.adicionarApontamento(apontamento!);

					this.tarefa.recalcularTempoTotalApontadoNaoSincronizadoChannel(usuario!);
					this.tarefa.recalcularTempoTotalApontadoSincronizadoChannel(usuario!);

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