import * as moment from "moment";
import { IModel } from "src/app/common/models/model";

export class JobInfo implements IModel<JobInfo> {

	public id: string = "";
	public ultimaExecucao?: Date = new Date();
	public proximaExecucao?: Date = new Date();
	public intervaloExecucao: string = "";

	constructor() { }

	public criarNovo(params: any): JobInfo | undefined {
		if(!params)
			return undefined;

		let job = new JobInfo();

		if(params) {	
			job.id = params.id;
			job.ultimaExecucao = params.ultimaExecucao ? moment(params.ultimaExecucao).toDate() : undefined;
			job.proximaExecucao = params.proximaExecucao ? moment(params.proximaExecucao).toDate() : undefined;
			job.intervaloExecucao = params.intervaloExecucao;
		}	

		return job;
	}
}