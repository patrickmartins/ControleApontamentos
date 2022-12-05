import * as moment from "moment";
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
			model.validade = typeof params.validade === 'number' ? moment().utc().add(params.validade, 'minutes').toDate() : moment(params.validade).toDate();
		}

		return model;
	}

	public Expirou(): boolean {
		return moment().utc().toDate() > this.validade;
	}
}