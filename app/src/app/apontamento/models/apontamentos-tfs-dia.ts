import * as moment from "moment";
import { IModel } from "src/app/common/models/model";
import { Tarefa } from "src/app/core/models/tarefa";

export class ApontamentosTfsDia implements IModel<ApontamentosTfsDia> {

	public usuarioReferencia: string = "";
	public dataReferencia: Date = new Date();
	public tempoTotalApontadoNoDia: number = 0;
	public tempoTotalApontadoSincronizadoChannel: number = 0;
	public tempoTotalApontadoNaoSincronizadoChannel: number = 0;
	public tarefas: Tarefa[] = [];

	public criarNovo(params: any): ApontamentosTfsDia | undefined {
		if (!params)
			return undefined;

		let apontamento = new ApontamentosTfsDia();

		if (params) {
			apontamento.usuarioReferencia = params.usuarioReferencia;
			apontamento.dataReferencia = moment(params.dataReferencia).toDate();
			apontamento.tempoTotalApontadoNoDia = params.tempoTotalApontadoNoDia;
			apontamento.tempoTotalApontadoSincronizadoChannel = params.tempoTotalApontadoSincronizadoChannel;
			apontamento.tempoTotalApontadoNaoSincronizadoChannel = params.tempoTotalApontadoNaoSincronizadoChannel;

			apontamento.tarefas = Array.isArray(params.tarefas) ? Array.from(params.tarefas).map(item => new Tarefa().criarNovo(item)!) : [];
		}

		return apontamento;
	}

	public obterTarefasPorId(id: number): Tarefa[] {
		return this.tarefas.filter(c => c.id == id);
	}

	public recalcularTempoTotalApontado(): void {
		this.recalcularTempoTotalApontadoSincronizadoChannel();
		this.recalcularTempoTotalApontadoNaoSincronizadoChannel();

		this.tempoTotalApontadoNoDia = this.tempoTotalApontadoNaoSincronizadoChannel + this.tempoTotalApontadoSincronizadoChannel;
	}

	public recalcularTempoTotalApontadoSincronizadoChannel(): void {
		this.tempoTotalApontadoSincronizadoChannel = 0;

		this.tarefas?.forEach(tarefa => {
			tarefa.recalcularTempoTotalApontadoSincronizadoChannel(this.usuarioReferencia, this.dataReferencia);

			this.tempoTotalApontadoSincronizadoChannel += tarefa.obterTempoApontadoPorData(this.usuarioReferencia, true, this.dataReferencia);
		});
	}

	public recalcularTempoTotalApontadoNaoSincronizadoChannel(): void {
		this.tempoTotalApontadoNaoSincronizadoChannel = 0;

		this.tarefas?.forEach(tarefa => {
			tarefa.recalcularTempoTotalApontadoNaoSincronizadoChannel(this.usuarioReferencia, this.dataReferencia);

			this.tempoTotalApontadoNaoSincronizadoChannel += tarefa.obterTempoApontadoPorData(this.usuarioReferencia, false, this.dataReferencia);
		});
	}

	public removerTarefasSemApontamentos(): void {
		this.tarefas = this.tarefas.filter(c => c.apontamentos.length > 0 
												&& c.apontamentos.some(a => a.usuario == this.usuarioReferencia && a.data.getTime() == this.dataReferencia.getTime()));
	}
}