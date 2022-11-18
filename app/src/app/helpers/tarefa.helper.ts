import { environment } from "src/environments/environment";
import { Tarefa } from "../core/models/tarefa";
import { TarefaFixada } from "../core/models/tarefa-fixada";
import { LocalStorageHelper } from "./local-storage.helper";

export class TarefaHelper {

	private static _tarefasFixadas?: TarefaFixada[];

	public static obterTarefasFixadas(): TarefaFixada[] {
		if (this._tarefasFixadas)
			return this._tarefasFixadas;

		if (LocalStorageHelper.dadoExiste(environment.chaveStorageTarefasFixadas)) {
			this._tarefasFixadas = LocalStorageHelper.obterDados<TarefaFixada[]>(environment.chaveStorageTarefasFixadas, TarefaFixada)!;
		}
		else {
			this._tarefasFixadas = []
		}

		return this._tarefasFixadas;
	}

	public static fixarTarefa(tarefa: Tarefa) {
		let tarefasFixadas = this.obterTarefasFixadas();

		if (!tarefasFixadas.some(c => c.idTarefa == tarefa.id && c.colecao == tarefa.colecao)) {

			tarefasFixadas.push(new TarefaFixada().criarNovo({
				idTarefa: tarefa.id,
				colecao: tarefa.colecao
			})!)

			LocalStorageHelper.salvarDados(environment.chaveStorageTarefasFixadas, tarefasFixadas);

			this._tarefasFixadas = tarefasFixadas;
		}
	}

	public static desafixarTarefa(tarefa: Tarefa) {
		let tarefasFixadas = this.obterTarefasFixadas().filter(c => c.idTarefa != tarefa.id || c.colecao != tarefa.colecao);

		LocalStorageHelper.salvarDados(environment.chaveStorageTarefasFixadas, tarefasFixadas);

		this._tarefasFixadas = tarefasFixadas;
	}

	
	public static agruparTarefasFixadas(tarefasFixadas: any[]): any {
		let tarefas = new Array<any>();

		tarefasFixadas.reduce((g, tarefa) => {
			const { colecao } = tarefa;

			let grupo = tarefas.find(c => c.key == colecao);

			if (grupo) {
				grupo.values.push(tarefa);
			} else 
			{
				grupo = { key: colecao, values: [tarefa]};

				tarefas.push(grupo)
			}
		}, {});

		return tarefas;
	}
}