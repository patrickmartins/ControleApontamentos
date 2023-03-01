import * as moment from "moment";
import { IModel } from "src/app/common/models/model";
import { environment } from "src/environments/environment";
import { ApontamentoTfs } from "./apontamento-tfs";
import { Designado } from "./designado";
import { StatusTarefa } from "./status-tarefa";

export class Tarefa implements IModel<Tarefa> {

	public dataCriacao: Date = new Date();
	public tipo: number = 0;
	public id: number = 0;
	public titulo: string = "";
	public tituloPai: string = "";
	public status: string = "";
	public colecao: string = "";
	public projeto: string = "";
	public designado: Designado = new Designado();
	public tags: string[] = [];	
	public fixada: boolean = false;
	public apontamentoHabilitado: boolean = false;		
	public tempoTotalApontadoSincronizadoChannel: number = 0;
	public tempoTotalApontadoNaoSincronizadoChannel: number = 0;
	public apontamentos: ApontamentoTfs[] = [];
	
	constructor() { }

	public criarNovo(params: any): Tarefa | undefined {
		if(!params)
			return undefined;

		let tarefa = new Tarefa();

		if(params) {
			tarefa.id = params.id;
			tarefa.tipo = params.tipo;
			tarefa.titulo = params.titulo;	
			tarefa.tituloPai = params.tituloPai;
			tarefa.status = StatusTarefa[params.status];
			tarefa.colecao = params.colecao;
			tarefa.projeto = params.projeto;			
			tarefa.tags = params.tags;			
			tarefa.dataCriacao = moment(params.dataCriacao).toDate();
			tarefa.tempoTotalApontadoSincronizadoChannel = params.tempoTotalApontadoSincronizadoChannel;
			tarefa.tempoTotalApontadoNaoSincronizadoChannel = params.tempoTotalApontadoNaoSincronizadoChannel;			
			tarefa.apontamentoHabilitado = params.apontamentoHabilitado && (params.status != StatusTarefa.fechado && params.status != StatusTarefa.desconhecido);
			tarefa.designado = (new Designado()).criarNovo(params.designado)!;

			tarefa.apontamentos = Array.isArray(params.apontamentos) ? Array.from(params.apontamentos).map(item => new ApontamentoTfs().criarNovo(item)!) : [];
		}	

		return tarefa;
	}

	public adicionarApontamento(apontamento: ApontamentoTfs): void {
		this.apontamentos.push(apontamento);
	}

	public removerApontamentoPorHash(hash: string): void {
		var index = this.apontamentos.findIndex(c => c.hash == hash);

		if(index >= 0)			
			this.apontamentos.splice(index, 1);
	}

	public obterLinkTfs(): string {
		return `${environment.urlTfs}/${this.colecao}/${this.projeto}/_workitems?id=${this.id}&fullScreen=true`
	}

	public recalcularTempoTotalApontadoSincronizadoChannel(dataReferencia: Date, usuario: string): void {
		this.tempoTotalApontadoSincronizadoChannel = this.obterTempoApontadoPorData(dataReferencia, usuario, true);
	}

	public recalcularTempoTotalApontadoNaoSincronizadoChannel(dataReferencia: Date, usuario: string): void {
		this.tempoTotalApontadoNaoSincronizadoChannel = this.obterTempoApontadoPorData(dataReferencia, usuario, false);
	}

	public obterTempoApontadoPorData(data: Date, usuario: string, sincronizado: boolean): number {
		var tempoTotal = 0;

		this.apontamentos?.forEach(apontamento => {
			tempoTotal += apontamento.usuario == usuario && apontamento.data.getTime() == data.getTime() && apontamento.sincronizadoChannel == sincronizado ? apontamento.tempo : 0;
		});

		return tempoTotal;
	}
}