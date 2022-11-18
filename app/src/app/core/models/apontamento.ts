import * as moment from "moment";
import 'moment/locale/pt-br';

import { IModel } from "src/app/common/models/model";

export class Apontamento implements IModel<Apontamento> {

	public usuario: string = "";
	public comentario: string = "";
	public data: Date = new Date();
	public tempo: number = 0;
	public sincronizadoChannel: boolean = false;

	constructor() { }

	public criarNovo(params: any): Apontamento | undefined {
		if(!params)
			return undefined;

		let apontamento = new Apontamento();

		if(params) {
			apontamento.usuario = params.usuario;
			apontamento.comentario = params.comentario;
			apontamento.data = moment(params.data).toDate();	
			apontamento.tempo = params.tempo;
			apontamento.sincronizadoChannel = params.sincronizadoChannel;		
		}

		return apontamento;
	}
}