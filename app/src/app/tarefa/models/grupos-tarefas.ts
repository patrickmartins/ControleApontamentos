import { IModel } from "src/app/common/models/model";
import { Tarefa } from "src/app/core/models/tarefa";

export class GrupoTarefas implements IModel<GrupoTarefas> {
	public nome: string = "";
	public tarefas: Tarefa[] = [];
	
	constructor() { }

	public criarNovo(params: any): GrupoTarefas | undefined {
		if(!params)
		return undefined;

		let grupo = new GrupoTarefas();

		if(params) {
			grupo.nome = params.nome;	

			grupo.tarefas = Array.isArray(params.tarefas) ? Array.from(params.tarefas).map(item => new Tarefa().criarNovo(item)!) : [];
		}

		return grupo;
	}
	
}