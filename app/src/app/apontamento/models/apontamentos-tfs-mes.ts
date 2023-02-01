import { IModel } from "src/app/common/models/model";
import { ApontamentosTfsDia } from "./apontamentos-tfs-dia";

export class ApontamentosTfsMes implements IModel<ApontamentosTfsMes> {

	public mesReferencia: number = 0;
	public anoReferencia: number = 0;
	public tempoTotalApontadoNoMes: number = 0;
	public tempoTotalApontadoSincronizadoChannel: number = 0;
	public tempoTotalApontadoNaoSincronizadoChannel: number = 0;
	public apontamentosDiarios: ApontamentosTfsDia[] = [];

	public criarNovo(params: any): ApontamentosTfsMes | undefined {
		if (!params)
			return undefined;

		let apontamentos = new ApontamentosTfsMes();

		if (params) {
			apontamentos.mesReferencia = params.mesReferencia;
			apontamentos.anoReferencia = params.anoReferencia;
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

	public obterApontamentosDoUltimoDia(): ApontamentosTfsDia | undefined {
		let diasComApontamento = this.apontamentosDiarios.filter(c => c.tempoTotalApontadoNoDia > 0);

		if (diasComApontamento.length > 0) {
			return diasComApontamento[diasComApontamento.length - 1];
		}

		return undefined;
	}
}