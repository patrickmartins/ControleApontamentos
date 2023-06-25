import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { RelatorioApontamentosUsuarioPorMes } from '../../models/relatorio-apontamentos-usuario-por-mes';
import { ActiveElement, ChartConfiguration, ChartData, Point, Tooltip } from 'chart.js';
import { TempoHelper } from 'src/app/helpers/tempo.helper';
import { BaseChartDirective } from 'ng2-charts';

@Component({
    selector: 'quadro-relatorio-apontamentos',
    templateUrl: './quadro-relatorio-apontamentos.component.html',
    styleUrls: ['./quadro-relatorio-apontamentos.component.scss']
})
export class QuadroRelatorioApontamentosComponent implements OnDestroy, OnInit {

    @Input()
    public relatorio!: RelatorioApontamentosUsuarioPorMes;

    @Input()
    public tolerancia: number = 0;

    @ViewChild(BaseChartDirective)
	private grafico: BaseChartDirective | undefined;
    
    public configGrafico: ChartConfiguration['options'] = {
		responsive: true,
		maintainAspectRatio: false,
        animation: false,
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
			padding: {
                top: 5
            }
		},
		plugins: {
			legend: {
				display: false			
			},
			tooltip: {
				callbacks: {
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
				hidden: false,
				label: 'Horas trabalhadas (Ponto)',
				data: [],
				barPercentage: 0.5,
                categoryPercentage: 1.3,
                borderRadius: 100,
				backgroundColor: [
					'#00b74a4d'
				],
				hoverBackgroundColor: [
					'#00b74ab3'
				],
                stack: 'trabalhadas'
			},
			{
				hidden: false,
				label: 'Horas apontadas (Channel)',
				data: [],
				barPercentage: 0.5,
                categoryPercentage: 1.3,
                borderRadius: 100,
				backgroundColor: [
					'#ffd9004d'
				],
				hoverBackgroundColor: [
					'#ffd900b3'
				],
                stack: 'apontadas'
			},
			{
				hidden: false,
				label: 'Horas apontadas (TFS) (não sincronizado com o Channel)',
				data: [],
				barPercentage: 0.5,
                categoryPercentage: 1.3,
                borderRadius: 100,
				backgroundColor: [
					'#f931544d'
				],
				hoverBackgroundColor: [
					'#f93154b3'
				],
                stack: 'apontadas'
			}
		]
	};
    
    constructor() { 
        Tooltip.positioners.average = (items: readonly ActiveElement[], eventPosition: Point) => {
			return eventPosition;
		};
    }
    
    public ngOnInit(): void {
        this.dadosGrafico.datasets[0].data = this.relatorio ? [this.relatorio.tempoTotalTrabalhadoNoMes] : [];	
        this.dadosGrafico.datasets[1].data = this.relatorio ? [this.relatorio.tempoTotalApontadoNoChannelNoMes] : [];
        this.dadosGrafico.datasets[2].data = this.relatorio ? [this.relatorio.tempoTotalApontadoNaoSincronizadoNoTfsNoMes] : [];

		this.grafico?.update();
	}

    public ngOnDestroy(): void {
        this.grafico?.ngOnDestroy();
    }
}
