import { Component, OnInit } from '@angular/core';
import { DateAdapter } from '@angular/material/core';

import { ApontamentoService } from '../../services/apontamento.service';
import { ApontamentosDia } from '../../models/apontamentos-dia';
import { PontoService } from '../../services/ponto.service';
import { BatidasPontoDia } from '../../models/batidas-ponto-dia';
import { ContaService } from 'src/app/core/services/conta.service';
import { BaseComponent } from 'src/app/common/components/base.component';

@Component({
	selector: 'app-apontamentos-por-dia',
	templateUrl: './apontamentos-por-dia.component.html',
	styleUrls: ['./apontamentos-por-dia.component.scss']
})
export class ApontamentosPorDiaComponent extends BaseComponent implements OnInit {

	public carregando: boolean = true;

	public dataAtual: Date = new Date();
	public dataSelecionada: Date = new Date();

	public apontamentos?: ApontamentosDia;
	public batidas?: BatidasPontoDia;

	public dadosPontoNaoEncontrado: boolean = false;

	constructor(servicoConta: ContaService,
		private servicoApontamento: ApontamentoService,
		private servicoPonto: PontoService,
		private dataAdapter: DateAdapter<any>) {

		super(servicoConta);
		this.dataAdapter.setLocale('pt-br');
	}

	public ngOnInit(): void {
		this.obterBatidasEPontosPorDia(this.dataAtual);
	}

	public onDataAlterada(evento: any): void {
		this.dataSelecionada = evento.value;

		this.obterBatidasEPontosPorDia(this.dataSelecionada);
	}

	public obterBatidasEPontosPorDia(data: Date): void {
		this.carregando = true;

		this.servicoApontamento
			.obterApontamentosPorDia(data).subscribe({
				next: (apontamentos) => {
					this.apontamentos = apontamentos;
				},
				complete: () => this.carregando = false
			});

		this.servicoPonto
			.obterBatidasPorDia(data).subscribe({
				next: (batidas) => {
					this.batidas = batidas;
				},
				error: (erro: any) => {
					if (erro.status && erro.status == 404) {
						this.dadosPontoNaoEncontrado = true;

						this.batidas = new BatidasPontoDia();
					}
				}
			});
	}

	public EHoje(data: Date): boolean {
		const hoje = new Date()

		return data.getDate() == hoje.getDate() &&
			data.getMonth() == hoje.getMonth() &&
			data.getFullYear() == hoje.getFullYear()
	}
}
