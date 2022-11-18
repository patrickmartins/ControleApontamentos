import * as moment from "moment";
import { IModel } from "src/app/common/models/model";
import { Tarefa } from "src/app/core/models/tarefa";

export class ApontamentosDia implements IModel<ApontamentosDia> {

	public dataReferencia: Date = new Date();
	public tempoTotalApontadoNoDia: number = 0;
	public tempoTotalApontadoSincronizadoChannel: number = 0;
	public tempoTotalApontadoNaoSincronizadoChannel: number = 0;
	public tarefas: Tarefa[] = [];

	public criarNovo(params: any): ApontamentosDia | undefined {
		if (!params)
			return undefined;

		let apontamento = new ApontamentosDia();

		if (params) {
			apontamento.dataReferencia = moment(params.dataReferencia).toDate();
			apontamento.tempoTotalApontadoNoDia = params.tempoTotalApontadoNoDia;
			apontamento.tempoTotalApontadoSincronizadoChannel = params.tempoTotalApontadoSincronizadoChannel;
			apontamento.tempoTotalApontadoNaoSincronizadoChannel = params.tempoTotalApontadoNaoSincronizadoChannel;

			apontamento.tarefas = Array.isArray(params.tarefas) ? Array.from(params.tarefas).map(item => new Tarefa().criarNovo(item)!) : [];
		}

		return apontamento;
	}
}