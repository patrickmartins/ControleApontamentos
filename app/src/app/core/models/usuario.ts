import { SafeUrl } from "@angular/platform-browser";
import { IModel } from "src/app/common/models/model";
import { UsuarioTfs } from "./usuarioTfs";
import { Funcionario } from "./funcionario";
import { UsuarioChannel } from "./usuarioChannel";
import { Unidade } from "./unidade";

export class Usuario implements IModel<Usuario> {
    public id: string = "";
    public idUsuarioTfs?: string;
    public idUsuarioChannel?: number;
    public idFuncionarioPonto?: number;
    public nomeCompleto: string = "";
	public nomeUsuario: string = "";
	public foto?: SafeUrl;
    public email: string = "";
	public roles: string[] = [];
	public colecoes: string[] = [];	

	public possuiContaPonto: boolean = false;
	public possuiContaTfs: boolean = false;
	public possuiContaChannel: boolean = false;

    public ehAdministrador?: boolean;
    
    public usuarioTfs?: UsuarioTfs;
    public usuarioChannel?: UsuarioChannel;
    public funcionarioPonto?: Funcionario;

    public idUnidade?: string;
    public unidade?: Unidade;
    
    public idGerente?: string;	
    public gerente?: Usuario;

	constructor() {	}

	public criarNovo(params: any): Usuario | undefined {
		if(!params)
			return undefined;

		let user = new Usuario();

		if(params) {
            user.id = params.id;
            user.idUsuarioTfs = params.idUsuarioTfs;
            user.idUsuarioChannel = params.idUsuarioChannel;
            user.idFuncionarioPonto = params.idFuncionarioPonto;
			user.nomeCompleto = params.nomeCompleto;
			user.nomeUsuario = params.nomeUsuario;
			user.email = params.email;
			user.roles = params.roles;
			user.colecoes = params.colecoes;
            user.ehAdministrador = params.ehAdministrador;
			user.possuiContaPonto = params.possuiContaPonto;
			user.possuiContaTfs = params.possuiContaTfs;
			user.possuiContaChannel = params.possuiContaChannel;
            user.idUnidade = params.idUnidade
            user.idGerente = params.idGerente
            
            user.unidade = new Unidade().criarNovo(params.unidade);
            user.gerente = new Usuario().criarNovo(params.gerente);
		}

		return user;
	}

	public possuiRole(role: string): boolean {
		return this.roles.some(c => c == role);
	}
}