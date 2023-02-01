import * as moment from "moment";
import { IModel } from "src/app/common/models/model";
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
}