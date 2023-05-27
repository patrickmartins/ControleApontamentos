import { IModel } from "src/app/common/models/model";

export class UsuarioTfs implements IModel<UsuarioTfs> {
    public id: string = "";
    public nomeCompleto: string = "";
    public nomeUsuario: string = "";
    public dominio: string = "";
    public email: string = "";

	constructor() {	}

	public criarNovo(params: any): UsuarioTfs | undefined {
		if(!params)
			return undefined;

		let user = new UsuarioTfs();

		if(params) {
            user.id = params.identidade ? params.identidade.id : "";
            user.nomeCompleto = params.nomeCompleto;
            user.nomeUsuario = params.nomeUsuario;
            user.dominio = params.dominio;
			user.email = params.email;
		}

		return user;
	}
}