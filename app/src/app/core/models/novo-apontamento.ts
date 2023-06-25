import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import 'moment/locale/pt-br';
import * as moment from 'moment';

import { FormModel } from 'src/app/common/models/form.model';
import { ValidatorWrapper } from 'src/app/common/validators/validator-wrapper';
import { IModel } from 'src/app/common/models/model';
import { ValidatorTimeMin } from 'src/app/common/validators/validator-time-min';
import { TempoHelper } from 'src/app/helpers/tempo.helper';

export class NovoApontamento extends FormModel<NovoApontamento> implements IModel<NovoApontamento> {
	
	public idTarefa: number = 0;
	public comentario: string = "";
	public colecao: string = "";
	public data: Date = new Date();
	public tempoTotal: number = 0;

	public toForm(): FormGroup<any> {
		return new FormBuilder().group({
			idTarefa: [this.idTarefa, [ValidatorWrapper.isValid(Validators.required, "O id da tarefa é obrigatório")]],
			data: [this.data, [ValidatorWrapper.isValid(Validators.required, "A data é obrigatória")]],
			tempoTotal: [this.tempoTotal, [ValidatorWrapper.isValid(Validators.required, "O tempo total é obrigatório"),
										   ValidatorWrapper.isValid(ValidatorTimeMin.min(1), "O tempo total é obrigatório")]],
			comentario: [this.comentario, [ValidatorWrapper.isValid(Validators.required, "O comentário é obrigatório")]],
			colecao: [this.colecao, [ValidatorWrapper.isValid(Validators.required, "O nome da coleção é obrigatório")]]
		});
	}

	public toModel(form: FormGroup<any>): NovoApontamento {
		let model = new NovoApontamento();

		model.idTarefa = form.contains('idTarefa') ? form.controls['idTarefa'].value : '';
		model.data = form.contains('data') ? moment(form.controls['data'].value).toDate() : new Date();
		model.tempoTotal = form.contains('tempoTotal') ? TempoHelper.stringParaMinutos(form.controls['tempoTotal'].value) : 0;
		model.comentario = form.contains('comentario') ? form.controls['comentario'].value : '';
		model.colecao = form.contains('colecao') ? form.controls['colecao'].value : '';		

		return model;
	}

	public criarNovo(params: any): NovoApontamento | undefined {
		if (!params)
			return undefined;

		let apontamento = new NovoApontamento();

		if (params) {
			apontamento.idTarefa = params.idTarefa;
			apontamento.comentario = params.comentario;
			apontamento.colecao = params.colecao;
			apontamento.data = moment(params.data).toDate();;
			apontamento.tempoTotal = typeof params.tempoTotal == 'number' ? params.tempoTotal : moment(params.tempoTotal, 'hh:mm').minutes();
		}

		return apontamento;
	}
}