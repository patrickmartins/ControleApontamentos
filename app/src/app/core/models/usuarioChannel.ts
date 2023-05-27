import { IModel } from "src/app/common/models/model";

export class UsuarioChannel implements IModel<UsuarioChannel> {
    public id: number = 0;
    public nomeCompleto: string = "";
    public nomeUsuario: string = "";
    public email: string = "";

	constructor() {	}

	public criarNovo(params: any): UsuarioChannel | undefined {
		if(!params)
			return undefined;

		let user = new UsuarioChannel();

		if(params) {
            user.id = params.id;
            user.nomeCompleto = params.nomeCompleto;
            user.nomeUsuario = params.nomeUsuario;
			user.email = params.email;
		}

		return user;
	}
}