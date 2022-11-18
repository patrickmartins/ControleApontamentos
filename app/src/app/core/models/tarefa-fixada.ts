
import { IModel } from "src/app/common/models/model";

export class TarefaFixada implements IModel<TarefaFixada> {

	public idTarefa: number = 0;
	public colecao: string = "";

	public criarNovo(params: any): TarefaFixada | undefined {
		if(!params)
			return undefined;

		let tarefaFixada = new TarefaFixada();

		if(params) {
			tarefaFixada.idTarefa = params.idTarefa;
			tarefaFixada.colecao = params.colecao;
		}

		return tarefaFixada;
	}

}