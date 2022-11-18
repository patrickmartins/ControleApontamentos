import { IModel } from "src/app/common/models/model";
import { StatusTarefa } from "src/app/core/models/status-tarefa";

export class FiltroBusca implements IModel<FiltroBusca> {
	public colecao: string = "";
	public palavraChave: string = "";
	public pagina: number = 1;
	public tamanhoPagina: number = 10;
	public status: StatusTarefa[] = [];
	
	constructor() { }

	public criarNovo(params: any): FiltroBusca | undefined {
		if(!params)
			return undefined;

		let filtro = new FiltroBusca();

		if(params) {
			filtro.colecao = params.colecao;
			filtro.palavraChave = params.palavraChave;
			filtro.pagina = params.pagina ? params.pagina as number : 1;
			filtro.tamanhoPagina = params.tamanhoPagina ? params.tamanhoPagina as number : 10;

			filtro.status = Array.isArray(params.status) ? params.status.map((c: any) => c as number) : []
		}

		return filtro;
	}
	
	public removerStatus(status: StatusTarefa): void {
		this.status = this.status.filter(c => c != status);		
	}
}