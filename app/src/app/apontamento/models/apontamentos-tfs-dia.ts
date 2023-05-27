import * as moment from "moment";
import { IModel } from "src/app/common/models/model";
import { ApontamentoTfs } from "src/app/core/models/apontamento-tfs";
import { Tarefa } from "src/app/core/models/tarefa";
import { IColecaoApontamentosTfs } from "./colecao-apontamentos";

export class ApontamentosTfsDia implements IModel<ApontamentosTfsDia>, IColecaoApontamentosTfs {

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

    public adicionarTarefa(tarefa: Tarefa): void {
        this.tarefas.push(tarefa);
    }

	public obterTarefaPorId(id: number): Tarefa | undefined {
		return this.tarefas.find(c => c.id == id);
	}

    public obterTodosApontamentos(): ApontamentoTfs[] {
        let apontamentos: ApontamentoTfs[] = [];

		for(let tarefa of this.tarefas) {
            apontamentos.push(...tarefa.apontamentos);
        }

        return apontamentos;
	}

	public recalcularTempoTotalApontado(): void {
		this.recalcularTempoTotalApontadoSincronizadoChannel();
		this.recalcularTempoTotalApontadoNaoSincronizadoChannel();

		this.tempoTotalApontadoNoDia = this.tempoTotalApontadoNaoSincronizadoChannel + this.tempoTotalApontadoSincronizadoChannel;
	}

	public recalcularTempoTotalApontadoSincronizadoChannel(): void {
		this.tempoTotalApontadoSincronizadoChannel = 0;

		for(let tarefa of this.tarefas) {
			tarefa.recalcularTempoTotalApontadoSincronizadoChannel(this.usuarioReferencia, this.dataReferencia);

			this.tempoTotalApontadoSincronizadoChannel += tarefa.obterTempoApontadoPorData(this.usuarioReferencia, true, this.dataReferencia);
		}
	}

	public recalcularTempoTotalApontadoNaoSincronizadoChannel(): void {
		this.tempoTotalApontadoNaoSincronizadoChannel = 0;

		for(let tarefa of this.tarefas) {
			tarefa.recalcularTempoTotalApontadoNaoSincronizadoChannel(this.usuarioReferencia, this.dataReferencia);

			this.tempoTotalApontadoNaoSincronizadoChannel += tarefa.obterTempoApontadoPorData(this.usuarioReferencia, false, this.dataReferencia);
		}
	}

	public removerTarefasSemApontamentos(): void {
		this.tarefas = this.tarefas.filter(c => c.apontamentos.length > 0 
												&& c.apontamentos.some(a => a.usuario == this.usuarioReferencia && a.data.getTime() == this.dataReferencia.getTime()));
	}

    public removerApontamentoPorHash(hash: string): boolean {
        for(let tarefa of this.tarefas) {			
			if(tarefa.removerApontamentoPorHash(hash))
                return true;
		}

        return false;
	}
}