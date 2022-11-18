import { IModel } from "src/app/common/models/model";

export class Login implements IModel<Login> {
    public email?: string = "";
    public password?: string = "";    

	constructor() {	}

	public criarNovo(params: any): Login | undefined {
		if(!params)
			return undefined;

		let model = new Login();

		if(params) {
			model.email = params.email;
			model.password = params.password;
		}

		return model;
	}
}