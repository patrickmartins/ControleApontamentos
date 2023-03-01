import { IModel } from "src/app/common/models/model";

export class Designado implements IModel<Designado> {

	public nome: string = "";
    public usuario: string = "";
	
	constructor() { }

	public criarNovo(params: any): Designado | undefined {
		if(!params)
			return undefined;

		let designado = new Designado();

		if(params) {
			designado.nome = params.nome;
			designado.usuario = params.usuario;
        }	

		return designado;
	}
}