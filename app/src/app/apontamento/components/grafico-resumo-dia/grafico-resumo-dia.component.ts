import { Component, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { ActiveElement, ChartConfiguration, ChartData, Point, Tooltip } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { TempoHelper } from 'src/app/helpers/tempo.helper';
import { ApontamentosDia } from '../../models/apontamentos-dia';
import { BatidasPontoDia } from '../../models/batidas-ponto-dia';

@Component({
	selector: 'grafico-resumo-dia',
	templateUrl: './grafico-resumo-dia.component.html',
	styleUrls: ['./grafico-resumo-dia.component.scss']
})
export class GraficoResumoDiaComponent implements OnChanges {

	public configGrafico: ChartConfiguration['options'] = {
		responsive: true,
		maintainAspectRatio: false,
		indexAxis: 'y',
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
	public apontamentos?: ApontamentosDia;

	@Input()
	public batidas?: BatidasPontoDia;

	@ViewChild(BaseChartDirective)
	private grafico: BaseChartDirective | undefined;

	constructor() {
		Tooltip.positioners.average = (items: readonly ActiveElement[], eventPosition: Point) => {
			return eventPosition;
		};
	}

	public ngOnChanges(changes: SimpleChanges): void {
		this.atualizarGrafico();
	}

	public atualizarGrafico(): void {
		if(this.batidas) {
			this.dadosGrafico.datasets[0].data = [this.batidas.tempoTotalTrabalhadoNoDia];
			this.dadosGrafico.datasets[0].hidden = this.batidas.tempoTotalTrabalhadoNoDia <= 0;
		}

		if(this.apontamentos) {
			this.dadosGrafico.datasets[1].data = [0, this.apontamentos.tempoTotalApontadoSincronizadoChannel];
			this.dadosGrafico.datasets[1].hidden = this.apontamentos.tempoTotalApontadoSincronizadoChannel <= 0;

			this.dadosGrafico.datasets[2].data = [0, this.apontamentos.tempoTotalApontadoNaoSincronizadoChannel];
			this.dadosGrafico.datasets[2].hidden = this.apontamentos.tempoTotalApontadoNaoSincronizadoChannel <= 0;
		}

		this.grafico?.update();
	}
}