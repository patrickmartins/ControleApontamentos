import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DateAdapter } from '@angular/material/core';
import { tap } from 'rxjs';
import * as moment from 'moment';

import { ApontamentoService } from '../../services/apontamento.service';
import { ApontamentosTfsDia } from '../../models/apontamentos-tfs-dia';
import { PontoService } from '../../services/ponto.service';
import { BatidasPontoDia } from '../../models/batidas-ponto-dia';
import { ContaService } from 'src/app/core/services/conta.service';
import { BaseComponent } from 'src/app/common/components/base.component';
import { ApontamentosChannelDia } from '../../models/apontamentos-channel-dia';
import { JobInfo } from 'src/app/core/models/job-info';
import { environment } from 'src/environments/environment';
import { JobService } from 'src/app/core/services/job.service';

@Component({
	selector: 'app-apontamentos-por-dia',
	templateUrl: './apontamentos-por-dia.component.html',
	styleUrls: ['./apontamentos-por-dia.component.scss']
})
export class ApontamentosPorDiaComponent extends BaseComponent implements OnInit {

	public get carregando(): boolean {
		return this.carregandoApontamentosTfs || this.carregandoApontamentosChannel || this.carregandoBatidasPonto;
	}

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
	
	public carregandoApontamentosTfs: boolean = true;
	public carregandoApontamentosChannel: boolean = true;
	public carregandoBatidasPonto: boolean = true;

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

		this.carregandoApontamentosTfs = true;
		this.carregandoApontamentosChannel = true;
		this.carregandoBatidasPonto = true;

		if(this.usuarioLogado?.possuiContaTfs) {
			this.servicoApontamento
				.obterApontamentosTfsPorDia(data)
				.pipe(tap((apontamentosTfsDia: ApontamentosTfsDia) => this.consolidarTarefasEAtividades(apontamentosTfsDia, this.apontamentosChannelDia)))
				.subscribe({
					next: (apontamentos) => {
						this.apontamentosTfsDia = apontamentos;
					},
					complete: () => this.carregandoApontamentosTfs = false
				});
		}
		else {
			this.carregandoApontamentosTfs = false;
		}

		if(this.usuarioLogado?.possuiContaChannel) {
			this.servicoApontamento
				.obterApontamentosChannelPorDia(data)
				.pipe(tap((apontamentosChannelDia: ApontamentosChannelDia) => this.consolidarTarefasEAtividades(this.apontamentosTfsDia, apontamentosChannelDia)))
				.subscribe({
					next: (apontamentos) => {
						this.apontamentosChannelDia = apontamentos;
					},
					complete: () => this.carregandoApontamentosChannel = false
				});
		}
		else {
			this.carregandoApontamentosChannel = false;
		}

		if(this.usuarioLogado?.possuiContaPonto) {
			this.servicoPonto
				.obterBatidasPorDia(data)
				.subscribe({
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

	private consolidarTarefasEAtividades(apontamentosTfs: ApontamentosTfsDia | undefined, apontamentosChannel: ApontamentosChannelDia | undefined): void {
		if (apontamentosTfs && apontamentosChannel) {
            var apontamentosChannelTfs = apontamentosChannel.obterApontamentosTfs();
            
            apontamentosChannelTfs = apontamentosChannelTfs.filter((apontamento, i) => apontamentosChannelTfs.findIndex(c => c.hash === apontamento.hash) === i);

            apontamentosChannelTfs.forEach(apontamento => {
                var tarefas = apontamentosTfs.obterTarefasPorId(apontamento.idTarefaTfs);

                tarefas.forEach(tarefa => {
                    tarefa.removerApontamentoPorHash(apontamento.hash);
                });
            })

            apontamentosChannel.removerApontamentosExcluidos();
            apontamentosChannel.removerTarefasSemApontamentos();
            apontamentosChannel.recalcularTempoTotalApontado();

            apontamentosTfs.removerTarefasSemApontamentos();
            apontamentosTfs.recalcularTempoTotalApontado();
        }
        else if (!this.usuarioLogado?.possuiContaTfs && apontamentosChannel) {
            apontamentosChannel.removerApontamentosExcluidos();
            apontamentosChannel.removerTarefasSemApontamentos();
            apontamentosChannel.recalcularTempoTotalApontado();
        }
	}
}
