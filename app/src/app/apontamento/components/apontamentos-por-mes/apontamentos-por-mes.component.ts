import { Component, NgZone, OnInit } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { forkJoin, of, tap } from 'rxjs';

import { BaseComponent } from 'src/app/common/components/base.component';
import { JobService } from 'src/app/core/services/job.service';
import { JobInfo } from 'src/app/core/models/job-info';
import { ApontamentosChannelDia } from '../../models/apontamentos-channel-dia';
import { ApontamentosChannelMes } from '../../models/apontamentos-channel-mes';
import { BatidasPontoMes } from '../../models/batidas-ponto-mes';
import { ApontamentosTfsMes } from '../../models/apontamentos-tfs-mes';
import { ApontamentosTfsDia } from '../../models/apontamentos-tfs-dia';
import { ApontamentoService } from '../../services/apontamento.service';
import { PontoService } from '../../services/ponto.service';
import { ContaService } from 'src/app/core/services/conta.service';

@Component({
	selector: 'app-apontamentos-por-mes',
	templateUrl: './apontamentos-por-mes.component.html',
	styleUrls: ['./apontamentos-por-mes.component.scss']
})
export class ApontamentosPorMesComponent extends BaseComponent implements OnInit {

	public carregando: boolean = true;
	
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
		return (this.apontamentosTfsMes ? this.apontamentosTfsMes.tempoTotalApontadoSincronizadoChannel : 0) + 
				(this.apontamentosChannelMes ? this.apontamentosChannelMes.tempoTotalApontadoNoMes : 0);
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
	
	public infoJobCarga?: JobInfo;

	constructor(servicoConta: ContaService,
		private servicoApontamento: ApontamentoService,
		private servicoPonto: PontoService,
		private servicoJob: JobService,
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

				this.servicoJob.obterJobCarga().subscribe({
					next: (jobInfo) => {
						this.infoJobCarga = jobInfo;
					}
				});

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
		this.apontamentosChannelMes = undefined;
		this.apontamentosTfsMes = undefined;

        this.carregando = true;
		
		let mes = data.getMonth() + 1;
		let ano = data.getFullYear();
        this.apontamentosChannelMes = undefined;
        this.apontamentosTfsMes = undefined;       

        forkJoin({
            apontamentosTfsMes: !this.usuarioLogado?.possuiContaTfs ? of(undefined) : this.servicoApontamento.obterApontamentosTfsPorMes(mes, ano),
            apontamentosChannelMes: !this.usuarioLogado?.possuiContaChannel ? of(undefined) : this.servicoApontamento.obterApontamentosChannelPorMes(mes, ano),
            batidas: !this.usuarioLogado?.possuiContaPonto ? of(undefined) : this.servicoPonto.obterBatidasPorMes(mes, ano),
            infoJobCarga: this.servicoJob.obterJobCarga()
        })		
        .subscribe({ 
            next: (resultado: any) => {
                this.apontamentosTfsMes = resultado.apontamentosTfsMes;
                this.apontamentosChannelMes = resultado.apontamentosChannelMes;
                this.batidas = resultado.batidas;
                this.infoJobCarga = resultado.jobInfo;

                this.servicoApontamento.consolidarTarefasEAtividadesMes(this.apontamentosTfsMes, this.apontamentosChannelMes);

                this.selecionarUltimosApontamentos();
            },
            complete: () => this.carregando = false
        });
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
