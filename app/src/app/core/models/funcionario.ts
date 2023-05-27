import { IModel } from "src/app/common/models/model";

export class Funcionario implements IModel<Funcionario> {
    public id: number = 0;
    public nomeCompleto: string = "";
    public email: string = "";
    public numeroFolha: string = "";

	constructor() {	}

	public criarNovo(params: any): Funcionario | undefined {
		if(!params)
			return undefined;

		let user = new Funcionario();

		if(params) {
            user.id = params.id;
            user.nomeCompleto = params.nome;
			user.email = params.email;
            user.numeroFolha = params.numeroFolha;
		}

		return user;
	}
}