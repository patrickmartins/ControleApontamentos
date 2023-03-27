import { IModel } from "src/app/common/models/model";
import { JwtToken } from "./jwt.token";
import { Usuario } from "../../core/models/usuario";

export class LoginSucesso implements IModel<LoginSucesso> {
    public usuario?: Usuario;
	public token?: JwtToken;

	constructor() { }

	public criarNovo(params: any): LoginSucesso | undefined {
		if(!params)
			return undefined;

		let model = new LoginSucesso();

		if(params) {
			model.usuario = new Usuario().criarNovo(params.usuario)!;
			model.token = new JwtToken().criarNovo(params.token)!;
		}

		return model;
	}
}