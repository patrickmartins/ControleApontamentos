import { DOCUMENT } from "@angular/common";
import { Inject, Injectable } from "@angular/core";
import { Tema } from "../models/configuracoes";
import { ConfigHelper } from "../../helpers/config.helper";

@Injectable({
    providedIn: 'root'
})
export class TemaService {

	constructor(@Inject(DOCUMENT) private document: Document) {
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

		if(tema == Tema.Claro) {
			this.document.body.classList.remove("dark-mode");
		}
		else if(tema == Tema.Escuro) {
			this.document.body.classList.add("dark-mode");
		}
		else {
			var eTemaEscuro = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;

			if(eTemaEscuro) {
				this.document.body.classList.add("dark-mode");
			}
		}
	}
}