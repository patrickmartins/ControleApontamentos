import { environment } from "src/environments/environment";
import { Configuracoes } from "../core/models/configuracoes";
import { LocalStorageHelper } from "./local-storage.helper";

export class ConfigHelper {

	private static _configuracoes?: Configuracoes;

	public static obterConfiguracoes(): Configuracoes {
		if(!this._configuracoes) {
			this._configuracoes = LocalStorageHelper.obterDados(environment.chaveStorageConfiguracoes, Configuracoes);

			if(!this._configuracoes)
				this._configuracoes = new Configuracoes();
		}

		return this._configuracoes;
	}

	public static salvarConfiguracoes(configuracoes: Configuracoes): void {
		this._configuracoes = configuracoes;

		LocalStorageHelper.salvarDados(environment.chaveStorageConfiguracoes, this._configuracoes);
	}
}