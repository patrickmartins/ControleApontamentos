import { IModel } from "src/app/common/models/model";
import { Tarefa } from "src/app/core/models/tarefa";
import { ApontamentosTfsDia } from "./apontamentos-tfs-dia";
import { ApontamentoTfs } from "src/app/core/models/apontamento-tfs";
import { IColecaoApontamentosTfs } from "./colecao-apontamentos";

export class ApontamentosTfsMes implements IModel<ApontamentosTfsMes>, IColecaoApontamentosTfs {

	public usuarioReferencia: string = "";
	public mesReferencia: number = 0;
	public anoReferencia: number = 0;
    public diasApontados: number = 0;
	public tempoTotalApontadoNoMes: number = 0;
	public tempoTotalApontadoSincronizadoChannel: number = 0;
	public tempoTotalApontadoNaoSincronizadoChannel: number = 0;
	public apontamentosDiarios: ApontamentosTfsDia[] = [];

	public criarNovo(params: any): ApontamentosTfsMes | undefined {
		if (!params)
			return undefined;

		let apontamentos = new ApontamentosTfsMes();

		if (params) {
			apontamentos.usuarioReferencia = params.usuarioReferencia;
			apontamentos.mesReferencia = params.mesReferencia;
			apontamentos.anoReferencia = params.anoReferencia;
            apontamentos.diasApontados = params.diasApontados;
			apontamentos.tempoTotalApontadoNoMes = params.tempoTotalApontadoNoMes;
			apontamentos.tempoTotalApontadoSincronizadoChannel = params.tempoTotalApontadoSincronizadoChannel;
			apontamentos.tempoTotalApontadoNaoSincronizadoChannel = params.tempoTotalApontadoNaoSincronizadoChannel;

			apontamentos.apontamentosDiarios = Array.isArray(params.apontamentosDiarios) ? Array.from(params.apontamentosDiarios).map(item => new ApontamentosTfsDia().criarNovo(item)!) : [];
		}

		return apontamentos;
	}

	public obterApontamentosPorDia(dia: number): ApontamentosTfsDia | undefined {
		return this.apontamentosDiarios.find(c => c.dataReferencia.getDate() == dia);
	}

    public obterTodosApontamentos(): ApontamentoTfs[] {
        let apontamentos: ApontamentoTfs[] = [];

		for (let apontamentosDia of this.apontamentosDiarios) {
            apontamentos.push(...apontamentosDia.obterTodosApontamentos());
        }

        return apontamentos;
	}

	public obterApontamentosDoUltimoDia(): ApontamentosTfsDia | undefined {
		let diasComApontamento = this.apontamentosDiarios.filter(c => c.tempoTotalApontadoNoDia > 0);

		if (diasComApontamento.length > 0) {
			return diasComApontamento[diasComApontamento.length - 1];
		}

		return undefined;
	}
	
	public obterTarefasPorId(id: number): Tarefa[] {		
		let tarefas: Tarefa[] = [];

		for (let apontamentosDia of this.apontamentosDiarios) {			
			var tarefa = apontamentosDia.obterTarefaPorId(id);

            if(tarefa)
				tarefas.push(tarefa);			
		}

		return tarefas;
	}

	public recalcularTempoTotalApontado(): void {
		this.recalcularTempoTotalApontadoSincronizadoChannel();
		this.recalcularTempoTotalApontadoNaoSincronizadoChannel();

		this.tempoTotalApontadoNoMes = this.tempoTotalApontadoNaoSincronizadoChannel + this.tempoTotalApontadoSincronizadoChannel;
	}

	public recalcularTempoTotalApontadoSincronizadoChannel(): void {
		this.tempoTotalApontadoSincronizadoChannel = 0;

		for(let apontamentosDia of this.apontamentosDiarios) {
			apontamentosDia.recalcularTempoTotalApontado();

			this.tempoTotalApontadoSincronizadoChannel += apontamentosDia.tempoTotalApontadoSincronizadoChannel;
		}
	}

	public recalcularTempoTotalApontadoNaoSincronizadoChannel(): void {
		this.tempoTotalApontadoNaoSincronizadoChannel = 0;

		for(let apontamentosDia of this.apontamentosDiarios) {
			apontamentosDia.recalcularTempoTotalApontado();

			this.tempoTotalApontadoNaoSincronizadoChannel += apontamentosDia.tempoTotalApontadoNaoSincronizadoChannel;
		}
	}

	public removerTarefasSemApontamentos(): void {
		this.apontamentosDiarios.forEach(c => c.removerTarefasSemApontamentos());
	}

    public removerApontamentoPorIdTfs(idTfs: string): void {
        for(let apontamentosDia of this.apontamentosDiarios) {			
		    apontamentosDia.removerApontamentoPorIdTfs(idTfs)
		}
	}
}