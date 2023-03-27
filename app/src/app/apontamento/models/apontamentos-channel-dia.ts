import * as moment from "moment";
import { IModel } from "src/app/common/models/model";
import { ApontamentoChannel } from "src/app/core/models/apontamento-channel";

import { Atividade } from "../../core/models/atividade";

export class ApontamentosChannelDia implements IModel<ApontamentosChannelDia> {

	public dataReferencia: Date = new Date();
	public tempoTotalApontadoNoDia: number = 0;
	public atividades: Atividade[] = [];

	public criarNovo(params: any): ApontamentosChannelDia | undefined {
		if (!params)
			return undefined;

		let apontamento = new ApontamentosChannelDia();

		if (params) {
			apontamento.dataReferencia = moment(params.dataReferencia).toDate();
			apontamento.tempoTotalApontadoNoDia = params.tempoTotalApontadoNoDia;

			apontamento.atividades = Array.isArray(params.atividades) ? Array.from(params.atividades).map(item => new Atividade().criarNovo(item)!) : [];
		}

		return apontamento;
	}

	public obterApontamentosTfs(): ApontamentoChannel[] {		
		let apontamentos: ApontamentoChannel[] = [];

		this.atividades.forEach(atividade => {			
			atividade.obterApontamentosTfs().forEach(tarefa => {
				apontamentos.push(tarefa);
			});
		});

		return apontamentos;
	}

	public recalcularTempoTotalApontado(): void {
		this.tempoTotalApontadoNoDia = 0;

		this.atividades?.forEach(atividade => {
			atividade.recalcularTempoTotalApontado(this.dataReferencia);

			this.tempoTotalApontadoNoDia += atividade.obterTempoApontadoPorData(this.dataReferencia);
		});
	}

	public removerApontamentosExcluidos(): void {
		this.atividades.forEach(atividade => {			
			atividade.removerApontamentosExcluidos();
		});
	}

	public removerTarefasSemApontamentos(): void {
		this.atividades = this.atividades.filter(c => c.apontamentos.length > 0 
												&& c.apontamentos.some(a => a.data.getTime() == this.dataReferencia.getTime()));
	}
}