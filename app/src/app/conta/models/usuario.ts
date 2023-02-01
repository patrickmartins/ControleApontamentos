import { SafeUrl } from "@angular/platform-browser";
import { IModel } from "src/app/common/models/model";

export class Usuario implements IModel<Usuario> {
    public nomeCompleto: string = "";
	public nomeUsuario: string = "";
	public foto?: SafeUrl;
    public email: string = "";
	public roles: string[] = [];
	public colecoes: string[] = [];	

	public possuiContaPonto: boolean = false;
	public possuiContaTfs: boolean = false;
	public possuiContaChannel: boolean = false;

	constructor() {	}

	public criarNovo(params: any): Usuario | undefined {
		if(!params)
			return undefined;

		let user = new Usuario();

		if(params) {
			user.nomeCompleto = params.nomeCompleto;
			user.nomeUsuario = params.nomeUsuario;
			user.email = params.email;
			user.roles = params.roles;
			user.colecoes = params.colecoes;
			user.possuiContaPonto = params.possuiContaPonto;
			user.possuiContaTfs = params.possuiContaTfs;
			user.possuiContaChannel = params.possuiContaChannel;
		}

		return user;
	}

	public possuiRole(role: string): boolean {
		return this.roles.some(c => c == role);
	}
}