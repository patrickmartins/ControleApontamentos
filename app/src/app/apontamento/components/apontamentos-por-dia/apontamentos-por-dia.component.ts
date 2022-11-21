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

		if(this.usuarioLogado?.possuiContaTfs) {
			this.servicoApontamento
				.obterApontamentosPorDia(data).subscribe({
					next: (apontamentos) => {
						this.apontamentos = apontamentos;
					},
					complete: () => this.carregando = false
				});
		}
		else {
			this.carregando = this.usuarioLogado?.possuiContaPonto == true;
		}

		if(this.usuarioLogado?.possuiContaPonto) {
			this.servicoPonto
				.obterBatidasPorDia(data).subscribe({
					next: (batidas) => {
						this.batidas = batidas;
					},
					complete: () => this.carregando = this.usuarioLogado?.possuiContaTfs == true
				});
		}
		else {
			this.carregando = this.usuarioLogado?.possuiContaTfs == true;
		}		
	}

	public EHoje(data: Date): boolean {
		const hoje = new Date()

		return data.getDate() == hoje.getDate() &&
			data.getMonth() == hoje.getMonth() &&
			data.getFullYear() == hoje.getFullYear()
	}
}
