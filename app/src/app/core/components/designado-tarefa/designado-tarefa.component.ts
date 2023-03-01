import { Component, Input } from '@angular/core';
import { Designado } from '../../models/designado';

@Component({
	selector: 'designado-tarefa',
	templateUrl: './designado-tarefa.component.html',
	styleUrls: ['./designado-tarefa.component.scss']
})
export class DesignadoTarefaComponent {

	@Input()
	public designado: Designado | undefined;
	
	@Input()
	public tempoTotalApontadoSincronizadoChannel: number = 0;

	@Input()
	public tempoTotalApontadoNaoSincronizadoChannel: number = 0;

	constructor() { }

}
