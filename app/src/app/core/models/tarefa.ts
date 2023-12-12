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
			tarefa.status = typeof params.status == 'number' ? StatusTarefa[params.status] : params.status;
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

	public removerApontamentoPorIdTfs(idTfs: string): boolean {
		let index = this.apontamentos.findIndex(c => c.idTfs == idTfs);

		if(index >= 0)			
			this.apontamentos.splice(index, 1);

        return index >= 0;
	}

	public obterLinkTfs(): string {
		return `${environment.urlTfs}/${this.colecao}/${this.projeto}/_workitems?id=${this.id}&fullScreen=true`
	}

    public obterApontamentosPorUsuario(usuario: string): ApontamentoTfs[] {
        return this.apontamentos.filter(c => c.usuario == usuario);
    }

    public obterApontamentoPorIdTfs(idTfs: string): ApontamentoTfs | undefined {
        return this.apontamentos.find(c => c.idTfs == idTfs);
    }
	
	public recalcularTempoTotalApontadoSincronizadoChannel(usuario: string): void;
	public recalcularTempoTotalApontadoSincronizadoChannel(usuario: string, dataReferencia?: Date): void;
	public recalcularTempoTotalApontadoSincronizadoChannel(usuario: string, dataReferencia?: Date): void {
		this.tempoTotalApontadoSincronizadoChannel = this.obterTempoApontadoPorData(usuario, true, dataReferencia);
	}

	public recalcularTempoTotalApontadoNaoSincronizadoChannel(usuario: string): void;
	public recalcularTempoTotalApontadoNaoSincronizadoChannel(usuario: string, dataReferencia?: Date): void;
	public recalcularTempoTotalApontadoNaoSincronizadoChannel(usuario: string, dataReferencia?: Date): void {
		this.tempoTotalApontadoNaoSincronizadoChannel = this.obterTempoApontadoPorData(usuario, false, dataReferencia);
	}
	
	public obterTempoApontadoPorData(usuario: string, sincronizado: boolean): number;
	public obterTempoApontadoPorData(usuario: string, sincronizado: boolean, dataReferencia?: Date): number;
	public obterTempoApontadoPorData(usuario: string, sincronizado?: boolean, dataReferencia?: Date): number {
		let tempoTotal = 0;

		for (let apontamento of this.apontamentos) {
			tempoTotal += apontamento.usuario == usuario && (!dataReferencia || apontamento.data.toLocaleDateString() == dataReferencia.toLocaleDateString()) && apontamento.sincronizadoChannel == sincronizado ? apontamento.tempo : 0;
		}

		return tempoTotal;
	}
}