export interface IModel<TType> {	
	criarNovo(params: any): TType | undefined;
}


