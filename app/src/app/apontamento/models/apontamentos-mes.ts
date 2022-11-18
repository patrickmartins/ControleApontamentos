import { IModel } from "src/app/common/models/model";
import { ApontamentosDia } from "./apontamentos-dia";

export class ApontamentosMes implements IModel<ApontamentosMes> {

	public mesReferencia: number = 0;
	public anoReferencia: number = 0;
	public tempoTotalApontadoNoMes: number = 0;
	public tempoTotalApontadoSincronizadoChannel: number = 0;
	public tempoTotalApontadoNaoSincronizadoChannel: number = 0;
	public apontamentosDiarios: ApontamentosDia[] = [];

	public criarNovo(params: any): ApontamentosMes | undefined {
		if (!params)
			return undefined;

		let apontamentos = new ApontamentosMes();

		if (params) {
			apontamentos.mesReferencia = params.mesReferencia;
			apontamentos.anoReferencia = params.anoReferencia;
			apontamentos.tempoTotalApontadoNoMes = params.tempoTotalApontadoNoMes;
			apontamentos.tempoTotalApontadoSincronizadoChannel = params.tempoTotalApontadoSincronizadoChannel;
			apontamentos.tempoTotalApontadoNaoSincronizadoChannel = params.tempoTotalApontadoNaoSincronizadoChannel;

			apontamentos.apontamentosDiarios = Array.isArray(params.apontamentosDiarios) ? Array.from(params.apontamentosDiarios).map(item => new ApontamentosDia().criarNovo(item)!) : [];
		}

		return apontamentos;
	}

	public obterApontamentosPorDia(dia: number): ApontamentosDia | undefined {
		return this.apontamentosDiarios.find(c => c.dataReferencia.getDate() == dia);
	}
}