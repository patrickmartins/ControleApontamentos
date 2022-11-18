import { IModel } from "src/app/common/models/model";
import { Tarefa } from "src/app/core/models/tarefa";

export class PaginaBusca implements IModel<PaginaBusca> {
	public totalResultados: number = 0;
	public numeroPagina: number = 0;
	public tamanhoPagina: number = 0;	
	public palavraChave: string = ""
	public tarefas: Tarefa[] = []
	
	constructor() { }

	public criarNovo(params: any): PaginaBusca | undefined {
		if(!params)
			return undefined;

		let pagina = new PaginaBusca();

		if(params) {
			pagina.totalResultados = params.totalResultados;
			pagina.numeroPagina = params.numeroPagina;
			pagina.tamanhoPagina = params.tamanhoPagina;
			pagina.palavraChave = params.palavraChave;

			pagina.tarefas = Array.isArray(params.resultados) ? Array.from(params.resultados).map(item => new Tarefa().criarNovo(item)!) : [];
		}

		return pagina;
	}	
}