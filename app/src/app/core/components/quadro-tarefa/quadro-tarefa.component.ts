import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BaseComponent } from 'src/app/common/components/base.component';

import { ContaService } from 'src/app/core/services/conta.service';
import { TarefaHelper } from 'src/app/helpers/tarefa.helper';
import { ApontamentoTfs } from '../../models/apontamento-tfs';
import { NovoApontamento } from '../../models/novo-apontamento';
import { Tarefa } from '../../models/tarefa';
import { ContadorTarefaComponent } from '../contador-tarefa/contador-tarefa.component';
import { TfsService } from '../../services/tfs.service';
import { Erro } from 'src/app/common/models/erro';
import { MatDialog } from '@angular/material/dialog';
import { ModalSalvarApontamentoComponent } from '../modal-salvar-apontamento/modal-salvar-apontamento.component';

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

    @Output()
	public onApontamentoSalvo = new EventEmitter<ApontamentoTfs>();
    
	@ViewChild('contador')
	public contador!: ContadorTarefaComponent

	public apontamentosExpandido: boolean = false;
	public salvandoApontamento: boolean = false;	
	
	constructor(servicoConta: ContaService, snackBar: MatSnackBar, private servicoTfs: TfsService, private dialog: MatDialog) {
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

    public novoApontamento(): void {
		let dialogRef = this.dialog.open(ModalSalvarApontamentoComponent, {
			width: '500px',
			height: '430px',
			disableClose: true,
			data: new NovoApontamento().criarNovo({
				idTarefa: this.tarefa.id,
				colecao: this.tarefa.colecao,
				data: new Date(),
				tempoTotal: 0
			})
		});

		dialogRef.afterClosed().subscribe((result) => {
			if(result)
				this.onSalvarApontamento(result);
		});
	}

	public onSalvarApontamento(novoApontamento: NovoApontamento): void {
		this.salvandoApontamento = true;

		this.servicoTfs
			.salvarApontamento(novoApontamento).subscribe({
				next: (apontamento) => {
					let usuario = this.usuarioLogado?.nomeUsuario.split('@')[0];

					this.tarefa.adicionarApontamento(apontamento!);

					this.tarefa.recalcularTempoTotalApontadoNaoSincronizadoChannel(usuario!);
					this.tarefa.recalcularTempoTotalApontadoSincronizadoChannel(usuario!);

					this.contador.resetarContador();

					this.snackBar.open("Apontamento salvo com sucesso!", "OK", {
						duration: 5000,
						verticalPosition: "bottom",
						horizontalPosition: "center",
						panelClass: "sucesso"
					});

                    this.onApontamentoSalvo.emit(apontamento);
				},
				error: (erros: Erro[]) => {
                    this.salvandoApontamento = false
                    
                    for(let erro of erros) {
                        this.snackBar.open(erro.descricao, "OK", {
                            duration: 5000,
                            verticalPosition: "top",
                            horizontalPosition: "center",
                            panelClass: "erro"
                        });
                    }
				},
                complete: () => this.salvandoApontamento = false
			});
	}
}