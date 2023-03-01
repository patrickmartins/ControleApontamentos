import * as moment from "moment";
import 'moment/locale/pt-br';

import { IModel } from "src/app/common/models/model";
import { StatusApontamento } from "./status-apontamento";

export class ApontamentoChannel implements IModel<ApontamentoChannel> {

	public hash: string = "";	
	public idTarefaTfs: number = 0;
	public usuario: string = "";
	public comentario: string = "";
	public data: Date = new Date();
	public tempo: number = 0;
	public apontamentoTfs: boolean = false;
	public status: StatusApontamento = StatusApontamento.Inserido;

	constructor() { }

	public criarNovo(params: any): ApontamentoChannel | undefined {
		if(!params)
			return undefined;

		let apontamento = new ApontamentoChannel();

		if(params) {
			apontamento.hash = params.hash;
			apontamento.idTarefaTfs = params.idTarefaTfs;
			apontamento.usuario = params.usuario;
			apontamento.comentario = params.comentario;
			apontamento.data = moment(params.data).toDate();	
			apontamento.tempo = params.tempo;
			apontamento.apontamentoTfs = params.apontamentoTfs;
			apontamento.status = params.status as StatusApontamento;		
		}

		return apontamento;
	}
}