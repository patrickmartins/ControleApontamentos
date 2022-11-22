import { DOCUMENT } from "@angular/common";
import { Inject, Injectable } from "@angular/core";
import { Tema } from "../models/configuracoes";
import { ConfigHelper } from "../../helpers/config.helper";
import { ThemeService } from "ng2-charts";
import { ChartOptions } from "chart.js";

@Injectable({
	providedIn: 'root'
})
export class TemaService {

	constructor(@Inject(DOCUMENT) private document: Document, private servicoTemaGrafico: ThemeService) {
		const tema = this.obterTemaAtual();

		this.alterarTemaAtual(tema);
	}

	public obterTemaAtual(): Tema {
		return ConfigHelper.obterConfiguracoes().tema;
	}

	public alterarTemaAtual(tema: Tema): void {
		let configuracoes = ConfigHelper.obterConfiguracoes();

		configuracoes.tema = tema;

		ConfigHelper.salvarConfiguracoes(configuracoes);

		this.aplicarTemaAtual();
	}

	public aplicarTemaAtual() {
		let tema = ConfigHelper.obterConfiguracoes().tema;

		if (tema == Tema.Claro) {
			this.aplicarTemaClaro();
		}
		else if (tema == Tema.Escuro) {
			this.aplicarTemaEscuro();
		}
		else {
			var eTemaEscuro = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;

			if (eTemaEscuro) {
				this.aplicarTemaEscuro();
			}
			else {
				this.aplicarTemaClaro();
			}
		}
	}

	private aplicarTemaEscuro(): void {
		let opcoesGrafico: ChartOptions = {
				plugins: {
					legend: {
						labels: { color: 'white' }
					}
				},
				scales: {
					x: {
						ticks: {
							color: 'white'
						}
					},
					y: {
						ticks: {
							color: 'white'
						}
					}
				}
			};

		this.document.body.classList.add("dark-mode");

		this.servicoTemaGrafico.setColorschemesOptions(opcoesGrafico);
	}

	private aplicarTemaClaro(): void {
		this.document.body.classList.remove("dark-mode");
		
		this.servicoTemaGrafico.setColorschemesOptions({});
	}
}