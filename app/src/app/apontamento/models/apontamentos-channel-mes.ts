import { IModel } from "src/app/common/models/model";
import { ApontamentoChannel } from "src/app/core/models/apontamento-channel";
import { ApontamentosChannelDia } from "./apontamentos-channel-dia";

export class ApontamentosChannelMes implements IModel<ApontamentosChannelMes> {

	public mesReferencia: number = 0;
	public anoReferencia: number = 0;
    public diasApontados: number = 0;
	public tempoTotalApontadoNoMes: number = 0;
	public apontamentosDiarios: ApontamentosChannelDia[] = [];

	public criarNovo(params: any): ApontamentosChannelMes | undefined {
		if (!params)
			return undefined;

		let apontamentos = new ApontamentosChannelMes();

		if (params) {
			apontamentos.mesReferencia = params.mesReferencia;
			apontamentos.anoReferencia = params.anoReferencia;
            apontamentos.diasApontados = params.diasApontados;
			apontamentos.tempoTotalApontadoNoMes = params.tempoTotalApontadoNoMes;

			apontamentos.apontamentosDiarios = Array.isArray(params.apontamentosDiarios) ? Array.from(params.apontamentosDiarios).map(item => new ApontamentosChannelDia().criarNovo(item)!) : [];
		}

		return apontamentos;
	}

	public obterApontamentosPorDia(dia: number): ApontamentosChannelDia | undefined {
		return this.apontamentosDiarios.find(c => c.dataReferencia.getDate() == dia);
	}

	public obterApontamentosDoUltimoDia(): ApontamentosChannelDia | undefined {
		let diasComApontamento = this.apontamentosDiarios.filter(c => c.tempoTotalApontadoNoDia > 0);

		if (diasComApontamento.length > 0) {
			return diasComApontamento[diasComApontamento.length - 1];
		}

		return undefined;
	}

	public obterApontamentosTfs(): ApontamentoChannel[] {		
		let apontamentos: ApontamentoChannel[] = [];

		this.apontamentosDiarios.forEach(apontamentosDia => {			
			apontamentosDia.obterApontamentosTfs().forEach(apontamento => {
				apontamentos.push(apontamento);
			});
		});

		return apontamentos;
	}

	public removerApontamentosExcluidos(): void {
		this.apontamentosDiarios.forEach(apontamentosDia => {			
			apontamentosDia.removerApontamentosExcluidos();
		});
	}

	public recalcularTempoTotalApontado(): void {
		this.tempoTotalApontadoNoMes = 0;

		this.apontamentosDiarios?.forEach(apontamentoDia => {
			apontamentoDia.recalcularTempoTotalApontado();

			this.tempoTotalApontadoNoMes += apontamentoDia.tempoTotalApontadoNoDia;
		});
	}

	public removerTarefasSemApontamentos(): void {
		this.apontamentosDiarios.forEach(c => c.removerTarefasSemApontamentos());
	}
}