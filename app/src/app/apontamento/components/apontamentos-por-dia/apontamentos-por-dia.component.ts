import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DateAdapter } from '@angular/material/core';
import { forkJoin, of } from 'rxjs';
import * as moment from 'moment';

import { ApontamentoService } from '../../services/apontamento.service';
import { ApontamentosTfsDia } from '../../models/apontamentos-tfs-dia';
import { PontoService } from '../../services/ponto.service';
import { BatidasPontoDia } from '../../models/batidas-ponto-dia';
import { ContaService } from 'src/app/core/services/conta.service';
import { BaseComponent } from 'src/app/common/components/base.component';
import { ApontamentosChannelDia } from '../../models/apontamentos-channel-dia';
import { JobInfo } from 'src/app/core/models/job-info';
import { JobService } from 'src/app/core/services/job.service';

@Component({
	selector: 'app-apontamentos-por-dia',
	templateUrl: './apontamentos-por-dia.component.html',
	styleUrls: ['./apontamentos-por-dia.component.scss']
})
export class ApontamentosPorDiaComponent extends BaseComponent implements OnInit {

	public carregando: boolean = true;

	public get tempoTotalTrabalhadoNoDia() : number {
		return this.batidas ? this.batidas.tempoTotalTrabalhadoNoDia : 0
	}

	public get tempoTotalApontadoNoDia() : number {
		let tempo = 0;

		if(this.apontamentosTfsDia)
			tempo = this.apontamentosTfsDia.tempoTotalApontadoNoDia;

		if(this.apontamentosChannelDia)
			tempo += this.apontamentosChannelDia.tempoTotalApontadoNoDia;

		return tempo;
	}

	public get tempoTotalApontadoSincronizadoNoDia() : number {
		return this.apontamentosTfsDia ? this.apontamentosTfsDia.tempoTotalApontadoSincronizadoChannel : 0;
	}

	public get tempoTotalApontadoNaoSincronizadoNoDia() : number {
		return this.apontamentosTfsDia ? this.apontamentosTfsDia.tempoTotalApontadoNaoSincronizadoChannel : 0;
	}

	public dataAtual: Date = new Date();
	public dataSelecionada: Date = new Date();

	public apontamentosTfsDia?: ApontamentosTfsDia;
	public apontamentosChannelDia?: ApontamentosChannelDia;
	public batidas?: BatidasPontoDia;

	public infoJobCarga?: JobInfo;
	
	constructor(servicoConta: ContaService,
		private servicoApontamento: ApontamentoService,
		private servicoPonto: PontoService,
		private servicoJob: JobService,
		private dataAdapter: DateAdapter<any>,
		private activeRoute: ActivatedRoute, 
		private router: Router) {

		super(servicoConta);
		this.dataAdapter.setLocale('pt-br');
	}
	
	public ngOnInit(): void {
		this.activeRoute
		.queryParamMap
		.subscribe((mapParams: any) => {			
			let data = moment(mapParams.params.data, 'DD-MM-YYYY');

			this.dataSelecionada = data.isValid() ? data.toDate() : this.dataSelecionada;

			this.servicoJob.obterJobCarga().subscribe({
				next: (jobInfo) => {
					this.infoJobCarga = jobInfo;
				}
			});

			this.obterBatidasEApontamentosPorDia(this.dataSelecionada);
		});		
	}

	public onDataAlterada(data: Date): void {
		this.dataSelecionada = data;

		this.router.navigate([], { queryParams: { data: moment(this.dataSelecionada).format('DD-MM-YYYY') }});
	}

	public obterBatidasEApontamentosPorDia(data: Date): void {
		this.apontamentosChannelDia = undefined;
		this.apontamentosTfsDia = undefined;

		this.carregando = true;

        forkJoin({
            apontamentosTfsDia: !this.usuarioLogado?.possuiContaTfs ? of(undefined) : this.servicoApontamento.obterApontamentosTfsPorDia(data),
            apontamentosChannelDia: !this.usuarioLogado?.possuiContaChannel ? of(undefined) : this.servicoApontamento.obterApontamentosChannelPorDia(data),
            batidas: !this.usuarioLogado?.possuiContaPonto ? of(undefined) : this.servicoPonto.obterBatidasPorDia(data),
            infoJobCarga: this.servicoJob.obterJobCarga()
        })		
        .subscribe({ 
            next: (resultado: any) => {
                this.apontamentosTfsDia = resultado.apontamentosTfsMes;
                this.apontamentosChannelDia = resultado.apontamentosChannelMes;
                this.batidas = resultado.batidas;
                this.infoJobCarga = resultado.jobInfo;

                this.servicoApontamento.consolidarTarefasEAtividadesDia(this.apontamentosTfsDia, this.apontamentosChannelDia);
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
}
