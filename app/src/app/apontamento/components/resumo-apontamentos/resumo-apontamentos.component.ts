import { Component, Input } from '@angular/core';

@Component({
	selector: 'resumo-apontamentos',
	templateUrl: './resumo-apontamentos.component.html',
	styleUrls: ['./resumo-apontamentos.component.scss']
})
export class ResumoApontamentosComponent {

	@Input()
	public tempoApontadoNaoSincronizado: number = 0;

	@Input()
	public tempoApontadoSincronizado: number = 0;

	@Input()
	public tempoTrabalhado: number = 0;

	@Input()
	public exibirTempoApontado: boolean = true;

	@Input()
	public exibirTempoTrabalhado: boolean = true;

	public get tempoTotalApontado(): number {
		return this.tempoApontadoNaoSincronizado +  this.tempoApontadoSincronizado;
	}

	constructor() { }

}
