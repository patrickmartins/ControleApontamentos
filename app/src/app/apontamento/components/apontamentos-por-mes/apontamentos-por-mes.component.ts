import { Component, NgZone, OnInit } from '@angular/core';
import { DateAdapter } from '@angular/material/core';

import { ApontamentoService } from '../../services/apontamento.service';
import { PontoService } from '../../services/ponto.service';
import { ContaService } from 'src/app/core/services/conta.service';
import { BatidasPontoMes } from '../../models/batidas-ponto-mes';
import { ApontamentosTfsMes } from '../../models/apontamentos-tfs-mes';
import { ApontamentosTfsDia } from '../../models/apontamentos-tfs-dia';
import { MatDatepicker } from '@angular/material/datepicker';
import { BaseComponent } from 'src/app/common/components/base.component';
import { ApontamentosChannelDia } from '../../models/apontamentos-channel-dia';
import { ApontamentosChannelMes } from '../../models/apontamentos-channel-mes';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';

@Component({
	selector: 'app-apontamentos-por-mes',
	templateUrl: './apontamentos-por-mes.component.html',
	styleUrls: ['./apontamentos-por-mes.component.scss']
})
export class ApontamentosPorMesComponent extends BaseComponent implements OnInit {

	public get carregando(): boolean {
		return this.carregandoApontamentosTfs || this.carregandoApontamentosChannel || this.carregandoBatidasPonto;
	}
	
	public get tempoTotalTrabalhadoNoMes() : number {
		return this.batidas ? this.batidas.tempoTotalTrabalhadoNoMes : 0
	}

	public get tempoTotalApontadoNoMes() : number {
		let tempo = 0;

		if(this.apontamentosTfsMes)
			tempo = this.apontamentosTfsMes.tempoTotalApontadoNoMes;

		if(this.apontamentosChannelMes)
			tempo += this.apontamentosChannelMes.tempoTotalApontadoNoMes;

		return tempo;
	}

	public get tempoTotalApontadoSincronizadoNoMes() : number {
		return this.apontamentosTfsMes ? this.apontamentosTfsMes.tempoTotalApontadoSincronizadoChannel : 0;
	}

	public get tempoTotalApontadoNaoSincronizadoNoMes() : number {
		return this.apontamentosTfsMes ? this.apontamentosTfsMes.tempoTotalApontadoNaoSincronizadoChannel : 0;
	}

	public carregandoApontamentosTfs: boolean = true;
	public carregandoApontamentosChannel: boolean = true;
	public carregandoBatidasPonto: boolean = true;

	public dataAtual: Date = new Date();
	public mesSelecionado: Date = new Date();
	public diaSelecionado: Date = new Date();

	public apontamentosTfsDiaSelecionado?: ApontamentosTfsDia;
	public apontamentosChannelDiaSelecionado?: ApontamentosChannelDia;

	public apontamentosTfsMes?: ApontamentosTfsMes;
	public apontamentosChannelMes?: ApontamentosChannelMes;
	public batidas?: BatidasPontoMes;

	constructor(servicoConta: ContaService,
		private servicoApontamento: ApontamentoService,
		private servicoPonto: PontoService,
		private dataAdapter: DateAdapter<any>,
		private zone: NgZone,
		private activeRoute: ActivatedRoute, 
		private router: Router) {

		super(servicoConta);

		this.dataAdapter.setLocale('pt-br');
	}

	public ngOnInit(): void {
		this.activeRoute
			.queryParamMap
			.subscribe((mapParams: any) => {
				let data = moment(`01-${mapParams.params.mes}-${mapParams.params.ano}`, 'DD-MM-YYYY');

				this.mesSelecionado = data.isValid() ? data.toDate() : this.mesSelecionado;

				this.obterBatidasEApontamentosPorMes(this.mesSelecionado);
			});		
	}

	public onDataAlterada(data: Date, picker: MatDatepicker<any>): void {
		picker.close();

		this.mesSelecionado = data;

		this.router
			.navigate([], 
			{ queryParams: 
				{ 
					mes: data.getMonth() + 1, 
					ano: data.getFullYear() 
				}
			});
	}

