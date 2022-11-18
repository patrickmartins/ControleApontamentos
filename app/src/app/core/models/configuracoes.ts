import { IModel } from "src/app/common/models/model";

export enum Tema {
	Sistema = 0,
	Claro,
	Escuro
}

export class Configuracoes implements IModel<Configuracoes> {

	public tema: Tema = Tema.Sistema;

	public criarNovo(params: any): Configuracoes | undefined {
		if (!params)
			return undefined;

		let config = new Configuracoes();

		if (params) {
			config.tema = params.tema;
		}

		return config;
	}
}