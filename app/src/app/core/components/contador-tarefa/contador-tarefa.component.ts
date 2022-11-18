import { AfterViewInit, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CdTimerComponent } from 'angular-cd-timer';
import * as moment from 'moment';

import { LocalStorageHelper } from 'src/app/helpers/local-storage.helper';
import { ContadorSalvo } from 'src/app/core/models/contador-salvo';
import { NovoApontamento } from '../../models/novo-apontamento';
import { StatusContador } from '../../models/status-contador';
import { ModalSalvarApontamentoComponent } from '../modal-salvar-apontamento/modal-salvar-apontamento.component';

@Component({
	selector: 'contador-tarefa',
	templateUrl: './contador-tarefa.component.html',
	styleUrls: ['./contador-tarefa.component.scss']
})
export class ContadorTarefaComponent implements AfterViewInit {

	@Input()
	public idTarefa: number = 0;

	@Input()
	public colecao: string = "";

	@Input()
	public habilitado: boolean = true;

	@Output()
	public onApontamentoSalvo = new EventEmitter<NovoApontamento>

	@ViewChild('timer')
	public timer!: CdTimerComponent;

	public status: StatusContador = StatusContador.naoiniciado;

	constructor(private dialog: MatDialog) { }

	public ngAfterViewInit(): void {
		this.carregarContadorSalvo();
	}

	public iniciarContagem(): void {
		if (this.status == StatusContador.contando)
			return;

		this.status == StatusContador.parado || this.status == StatusContador.naoiniciado ? this.timer.start() : this.timer.resume();

		this.alterarStatus(StatusContador.contando);
	}

	public pausarContagem(): void {
		this.timer.stop();

		this.alterarStatus(StatusContador.pausado);
	}

	public pararContagem(): void {
		this.timer.stop()

		this.alterarStatus(StatusContador.parado);
	}

	public salvarApontamento(): void {
		let tempoDecorrido = this.timer.get();

		let dialogRef = this.dialog.open(ModalSalvarApontamentoComponent, {
			width: '500px',
			height: '430px',
			disableClose: true,
			data: new NovoApontamento().criarNovo({
				idTarefa: this.idTarefa,
				colecao: this.colecao,
				tempoTotal: tempoDecorrido.tick_count > 0 ? tempoDecorrido.tick_count - 1 : 0
			})
		});

		dialogRef.afterClosed().subscribe((result) => {
			if(result)
				this.onApontamentoSalvo.emit(result);
		});
	}

	private alterarStatus(status: StatusContador): void {
		this.status = status;

		this.salvarContador();
	}

	public resetarContador(): void {
		const chave = `contador-${this.colecao}-${this.idTarefa}`;

		if (LocalStorageHelper.dadoExiste(chave)) {
			LocalStorageHelper.removerDados(chave);			
		}

		this.timer.startTime = 0;
		this.timer.reset();		

		this.status = StatusContador.naoiniciado;
	}


	private salvarContador(): void {
		const chave = `contador-${this.colecao}-${this.idTarefa}`;
		const tempoDecorrido = this.timer.get();

		let contador = new ContadorSalvo();

		contador.statusContador = this.status;
		contador.tempoDecorrido = tempoDecorrido.tick_count > 0 ? tempoDecorrido.tick_count - 1 : 0;
		contador.dataStatus = Date.now();		

		LocalStorageHelper.salvarDados(chave, contador);
	}

	private carregarContadorSalvo(): void {
		const chave = `contador-${this.colecao}-${this.idTarefa}`;

		if (LocalStorageHelper.dadoExiste(chave)) {

			const contador = LocalStorageHelper.obterDados<ContadorSalvo>(chave, ContadorSalvo)!;

			if (contador.statusContador == StatusContador.contando) {
				this.timer.startTime = contador.tempoDecorrido + moment(Date.now()).diff(new Date(contador.dataStatus), 'seconds');		
				
				this.timer.start();
			} else {
				this.timer.startTime = contador.tempoDecorrido;

				this.timer.start();
				this.timer.stop();
			}			

			this.status = contador.statusContador;
		}
	}

}