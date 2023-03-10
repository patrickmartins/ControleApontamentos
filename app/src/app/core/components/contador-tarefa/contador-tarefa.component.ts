import { AfterViewInit, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import * as moment from 'moment';

import { LocalStorageHelper } from 'src/app/helpers/local-storage.helper';
import { ContadorSalvo } from 'src/app/core/models/contador-salvo';
import { NovoApontamento } from '../../models/novo-apontamento';
import { StatusContador } from '../../models/status-contador';
import { ModalSalvarApontamentoComponent } from '../modal-salvar-apontamento/modal-salvar-apontamento.component';
import { fromEvent, Observable, Subscription } from 'rxjs';
import { TimerComponent } from 'src/libs/timer/timer.component';

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
	public timer!: TimerComponent;

	public status: StatusContador = StatusContador.naoiniciado;
	public focusObservable?: Observable<Event>;
	public focusSubscription?: Subscription;

	private get _id(): string {
		 return `contador-${this.colecao}-${this.idTarefa}`;
	}

	constructor(private dialog: MatDialog) 
	{
		this.focusObservable = fromEvent(window, "focus");  
		this.focusSubscription = this.focusObservable.subscribe((evento: any) => this.recarregarContadorSalvo());
	}

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

		this.salvarApontamento();
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
				data: new Date(),
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
		if (LocalStorageHelper.dadoExiste(this._id)) {
			LocalStorageHelper.removerDados(this._id);			
		}

		this.timer.startTime = 0;
		this.timer.reset();		

		this.status = StatusContador.naoiniciado;
	}


	private salvarContador(): void {
		const tempoDecorrido = this.timer.get();

		let contador = new ContadorSalvo();

		contador.statusContador = this.status;
		contador.tempoDecorrido = tempoDecorrido.tick_count > 0 ? tempoDecorrido.tick_count - 1 : 0;
		contador.dataStatus = Date.now();		

		LocalStorageHelper.salvarDados(this._id, contador);
	}

	private recarregarContadorSalvo(): void {
		if(this.status != StatusContador.contando)
			return;

		if (LocalStorageHelper.dadoExiste(this._id)) {

			const contador = LocalStorageHelper.obterDados<ContadorSalvo>(this._id, ContadorSalvo)!;

			this.timer.setTickCounter(contador.tempoDecorrido + moment(Date.now()).diff(new Date(contador.dataStatus), 'seconds'));

			console.log(contador.dataStatus);
		}
	}

	private carregarContadorSalvo(): void {
		if (LocalStorageHelper.dadoExiste(this._id)) {

			const contador = LocalStorageHelper.obterDados<ContadorSalvo>(this._id, ContadorSalvo)!;

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