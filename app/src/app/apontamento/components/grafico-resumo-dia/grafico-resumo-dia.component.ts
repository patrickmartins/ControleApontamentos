import { Component, Input, OnChanges, OnDestroy, SimpleChanges, ViewChild } from '@angular/core';
import { ActiveElement, ChartConfiguration, ChartData, Point, Tooltip } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { TempoHelper } from 'src/app/helpers/tempo.helper';
import { ApontamentosChannelDia } from '../../models/apontamentos-channel-dia';
import { ApontamentosTfsDia } from '../../models/apontamentos-tfs-dia';
import { BatidasPontoDia } from '../../models/batidas-ponto-dia';

@Component({
	selector: 'grafico-resumo-dia',
	templateUrl: './grafico-resumo-dia.component.html',
	styleUrls: ['./grafico-resumo-dia.component.scss']
})
export class GraficoResumoDiaComponent implements OnChanges, OnDestroy {

	public configGrafico: ChartConfiguration['options'] = {
		responsive: true,
		maintainAspectRatio: false,
		indexAxis: 'y',
		scales: {
			x: {
				display: false,
				grid: {
					display: false                    
				},
				stacked: true
			},
			y: {
				display: false,
				grid: {
					display: false
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
					footer: (contexto) => {
						if (contexto.length == 0)
							return "";

						if (contexto[0].datasetIndex == 0)
							return this.batidas!.toString();

						return "";
					},
					label: (contexto) => {
						if (!contexto)
							return "";

						return TempoHelper.converterParaHorasEmExtenso(contexto.parsed.x);
					}
				}
			}
		}
	};

	public dadosGrafico: ChartData<'bar'> = {
		labels: ['', ''],
		datasets: [
			{
				hidden: true,
				label: 'Horas trabalhadas (Ponto)',
				data: [],
				barThickness: 20,
				backgroundColor: [
					'#00b74a4d'
				],
				hoverBackgroundColor: [
					'#00b74ab3'
				]
			},
			{
				hidden: true,
				label: 'Horas apontadas (Channel)',
				data: [],
				barThickness: 20,
				backgroundColor: [
					'#ffd9004d'
				],
				hoverBackgroundColor: [
					'#ffd900b3'
				]
			},
			{
				hidden: true,
				label: 'Horas apontadas (TFS)',
				data: [],
				barThickness: 20,
				backgroundColor: [
					'#36a2eb4d'
				],
				hoverBackgroundColor: [
					'#36a2ebb3'
				]
			},
			{
				hidden: true,
				label: 'Horas apontadas (TFS) (não sincronizado com o Channel)',
				data: [],
				barThickness: 20,
				backgroundColor: [
					'#f931544d'
				],
				hoverBackgroundColor: [
					'#f93154b3'
				]
			}
		]
	};

	@Input()
	public apontamentosTfs?: ApontamentosTfsDia;

	@Input()
	public apontamentosChannel?: ApontamentosChannelDia;

	@Input()
	public batidas?: BatidasPontoDia;

	@ViewChild(BaseChartDirective)
	private grafico: BaseChartDirective | undefined;

	constructor() {
		Tooltip.positioners.average = (items: readonly ActiveElement[], eventPosition: Point) => {
			return eventPosition;
		};
	}

    public ngOnDestroy(): void {
        this.grafico?.ngOnDestroy();
    }

	public ngOnChanges(changes: SimpleChanges): void {
		this.atualizarGrafico();
	}

	public atualizarGrafico(): void {
		this.dadosGrafico.datasets[0].data = this.batidas ? [this.batidas.tempoTotalTrabalhadoNoDia] : [];
		this.dadosGrafico.datasets[0].hidden = !this.batidas || this.batidas.tempoTotalTrabalhadoNoDia <= 0;		

        this.dadosGrafico.datasets[1].data = this.apontamentosChannel ? [0, this.apontamentosChannel.tempoTotalApontadoNoDia] : [];
        this.dadosGrafico.datasets[1].hidden = !this.apontamentosChannel || this.apontamentosChannel.tempoTotalApontadoNoDia <= 0;

        this.dadosGrafico.datasets[2].data = this.apontamentosTfs ? [0, this.apontamentosTfs.tempoTotalApontadoSincronizadoChannel] : [];
        this.dadosGrafico.datasets[2].hidden = !this.apontamentosTfs || this.apontamentosTfs.tempoTotalApontadoSincronizadoChannel <= 0;

        this.dadosGrafico.datasets[3].data = this.apontamentosTfs ? [0, this.apontamentosTfs.tempoTotalApontadoNaoSincronizadoChannel]: [];
        this.dadosGrafico.datasets[3].hidden = !this.apontamentosTfs || this.apontamentosTfs.tempoTotalApontadoNaoSincronizadoChannel <= 0;		

		this.grafico?.update();
	}
}