	public onDiaClicado(dia: number) {
		this.zone.run(() => {
			this.diaSelecionado = new Date(this.mesSelecionado.getFullYear(), this.mesSelecionado.getMonth(), dia);

			this.apontamentosTfsDiaSelecionado = this.apontamentosTfsMes?.obterApontamentosPorDia(dia);
			this.apontamentosChannelDiaSelecionado = this.apontamentosChannelMes?.obterApontamentosPorDia(dia);
		});
	}

	public obterBatidasEApontamentosPorMes(data: Date): void {
		this.carregandoApontamentosTfs = true;
		this.carregandoApontamentosChannel = true;
		this.carregandoBatidasPonto = true;
		
		let mes = data.getMonth() + 1;
		let ano = data.getFullYear();

		if(this.usuarioLogado?.possuiContaTfs) {
			this.servicoApontamento
				.obterApontamentosTfsPorMes(mes, ano).subscribe({
					next: (apontamentos) => {
						this.apontamentosTfsMes = apontamentos;

						this.selecionarUltimosApontamentos();
					},
					complete: () => this.carregandoApontamentosTfs = false
				});
		}
		else {
			this.carregandoApontamentosTfs = false;
		}

		if(this.usuarioLogado?.possuiContaChannel) {
			this.servicoApontamento
				.obterApontamentosChannelPorMes(mes, ano).subscribe({
					next: (apontamentos) => {
						this.apontamentosChannelMes = apontamentos;

						this.selecionarUltimosApontamentos();
					},
					complete: () => this.carregandoApontamentosChannel = false
				});
		}
		else {
			this.carregandoApontamentosChannel = false;
		}

		if(this.usuarioLogado?.possuiContaPonto) {
			this.servicoPonto
				.obterBatidasPorMes(mes, ano).subscribe({
					next: (batidas) => {
						this.batidas = batidas;
					},
					complete: () => this.carregandoBatidasPonto = false
				});
		}
		else {
			this.carregandoBatidasPonto = false;
		}
	}

	public eHoje(data: Date): boolean {
		const hoje = new Date()

		return data.getDate() == hoje.getDate() &&
			data.getMonth() == hoje.getMonth() &&
			data.getFullYear() == hoje.getFullYear()
	}

	private selecionarUltimosApontamentos(): void {
		let dataUltimoApontamentoTfs: Date | undefined;
		let dataUltimoApontamentoChannel: Date | undefined;
		
		if (this.apontamentosTfsMes) {
			let apontamentosTfsDiaSelecionado = this.apontamentosTfsMes.obterApontamentosDoUltimoDia();
			
			dataUltimoApontamentoTfs = apontamentosTfsDiaSelecionado ? apontamentosTfsDiaSelecionado.dataReferencia : undefined;
		}

		if (this.apontamentosChannelMes) {
			let apontamentosChannelDiaSelecionado = this.apontamentosChannelMes.obterApontamentosDoUltimoDia();

			dataUltimoApontamentoChannel = apontamentosChannelDiaSelecionado ? apontamentosChannelDiaSelecionado.dataReferencia : undefined;
		}

		if(dataUltimoApontamentoTfs && dataUltimoApontamentoChannel)
			this.diaSelecionado = dataUltimoApontamentoChannel > dataUltimoApontamentoTfs ? dataUltimoApontamentoChannel : dataUltimoApontamentoTfs;

		if(!dataUltimoApontamentoTfs)
			this.diaSelecionado = dataUltimoApontamentoChannel!;

		if(!dataUltimoApontamentoChannel)
			this.diaSelecionado = dataUltimoApontamentoTfs!;	
		
		if(!dataUltimoApontamentoTfs && !dataUltimoApontamentoChannel)
			this.diaSelecionado = new Date();
		
		if(this.diaSelecionado)
			this.onDiaClicado(this.diaSelecionado.getDate());
	}
}
