import * as moment from "moment";
import 'moment/locale/pt-br';

import { IModel } from "src/app/common/models/model";
import { environment } from "src/environments/environment";
import { StatusApontamento } from "./status-apontamento";

export class ApontamentoChannel implements IModel<ApontamentoChannel> {

	public id: number = 0;
	public idTfs: string = "";	
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
			apontamento.id = params.id as number;
			apontamento.idTfs = params.idTfs;
			apontamento.idTarefaTfs = params.idTarefaTfs as number;
			apontamento.usuario = params.usuario;
			apontamento.comentario = params.comentario;
			apontamento.data = moment(params.data).toDate();	
			apontamento.tempo = params.tempo as number;
			apontamento.apontamentoTfs = params.apontamentoTfs;
			apontamento.status = params.status as StatusApontamento;		
		}

		return apontamento;
	}

	public obterLinkApontamentoChannel(): string {
		return `${environment.urlChannel}/apontamento.do?action=editar&key=${this.id}`
	}
}