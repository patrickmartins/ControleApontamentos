import { IModel } from "src/app/common/models/model";
import { environment } from "src/environments/environment";
import { ApontamentoChannel } from "./apontamento-channel";
import { StatusApontamento } from "./status-apontamento";
import { TipoApontamentoChannel } from "./tipo-apontamento-channel";

export class Atividade implements IModel<Atividade> {

	public id: number = 0;
	public nome: string = "";	
	public codigo: string = "";	
	public idProjeto: number = 0;
	public nomeProjeto: string = "";
	public tempoTotalApontado: number = 0;
	public apontamentos: ApontamentoChannel[] = [];
	
	public tipoApontamentos: TipoApontamentoChannel = TipoApontamentoChannel.Avulso;

	constructor() { }

	public criarNovo(params: any): Atividade | undefined {
		if(!params)
			return undefined;

		let tarefa = new Atividade();

		if(params) {
			tarefa.id = params.id;
			tarefa.nome = params.nome;
			tarefa.codigo = params.codigo;
			tarefa.idProjeto = params.idProjeto;
			tarefa.nomeProjeto = params.nomeProjeto;
			tarefa.tempoTotalApontado = params.tempoTotalApontado;		
			tarefa.tipoApontamentos = params.tipoApontamentos as TipoApontamentoChannel;

			tarefa.apontamentos = Array.isArray(params.apontamentos) ? Array.from(params.apontamentos).map(item => new ApontamentoChannel().criarNovo(item)!) : [];
		}	

		return tarefa;
	}

	public obterLinkChannel(): string {
		return `${environment.urlChannel}/projeto.do?action=escopo&idProjeto=${this.idProjeto}&idAtividade=${this.id}&abaSelecionada=abaApontamentos`
	}

	public obterApontamentosTfs(): ApontamentoChannel[] {
		return this.apontamentos.filter(c => c.apontamentoTfs);
	}

	public removerApontamentosExcluidos(): void {
		this.apontamentos = this.apontamentos.filter(c => c.status != StatusApontamento.Excluido);
	}

	public recalcularTempoTotalApontado(dataReferencia: Date): void {
		this.tempoTotalApontado = this.obterTempoApontadoPorData(dataReferencia);
	}

	public obterTempoApontadoPorData(data: Date): number {
		var tempoTotal = 0;

		this.apontamentos?.forEach(apontamento => {
			tempoTotal += apontamento.data.getTime() == data.getTime() ? apontamento.tempo : 0;
		});

		return tempoTotal;
	}
}