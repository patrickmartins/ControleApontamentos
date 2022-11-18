import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormModel } from 'src/app/common/models/form.model';
import 'moment/locale/pt-br';
import * as moment from 'moment';

import { ValidatorWrapper } from 'src/app/common/validators/validator-wrapper';
import { IModel } from 'src/app/common/models/model';
import { ValidatorTimeMin } from 'src/app/common/validators/validator-time-min';

export class NovoApontamento extends FormModel<NovoApontamento> implements IModel<NovoApontamento> {
	
	public idTarefa: number = 0;
	public comentario: string = "";
	public colecao: string = "";
	public tempoTotal: number = 0;

	public toForm(): FormGroup<any> {
		return new FormBuilder().group({
			idTarefa: [this.idTarefa, [ValidatorWrapper.isValid(Validators.required, "O id da tarefa é obrigatório")]],
			tempoTotal: [this.tempoTotal, [ValidatorWrapper.isValid(Validators.required, "O tempo total é obrigatório"),
										   ValidatorWrapper.isValid(ValidatorTimeMin.min(1), "O tempo total é obrigatório")]],
			comentario: [this.comentario, [ValidatorWrapper.isValid(Validators.required, "O comentário é obrigatório")]],
			colecao: [this.colecao, [ValidatorWrapper.isValid(Validators.required, "O nome da coleção é obrigatório")]]
		});
	}

	public toModel(form: FormGroup<any>): NovoApontamento {
		let model = new NovoApontamento();

		model.idTarefa = form.contains('idTarefa') ? form.controls['idTarefa'].value : '';
		model.tempoTotal = form.contains('tempoTotal') ? this.stringParaMinutos(form.controls['tempoTotal'].value) : 0;
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
			apontamento.tempoTotal = typeof params.tempoTotal == 'number' ? params.tempoTotal : moment(params.tempoTotal, 'hh:mm').minutes();
		}

		return apontamento;
	}

	public formatarTempoTotal(format: string): string {
		return moment.utc(this.tempoTotal*1000).format(format);
	}

	private stringParaMinutos(valor: any): number {
		const tempo = moment.duration(valor);

		return tempo.minutes() + (tempo.hours() * 60);
	}
}