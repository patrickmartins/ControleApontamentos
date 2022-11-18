import * as moment from "moment";
import { IModel } from "src/app/common/models/model";
import { environment } from "src/environments/environment";
import { Apontamento } from "./apontamento";
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
	public designado: string = "";
	public tags: string[] = [];	
	public fixada: boolean = false;
	public apontamentoHabilitado: boolean = false;		
	public tempoTotalApontadoSincronizadoChannel: number = 0;
	public tempoTotalApontadoNaoSincronizadoChannel: number = 0;
	public apontamentos: Apontamento[] = [];
	
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
			tarefa.designado = params.designado;
			tarefa.tags = params.tags;			
			tarefa.dataCriacao = moment(params.dataCriacao).toDate();
			tarefa.tempoTotalApontadoSincronizadoChannel = params.tempoTotalApontadoSincronizadoChannel;
			tarefa.tempoTotalApontadoNaoSincronizadoChannel = params.tempoTotalApontadoNaoSincronizadoChannel;			
			tarefa.apontamentoHabilitado = params.apontamentoHabilitado && (params.status != StatusTarefa.fechado && params.status != StatusTarefa.desconhecido);
			
			tarefa.apontamentos = Array.isArray(params.apontamentos) ? Array.from(params.apontamentos).map(item => new Apontamento().criarNovo(item)!) : [];
		}	

		return tarefa;
	}

	public adicionarApontamento(apontamento: Apontamento): void {
		this.apontamentos.push(apontamento);

		this.recalcularTempoTotalApontadoSincronizadoChannel();
		this.recalcularTempoTotalApontadoNaoSincronizadoChannel();
	}

	public obterLinkTfs(): string {
		return `${environment.urlTfs}/${this.colecao}/${this.projeto}/_workitems?id=${this.id}&fullScreen=true`
	}

	public recalcularTempoTotalApontadoSincronizadoChannel(): void {
		this.tempoTotalApontadoSincronizadoChannel = this.obterTempoTotalApontado(true);
	}

	public recalcularTempoTotalApontadoNaoSincronizadoChannel(): void {
		this.tempoTotalApontadoNaoSincronizadoChannel = this.obterTempoTotalApontado(false);
	}

	public obterTempoTotalApontado(sincronizado: boolean): number {
		var tempoTotal = 0;

		this.apontamentos?.forEach(apontamento => {
			tempoTotal += apontamento.sincronizadoChannel == sincronizado ? apontamento.tempo : 0;
		});

		return tempoTotal;
	}
}