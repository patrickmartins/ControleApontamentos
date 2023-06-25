import 'moment/locale/pt-br';

import { IModel } from 'src/app/common/models/model';

export class Unidade implements IModel<Unidade> {
	
	public id: string = "";
	public nome: string = "";

	public criarNovo(params: any): Unidade | undefined {
		if (!params)
			return undefined;

		let apontamento = new Unidade();

		if (params) {
			apontamento.id = params.id;
			apontamento.nome = params.nome;
		}

		return apontamento;
	}
}