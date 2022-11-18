import { IModel } from "../common/models/model";

export class LocalStorageHelper {

    public static salvarDados(key: string, data: any) {
        localStorage.setItem(key, JSON.stringify(data));
    }

    public static obterDados<TType>(key: string, tipo: (new () => IModel<any>)): TType | undefined {
        let data = localStorage.getItem(key);
		
		if(data === null)
			return;

		let obj = JSON.parse(data);

		if(!tipo)
			return obj;

		let entity = new tipo();

		if(Array.isArray(obj)) {			
			let array = new Array<TType>();
			
			obj.forEach(item => array.push(entity.criarNovo(item)));

			return array as any;
		}		

		if (tipo) {
			return entity.criarNovo(obj);
		}

        return obj; 
    }

    public static removerDados(key: string) {
        localStorage.removeItem(key);
    }

    public static dadoExiste(key: string): boolean {
        return localStorage.hasOwnProperty(key);
    }
}