import * as moment from "moment";
import { IModel } from "src/app/common/models/model";
import { StatusContador } from "./status-contador";

export class ContadorSalvo implements IModel<ContadorSalvo> {

	public statusContador: StatusContador = StatusContador.naoiniciado;	
	public tempoDecorrido: number = 0;
	public dataStatus: number = Date.now();

	public criarNovo(params: any): ContadorSalvo | undefined {
		if(!params)
			return undefined;

		let contador = new ContadorSalvo();

		if(params) {
			contador.statusContador = params.statusContador;
			contador.tempoDecorrido = params.tempoDecorrido;	
			contador.dataStatus = params.dataStatus;
		}

		return contador;
	}

}