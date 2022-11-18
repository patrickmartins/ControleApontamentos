import { IModel } from "src/app/common/models/model";

export class JwtToken implements IModel<JwtToken> {
    public tokenAcesso: string = "";
    public validade: Date = new Date();

	constructor() { }

	public criarNovo(params: any): JwtToken | undefined {
		if(!params)
			return undefined;

		let model = new JwtToken();

		if(params) {
			model.tokenAcesso = params.tokenAcesso;
			model.validade = params.validade;
		}

		return model;
	}
}