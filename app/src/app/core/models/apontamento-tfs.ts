import * as moment from "moment";
import 'moment/locale/pt-br';

import { IModel } from "src/app/common/models/model";

export class ApontamentoTfs implements IModel<ApontamentoTfs> {

	public idTfs: string = "";
	public usuario: string = "";
	public comentario: string = "";
	public data: Date = new Date();
	public tempo: number = 0;
    public dataApropriacao?: Date;
	public sincronizadoChannel: boolean = false;

	constructor() { }

	public criarNovo(params: any): ApontamentoTfs | undefined {
		if(!params)
			return undefined;

		let apontamento = new ApontamentoTfs();

		if(params) {
			apontamento.idTfs = params.idTfs;
			apontamento.usuario = params.usuario;
			apontamento.comentario = params.comentario;
			apontamento.data = moment(params.data).toDate();	
			apontamento.tempo = params.tempo as number;            
			apontamento.sincronizadoChannel = params.sincronizadoChannel;		
            apontamento.dataApropriacao = params.dataApropriacao ? moment(params.dataApropriacao).toDate() : undefined;
		}

		return apontamento;
	}
}