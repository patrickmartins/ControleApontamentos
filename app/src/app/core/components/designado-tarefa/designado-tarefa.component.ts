import { Component, Input } from '@angular/core';

@Component({
	selector: 'designado-tarefa',
	templateUrl: './designado-tarefa.component.html',
	styleUrls: ['./designado-tarefa.component.scss']
})
export class DesignadoTarefaComponent {

	@Input()
	public designado: string = "";
	
	@Input()
	public tempoTotalApontadoSincronizadoChannel: number = 0;

	@Input()
	public tempoTotalApontadoNaoSincronizadoChannel: number = 0;

	constructor() { }

}
