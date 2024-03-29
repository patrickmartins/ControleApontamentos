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

		let atividade = new Atividade();

		if(params) {
			atividade.id = params.id as number;
			atividade.nome = params.nome;
			atividade.codigo = params.codigo;
			atividade.idProjeto = params.idProjeto as number;
			atividade.nomeProjeto = params.nomeProjeto;
			atividade.tempoTotalApontado = params.tempoTotalApontado as number;		
			atividade.tipoApontamentos = params.tipoApontamentos as TipoApontamentoChannel;

			atividade.apontamentos = Array.isArray(params.apontamentos) ? Array.from(params.apontamentos).map(item => new ApontamentoChannel().criarNovo(item)!) : [];
		}	

		return atividade;
	}

	public obterLinkApontamentosProjetoChannel(): string {
		return `${environment.urlChannel}/projeto.do?action=escopo&idProjeto=${this.idProjeto}&idAtividade=${this.id}&abaSelecionada=abaApontamentos`
	}

    public obterTodosApontamentos(): ApontamentoChannel[] {
		return this.apontamentos;
    }

	public obterApontamentosTfs(): ApontamentoChannel[] {
		return this.apontamentos.filter(c => c.apontamentoTfs);
	}

    public apontamentosTfsExiste(idApontamentoTfs: string): boolean {
        return this.apontamentos.some(c => c.apontamentoTfs && c.idTfs == idApontamentoTfs);
    }

    public removerApontamentoPorIdTfs(idApontamentoTfs: string): boolean {
		let index = this.apontamentos.findIndex(c => c.idTfs == idApontamentoTfs);

		if(index >= 0)			
			this.apontamentos.splice(index, 1);
            
        return index >= 0;
	}

	public removerApontamentosExcluidos(): void {
		this.apontamentos = this.apontamentos.filter(c => c.status != StatusApontamento.Excluido);
	}

	public recalcularTempoTotalApontado(dataReferencia: Date): void {
		this.tempoTotalApontado = this.obterTempoApontadoPorData(dataReferencia);
	}

	public obterTempoApontadoPorData(data: Date): number {
		let tempoTotal = 0;

		for (let apontamento of this.apontamentos) {
			tempoTotal += apontamento.data.getTime() == data.getTime() ? apontamento.tempo : 0;
		}

		return tempoTotal;
	}
}