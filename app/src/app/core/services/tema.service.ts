import { DOCUMENT } from "@angular/common";
import { Inject, Injectable } from "@angular/core";
import { Tema } from "../models/configuracoes";
import { ConfigHelper } from "../../helpers/config.helper";
import { ThemeService } from "ng2-charts";
import { ChartOptions } from "chart.js";
import { BehaviorSubject, Observable, Subject } from "rxjs";

@Injectable({
	providedIn: 'root'
})
export class TemaService {

	private _onTemaAlteradoSubscription : Subject<number>;
	public onTemaAlterado : Observable<Tema | undefined>;   

	constructor(@Inject(DOCUMENT) private document: Document, private servicoTemaGrafico: ThemeService) {
		const tema = this.obterTemaAtual();

		this._onTemaAlteradoSubscription = new BehaviorSubject(tema);
		this.onTemaAlterado = this._onTemaAlteradoSubscription.asObservable();

		this.alterarTemaAtual(tema);
	}

	public ehTemaEscuro(): boolean {
		let tema = ConfigHelper.obterConfiguracoes().tema;

		if (tema == Tema.Claro) {
			return false;
		}
		else if (tema == Tema.Escuro) {
			return true;
		}
		else {
			let ehTemaEscuro = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;

			if (ehTemaEscuro) {
				return true;
			}
			else {
				return false;
			}
		}
	}

	public obterTemaAtual(): Tema {
		return ConfigHelper.obterConfiguracoes().tema;
	}

	public alterarTemaAtual(tema: Tema): void {
		let configuracoes = ConfigHelper.obterConfiguracoes();

		configuracoes.tema = tema;

		ConfigHelper.salvarConfiguracoes(configuracoes);

		this.aplicarTemaAtual();

		this._onTemaAlteradoSubscription.next(tema);
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
			let ehTemaEscuro = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;

			if (ehTemaEscuro) {
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
        this.document.getElementsByTagName("html")[0].style.colorScheme = "dark";

		this.servicoTemaGrafico.setColorschemesOptions(opcoesGrafico);
	}

	private aplicarTemaClaro(): void {
		this.document.body.classList.remove("dark-mode");
		this.document.getElementsByTagName("html")[0].style.colorScheme = "auto";

		this.servicoTemaGrafico.setColorschemesOptions({});
	}
}