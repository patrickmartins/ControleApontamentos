import { IModel } from "src/app/common/models/model";
import { BatidasPontoDia } from "./batidas-ponto-dia";

export class BatidasPontoMes implements IModel<BatidasPontoMes> {
	
	public mesReferencia: number = 0;
	public anoReferencia: number = 0;
    public diasTrabalhados: number = 0;
	public tempoTotalTrabalhadoNoMes: number = 0;

	public batidasDiarias: BatidasPontoDia[] = [];

	public criarNovo(params: any): BatidasPontoMes | undefined {
		if (!params)
			return undefined;

		let batidas = new BatidasPontoMes();

		if (params) {						
			batidas.mesReferencia = params.mesReferencia;
			batidas.anoReferencia = params.anoReferencia;
            batidas.diasTrabalhados = params.diasTrabalhados;
			batidas.tempoTotalTrabalhadoNoMes = params.tempoTotalTrabalhadoNoMes;

			batidas.batidasDiarias = Array.isArray(params.batidasDiarias) ? Array.from(params.batidasDiarias).map(item => new BatidasPontoDia().criarNovo(item)!) : [];
		}

		return batidas;
	}

	public obterBatidasPorDia(dia: number): BatidasPontoDia | undefined {
		return this.batidasDiarias.find(c => c.dataReferencia.getDate() == dia);
	}
}