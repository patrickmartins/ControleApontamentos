import * as moment from "moment";
import { IModel } from "src/app/common/models/model";
import { ApontamentoChannel } from "src/app/core/models/apontamento-channel";

import { Atividade } from "../../core/models/atividade";
import { IColecaoApontamentosChannel } from "./colecao-apontamentos";

export class ApontamentosChannelDia implements IModel<ApontamentosChannelDia>, IColecaoApontamentosChannel {
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

    public obterTodosApontamentos(): ApontamentoChannel[] {
        let apontamentos: ApontamentoChannel[] = [];

		for (let atividade of this.atividades) {		
            apontamentos.push(...atividade.obterTodosApontamentos());
		}

		return apontamentos;
    }

	public obterApontamentosTfs(): ApontamentoChannel[] {		
		let apontamentos: ApontamentoChannel[] = [];

		for (let atividade of this.atividades) {		
            apontamentos.push(...atividade.obterApontamentosTfs());
		}

		return apontamentos;
	}

	public recalcularTempoTotalApontado(): void {
		this.tempoTotalApontadoNoDia = 0;

		for (let atividade of this.atividades) {
			atividade.recalcularTempoTotalApontado(this.dataReferencia);

			this.tempoTotalApontadoNoDia += atividade.obterTempoApontadoPorData(this.dataReferencia);
		}
	}

	public removerApontamentosExcluidos(): void {
		this.atividades.forEach(atividade => {			
			atividade.removerApontamentosExcluidos();
		});
	}

	public removerAtividadesSemApontamentos(): void {
		this.atividades = this.atividades.filter(c => c.apontamentos.length > 0 
												&& c.apontamentos.some(a => a.data.getTime() == this.dataReferencia.getTime()));
	}

    public removerApontamentoPorHash(hash: string): boolean {
        for (let atividade of this.atividades) {			
			if(atividade.removerApontamentoPorHash(hash))
                return true;;
		}

        return false;
	}
}