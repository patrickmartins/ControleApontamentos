import { AfterViewInit, Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { ActiveElement, Chart, ChartConfiguration, ChartData, Point, Tooltip } from 'chart.js';
import * as moment from 'moment';
import { BaseChartDirective } from 'ng2-charts';
import { debounceTime, fromEvent, Observable, Subscription } from 'rxjs';

import { TempoHelper } from 'src/app/helpers/tempo.helper';
import { ApontamentosChannelMes } from '../../models/apontamentos-channel-mes';
import { ApontamentosTfsMes } from '../../models/apontamentos-tfs-mes';
import { BatidasPontoMes } from '../../models/batidas-ponto-mes';

@Component({
	selector: 'grafico-resumo-mes',
	templateUrl: './grafico-resumo-mes.component.html',
	styleUrls: ['./grafico-resumo-mes.component.scss']
})
export class GraficoResumoMesComponent implements OnChanges, OnInit, OnDestroy, AfterViewInit {

	public configGraficoVertical: ChartConfiguration['options'] = {		
		indexAxis: 'x',
		responsive: true,
		maintainAspectRatio: true,
		onClick: (grafico, elemento) => {
			if (elemento.length == 0)
				return;

			this.onDiaClicado.emit(elemento[0].index + 1);
		},
		scales: {
			x: {
				grid: {
					display: false,
					drawBorder: false
				},
				stacked: true
			},
			y: {
				display: false,
				grid: {
					display: false,
					drawBorder: false
				},
				stacked: true
			}
		},
		layout: {
			padding: 0
		},
		plugins: {
			legend: {
				display: true,
				position: 'bottom',
				labels: {
					padding: 15
				}				
			},
			tooltip: {
				callbacks: {
					title: (contexto) => "",
					footer: (contexto) => {
						if (contexto.length === 0 || contexto[0].datasetIndex != 0)
							return "";

						let batidasDia =  this.batidas ? this.batidas.obterBatidasPorDia(contexto[0].dataIndex + 1) : null;

						if (!batidasDia)
							return "";

						return batidasDia.toString()
					},
					label: (contexto) => {
						if (!contexto)
							return "";

						return TempoHelper.converterParaHorasEmExtenso(contexto.parsed.y);
					}
				}
			}
		}
	};

	public configGraficoHorizontal: ChartConfiguration['options'] = {
		indexAxis: 'y',		
		responsive: true,
		maintainAspectRatio: false,
		onClick: (grafico, elemento) => {
			if (elemento.length == 0)
				return;

			this.onDiaClicado.emit(elemento[0].index + 1);
		},
		scales: {
			x: {
				display: false,
				grid: {
					display: false,
					drawBorder: false
				},
				stacked: true
			},
			y: {				
				grid: {
					display: false,
					drawBorder: false
				},
				stacked: true
			}
		},
		layout: {
			padding: 0
		},
		plugins: {
			legend: {
				display: true,
				position: 'bottom',
				labels: {
					padding: 15
				}				
			},
			tooltip: {
				callbacks: {
					title: (contexto) => "",
					footer: (contexto) => {
						if (contexto.length === 0 || contexto[0].datasetIndex != 0)
							return "";

						let batidasDia =  this.batidas ? this.batidas.obterBatidasPorDia(contexto[0].dataIndex + 1) : null;

						if (!batidasDia)
							return "";

						return batidasDia.toString()
					},
					label: (contexto) => {
						if (!contexto)
							return "";

						return TempoHelper.converterParaHorasEmExtenso(contexto.parsed.y);
					}
				}
			}
		}
	};

	public dadosGrafico: ChartData<'bar'> = {
		labels: [],
		datasets: [
			{
				hidden: true,
				label: 'Horas trabalhadas (Ponto)',
				data: [],
				backgroundColor: [
					'#00b74a4d'
				],
				hoverBackgroundColor: [
					'#00b74ab3'
				],
				stack: "Trabalhadas"
			},
			{
				hidden: true,
				label: 'Horas apontadas (Channel)',
				data: [],
				backgroundColor: [
					'#ffd9004d'
				],
				hoverBackgroundColor: [
					'#ffd900b3'
				],
				stack: "Apontados"
			},
			{
				hidden: true,
				label: 'Horas apontadas (TFS)',
				data: [],
				backgroundColor: [
					'#36a2eb4d'
				],
				hoverBackgroundColor: [
					'#36a2ebb3'
				],
				stack: "Apontados"
			},
			{
				hidden: true,
				label: 'Horas apontadas (TFS) (não sincronizado com o Channel)',
				data: [],
				backgroundColor: [
					'#f931544d'
				],
				hoverBackgroundColor: [
					'#f93154b3'
				],
				stack: "Apontados"
			},
		]
	};

	@Input()
	public apontamentosTfs?: ApontamentosTfsMes;

	@Input()
	public apontamentosChannel?: ApontamentosChannelMes;

	@Input()
	public batidas?: BatidasPontoMes;

	@Input()
	public dataReferencia: Date = new Date();

	@Output()
	public onDiaClicado = new EventEmitter<number>;

	@ViewChild(BaseChartDirective)
	private grafico: BaseChartDirective | undefined;

	public resizeObservable?: Observable<Event>;
  	public resizeSubscription?: Subscription;

	constructor() {
		Tooltip.positioners.average = (items: readonly ActiveElement[], eventPosition: Point) => {
			return eventPosition;
		};
	}
	
	public ngOnInit(): void {
		this.resizeObservable = fromEvent(window, "resize");
   
		this.resizeSubscription = this.resizeObservable
										.pipe(debounceTime(100))
										.subscribe((evento: any) => {
											this.onJanelaRedimencionada(window.innerWidth);
										});
	}

	public ngAfterViewInit(): void {
		this.onJanelaRedimencionada(window.innerWidth);
	}

	public ngOnDestroy(): void {
		this.resizeSubscription?.unsubscribe();
	}

	public ngOnChanges(changes: SimpleChanges): void {
		this.atualizarGrafico();		
	}

	private atualizarGrafico(): void {
		this.dadosGrafico.labels = this.obterDiasPorMes(this.dataReferencia.getMonth(), this.dataReferencia.getFullYear());

		if(this.batidas) {
			this.dadosGrafico.datasets[0].data = this.batidas.batidasDiarias.map(c => c.tempoTotalTrabalhadoNoDia);
			this.dadosGrafico.datasets[0].hidden = this.batidas.tempoTotalTrabalhadoNoMes <= 0;
		}

		if(this.apontamentosChannel) {
			this.dadosGrafico.datasets[1].data = this.apontamentosChannel.apontamentosDiarios.map(c => c.tempoTotalApontadoNoDia);
			this.dadosGrafico.datasets[1].hidden = this.apontamentosChannel.tempoTotalApontadoNoMes <= 0;
		}

		if(this.apontamentosTfs) {
			this.dadosGrafico.datasets[2].data = this.apontamentosTfs.apontamentosDiarios.map(c => c.tempoTotalApontadoSincronizadoChannel);
			this.dadosGrafico.datasets[2].hidden = this.apontamentosTfs.tempoTotalApontadoSincronizadoChannel <= 0;

			this.dadosGrafico.datasets[3].data = this.apontamentosTfs.apontamentosDiarios.map(c => c.tempoTotalApontadoNaoSincronizadoChannel);
			this.dadosGrafico.datasets[3].hidden = this.apontamentosTfs.tempoTotalApontadoNaoSincronizadoChannel <= 0;
		}

		this.grafico?.update();				
	}

	private obterDiasPorMes(mes: number, ano: number): number[] {
		const data = moment(new Date(ano, mes, 1));
		const diasMes = data.daysInMonth();

		return Array(diasMes).fill(null).map((value, index) => ++index);
	}

	private onJanelaRedimencionada(largura: number) {
		if(largura < 680) {
			this.alternarGraficoHorizontal();
		} else if(largura >= 680) {
			this.alternarGraficoVertical();
		}
	}

	private alternarGraficoHorizontal() {
		const instancia = Chart.instances[0];

		if(instancia && instancia.config.options?.indexAxis == 'x') {				
			instancia.config.options = this.configGraficoHorizontal!;

			instancia.render();	
			instancia.resize(4000, 1000);
		}
	}

	private alternarGraficoVertical() {	
		const instancia = Chart.instances[0];

		if(instancia && instancia.config.options?.indexAxis == 'y') {	
			instancia.config.options = this.configGraficoVertical!;

			instancia.render();	
			instancia.resize(900, 210);
		}
	}
}