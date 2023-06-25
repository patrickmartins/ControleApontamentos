import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { FormBase } from 'src/app/common/form/form-base';
import { NovoApontamento } from '../../models/novo-apontamento';
import { TempoHelper } from 'src/app/helpers/tempo.helper';

@Component({
	selector: 'app-modal-salvar-apontamento',
	templateUrl: './modal-salvar-apontamento.component.html',
	styleUrls: ['./modal-salvar-apontamento.component.scss']
})
export class ModalSalvarApontamentoComponent {

	public form = new FormBase<NovoApontamento>(NovoApontamento);
	public dataAtual: Date = new Date();
	
	constructor(public dialogRef: MatDialogRef<ModalSalvarApontamentoComponent>, @Inject(MAT_DIALOG_DATA) private data: NovoApontamento) {
		this.form.formGroup.controls['idTarefa'].setValue(data.idTarefa);
		this.form.formGroup.controls['colecao'].setValue(data.colecao);
		this.form.formGroup.controls['data'].setValue(data.data);
		this.form.formGroup.controls['tempoTotal'].setValue(TempoHelper.minutosParaString(data.tempoTotal));
	}

	public salvarApontamento(): void {
		if (this.form.isValid()) {
			const model = this.form.getModel();

			this.dialogRef.close(model);
		}
	}
}
