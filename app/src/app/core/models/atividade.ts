import { IModel } from "src/app/common/models/model";
import { Apontamento } from "src/app/core/models/apontamento";
import { environment } from "src/environments/environment";

export class Atividade implements IModel<Atividade> {

	public id: number = 0;
	public nome: string = "";	
	public codigo: string = "";	
	public idProjeto: number = 0;
	public nomeProjeto: string = "";
	public tempoTotalApontado: number = 0;
	public apontamentos: Apontamento[] = [];
	
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

			tarefa.apontamentos = Array.isArray(params.apontamentos) ? Array.from(params.apontamentos).map(item => new Apontamento().criarNovo(item)!) : [];
		}	

		return tarefa;
	}

	public obterLinkChannel(): string {
		return `${environment.urlChannel}/projeto.do?action=escopo&idProjeto=${this.idProjeto}&idAtividade=${this.id}&abaSelecionada=abaApontamentos`
	}

	
}