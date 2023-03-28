import { Component, Input } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BaseComponent } from 'src/app/common/components/base.component';

import { ContaService } from 'src/app/core/services/conta.service';
import { Atividade } from '../../models/atividade';

@Component({
	selector: 'quadro-atividade',
	templateUrl: './quadro-atividade.component.html',
	styleUrls: ['./quadro-atividade.component.scss']
})

export class QuadroAtividadeComponent extends BaseComponent {

	@Input()
	public atividade!: Atividade;

	public apontamentosExpandido: boolean = false;
	
	constructor(servicoConta: ContaService, snackBar: MatSnackBar) {
		super(servicoConta, snackBar);
	}
}