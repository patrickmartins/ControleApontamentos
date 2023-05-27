import { Component, NgZone, OnInit, ViewChild } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { catchError, forkJoin, of } from 'rxjs';

import { BaseComponent } from 'src/app/common/components/base.component';
import { JobService } from 'src/app/core/services/job.service';
import { JobInfo } from 'src/app/core/models/job-info';
import { ApontamentosChannelDia } from '../../models/apontamentos-channel-dia';
import { ApontamentosChannelMes } from '../../models/apontamentos-channel-mes';
import { BatidasPontoMes } from '../../models/batidas-ponto-mes';
import { ApontamentosTfsMes } from '../../models/apontamentos-tfs-mes';
import { ApontamentosTfsDia } from '../../models/apontamentos-tfs-dia';
import { PontoService } from '../../services/ponto.service';
import { ContaService } from 'src/app/core/services/conta.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConsolidacaoService } from 'src/app/core/services/consolidacao.service';
import { TfsService } from 'src/app/core/services/tfs.service';
import { ChannelService } from '../../../core/services/channel.service';
import { GraficoResumoMesComponent } from '../grafico-resumo-mes/grafico-resumo-mes.component';
import { Tarefa } from 'src/app/core/models/tarefa';

@Component({
	selector: 'app-apontamentos-por-mes',
	templateUrl: './apontamentos-por-mes.component.html',
	styleUrls: ['./apontamentos-por-mes.component.scss']
})
export class ApontamentosPorMesComponent extends BaseComponent implements OnInit {

    @ViewChild('grafico')
    public grafico?: GraficoResumoMesComponent;

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
        snackBar: MatSnackBar,
		private servicoTfs: TfsService,
		private servicoPonto: PontoService,
        private servicoChannel: ChannelService,
        private servicoConsolidacao: ConsolidacaoService,
		private servicoJob: JobService,
		private dataAdapter: DateAdapter<any>,
		private zone: NgZone,
		private activeRoute: ActivatedRoute, 
		private router: Router) {

		super(servicoConta, snackBar);

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
		this.apontamentosChannelMes = undefined;
		this.apontamentosTfsMes = undefined;

        this.carregando = true;
		
		let mes = data.getMonth() + 1;
		let ano = data.getFullYear();
        this.apontamentosChannelMes = undefined;
        this.apontamentosTfsMes = undefined;       

        forkJoin({
            apontamentosTfsMes: this.usuarioLogado?.possuiContaTfs ? this.servicoTfs.obterApontamentosTfsPorMes(mes, ano).pipe(catchError(e => this.pipeErrosDeNegocio(e))) : of(undefined),
            apontamentosChannelMes: this.usuarioLogado?.possuiContaChannel ? this.servicoChannel.obterApontamentosChannelPorMes(mes, ano).pipe(catchError(e => this.pipeErrosDeNegocio(e))) : of(undefined),
            batidas: this.usuarioLogado?.possuiContaPonto ? this.servicoPonto.obterBatidasPorMes(mes, ano).pipe(catchError(e => this.pipeErrosDeNegocio(e))) : of(undefined),
            infoJobCarga: this.servicoJob.obterJobCarga()
        })		
        .subscribe({ 
            next: (resultado: any) => {
                this.apontamentosTfsMes = resultado.apontamentosTfsMes;
                this.apontamentosChannelMes = resultado.apontamentosChannelMes;
                this.batidas = resultado.batidas;
                this.infoJobCarga = resultado.infoJobCarga;

                this.servicoConsolidacao.consolidarTarefasEAtividades(this.apontamentosTfsMes, this.apontamentosChannelMes);

                this.selecionarUltimosApontamentos();
            },
            complete: () => this.carregando = false
        });
	}

    public onApontamentoSalvo(tarefa: Tarefa, apontamento: any): void {
        const apontamentosDia = this.apontamentosTfsMes?.obterApontamentosPorDia(apontamento.data.getDate());

        if(apontamentosDia) {
            const tarefaDiaApontado = apontamentosDia.obterTarefaPorId(tarefa.id);

            if(tarefaDiaApontado) {
                if(!tarefaDiaApontado.obterApontamentoPorHash(apontamento.hash)) {
                    tarefaDiaApontado.adicionarApontamento(apontamento!);
                }
            }
            else {
                apontamentosDia.adicionarTarefa(new Tarefa().criarNovo(tarefa)!);
            }
        }

        this.apontamentosTfsMes?.recalcularTempoTotalApontado();
        this.grafico?.atualizarGrafico();
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
