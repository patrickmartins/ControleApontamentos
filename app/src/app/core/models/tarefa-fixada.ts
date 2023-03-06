
import { IModel } from "src/app/common/models/model";

export class TarefaFixada implements IModel<TarefaFixada> {

	public idTarefa: number = 0;
	public colecao: string = "";
	public usuario: string = "";

	public criarNovo(params: any): TarefaFixada | undefined {
		if(!params)
			return undefined;

		let tarefaFixada = new TarefaFixada();

		if(params) {
			tarefaFixada.idTarefa = params.idTarefa as number;
			tarefaFixada.colecao = params.colecao;
			tarefaFixada.usuario = params.usuario;
		}

		return tarefaFixada;
	}

}