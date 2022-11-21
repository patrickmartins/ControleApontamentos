import { Component, NgZone, OnInit } from '@angular/core';
import { DateAdapter } from '@angular/material/core';

import { ApontamentoService } from '../../services/apontamento.service';
import { PontoService } from '../../services/ponto.service';
import { ContaService } from 'src/app/core/services/conta.service';
import { BatidasPontoMes } from '../../models/batidas-ponto-mes';
import { ApontamentosMes } from '../../models/apontamentos-mes';
import { ApontamentosDia } from '../../models/apontamentos-dia';
import { MatDatepicker } from '@angular/material/datepicker';
import { BaseComponent } from 'src/app/common/components/base.component';

@Component({
	selector: 'app-apontamentos-por-mes',
	templateUrl: './apontamentos-por-mes.component.html',
	styleUrls: ['./apontamentos-por-mes.component.scss']
})
export class ApontamentosPorMesComponent extends BaseComponent implements OnInit {

	public carregando: boolean = true;

	public dataAtual: Date = new Date();
	public mesSelecionado: Date = new Date();

	public apontamentosDiaSelecionado?: ApontamentosDia;
	public apontamentos?: ApontamentosMes;
	public batidas?: BatidasPontoMes;

	constructor(servicoConta: ContaService,
		private servicoApontamento: ApontamentoService,
		private servicoPonto: PontoService,
		private dataAdapter: DateAdapter<any>,
		private zone: NgZone) {

		super(servicoConta);

		this.dataAdapter.setLocale('pt-br');
	}

	public ngOnInit(): void {
		this.obterBatidasEPontosPorMes(this.dataAtual.getMonth() + 1, this.dataAtual.getFullYear());
	}

	public onDataAlterada(data: Date, picker: MatDatepicker<any>): void {
		this.mesSelecionado = data;

		picker.close();

		this.obterBatidasEPontosPorMes(this.mesSelecionado.getMonth() + 1, this.mesSelecionado.getFullYear());
	}

	public onDiaClicado(dia: number) {
		this.zone.run(() => {
			this.apontamentosDiaSelecionado = this.apontamentos?.obterApontamentosPorDia(dia);
		});
	}

	public obterBatidasEPontosPorMes(mes: number, ano: number): void {
		this.carregando = true;
		
		if(this.usuarioLogado?.possuiContaTfs) {
			this.servicoApontamento
				.obterApontamentosPorMes(mes, ano).subscribe({
					next: (apontamentos) => {
						this.apontamentos = apontamentos;

						if (this.apontamentos) {
							let diasComApontamento = this.apontamentos.apontamentosDiarios.filter(c => c.tempoTotalApontadoNoDia > 0);

							if (diasComApontamento.length > 0) {
								this.apontamentosDiaSelecionado = diasComApontamento[diasComApontamento.length - 1];
							}
							else if (this.apontamentos.apontamentosDiarios.length > 0) {
								this.apontamentosDiaSelecionado = this.apontamentos.apontamentosDiarios[this.apontamentos.apontamentosDiarios.length - 1];
							}
						}
					},
					complete: () => this.carregando = false
				});
		}
		else {
			this.carregando = this.usuarioLogado?.possuiContaPonto == true;
		}

		if(this.usuarioLogado?.possuiContaPonto) {
			this.servicoPonto
				.obterBatidasPorMes(mes, ano).subscribe({
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
