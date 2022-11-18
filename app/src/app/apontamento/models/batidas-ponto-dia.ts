import * as moment from "moment";
import { IModel } from "src/app/common/models/model";
import 'moment-duration-format';

export class BatidasPontoDia implements IModel<BatidasPontoDia> {
	
	public entrada1Ext: string = "";
	public saida1Ext: string = "";
	public entrada2Ext: string = "";
	public saida2Ext: string = "";
	public entrada3Ext: string = "";
	public saida3Ext: string = "";
	public entrada4Ext: string = "";
	public saida4Ext: string = "";
	public entrada5Ext: string = "";
	public saida5Ext: string = "";
	public entrada6Ext: string = "";
	public saida6Ext: string = "";

	public entrada1: number = 0;
	public saida1: number = 0;
	public entrada2: number = 0;
	public saida2: number = 0;
	public entrada3: number = 0;
	public saida3: number = 0;
	public entrada4: number = 0;
	public saida4: number = 0;
	public entrada5: number = 0;
	public saida5: number = 0;
	public entrada6: number = 0;
	public saida6: number = 0;
	public dataReferencia: Date = new Date();
	public tempoTotalTrabalhadoNoDia: number = 0;

	public criarNovo(params: any): BatidasPontoDia | undefined {
		if (!params)
			return undefined;

		let batida = new BatidasPontoDia();

		if (params) {			
			batida.entrada1 = params.entrada1;
			batida.saida1 = params.saida1;
			batida.entrada2 = params.entrada2;
			batida.saida2 = params.saida2;
			batida.entrada3 = params.entrada3;
			batida.saida3 = params.saida3;
			batida.entrada4 = params.entrada4;
			batida.saida4 = params.saida4;
			batida.entrada5 = params.entrada5;
			batida.saida5 = params.saida5;
			batida.entrada6 = params.entrada6;
			batida.saida6 = params.saida6;	

			batida.entrada1Ext = moment.duration(batida.entrada1, "minutes").format("hh:mm"),
			batida.saida1Ext = moment.duration(batida.saida1, "minutes").format("hh:mm"),
			batida.entrada2Ext = moment.duration(batida.entrada2, "minutes").format("hh:mm"),
			batida.saida2Ext = moment.duration(batida.saida2, "minutes").format("hh:mm"),
			batida.entrada3Ext = moment.duration(batida.entrada3, "minutes").format("hh:mm"),
			batida.saida3Ext = moment.duration(batida.saida3, "minutes").format("hh:mm"),
			batida.entrada4Ext = moment.duration(batida.entrada4, "minutes").format("hh:mm"),
			batida.saida4Ext = moment.duration(batida.saida4, "minutes").format("hh:mm"),
			batida.entrada5Ext = moment.duration(batida.entrada5, "minutes").format("hh:mm"),
			batida.saida5Ext = moment.duration(batida.saida5, "minutes").format("hh:mm"),
			batida.entrada6Ext = moment.duration(batida.entrada6, "minutes").format("hh:mm"),
			batida.saida6Ext = moment.duration(batida.saida6, "minutes").format("hh:mm");

			batida.dataReferencia = moment(params.dataReferencia).toDate();		

			batida.tempoTotalTrabalhadoNoDia = params.tempoTotalTrabalhadoNoDia;
		}

		return batida;
	}

	public toString(): string {
		return (this.entrada1Ext !== "00" ? `Entrada e Saída 1: ${this.entrada1Ext} - ${this.saida1Ext !== "00" ? this.saida1Ext : "não registrou"}` : "") +
				(this.entrada2Ext !== "00" ? `\nEntrada e Saída 2: ${this.entrada2Ext} - ${this.saida2Ext !== "00" ? this.saida2Ext : "não registrou"}` : "") +
				(this.entrada3Ext !== "00" ? `\nEntrada e Saída 3: ${this.entrada3Ext} - ${this.saida3Ext !== "00" ? this.saida3Ext : "não registrou"}` : "") +
				(this.entrada4Ext !== "00" ? `\nEntrada e Saída 4: ${this.entrada4Ext} - ${this.saida4Ext !== "00" ? this.saida4Ext : "não registrou"}` : "") +
				(this.entrada5Ext !== "00" ? `\nEntrada e Saída 5: ${this.entrada5Ext} - ${this.saida5Ext !== "00" ? this.saida5Ext : "não registrou"}` : "") +
				(this.entrada6Ext !== "00" ? `\nEntrada e Saída 6: ${this.entrada6Ext} - ${this.saida6Ext !== "00" ? this.saida6Ext : "não registrou"}` : "");
	}
}