import { FormGroup, FormBuilder, Validators } from "@angular/forms";

import { FormModel } from 'src/app/common/models/form.model';
import { IModel } from "src/app/common/models/model";
import { ValidatorWrapper } from "src/app/common/validators/validator-wrapper";
import { TipoOrdenacaoRelatorio } from "./tipo-ordenacao-relatorio";

export class FiltroRelatorio extends FormModel<FiltroRelatorio> implements IModel<FiltroRelatorio> {

	public mes: number = 0;
	public ano: number = 0;
    public ordenacao: TipoOrdenacaoRelatorio = TipoOrdenacaoRelatorio.NomeUsuario;
    public somenteApontamentosAteDiaAnterior: boolean = false;
    public somenteApontamentosSincronizados: boolean = false;
    public somenteUsuariosComCadastroNoPonto: boolean = false;
    public somenteUsuariosComTempoTrabalhado: boolean = false;

    public toForm(): FormGroup<any> {
		return new FormBuilder().group({
			mes: [this.mes, [ValidatorWrapper.isValid(Validators.required, "O mês é obrigatório")]],
			ano: [this.ano, [ValidatorWrapper.isValid(Validators.required, "O ano obrigatório")]],
            ordenacao: [this.ordenacao],            
            somenteSincronizados: [this.somenteApontamentosSincronizados],
            somenteUsuariosPonto: [this.somenteUsuariosComCadastroNoPonto],
            somenteComTempoTrabalhado: [this.somenteUsuariosComTempoTrabalhado],
            somenteAteDiaAnterior: [this.somenteApontamentosAteDiaAnterior]
		});
	}

	public toModel(form: FormGroup<any>): FiltroRelatorio {
		let model = new FiltroRelatorio();

		model.mes = form.contains('mes') ? form.controls['mes'].value : new Date().getDate();
		model.ano = form.contains('ano') ? form.controls['ano'].value : new Date().getFullYear();
        model.ordenacao = form.contains('ordenacao') ? form.controls['ordenacao'].value : false;
        model.somenteApontamentosAteDiaAnterior = form.contains('somenteAteDiaAnterior') ? form.controls['somenteAteDiaAnterior'].value : false;
		model.somenteApontamentosSincronizados = form.contains('somenteSincronizados') ? form.controls['somenteSincronizados'].value : false;
		model.somenteUsuariosComCadastroNoPonto = form.contains('somenteUsuariosPonto') ? form.controls['somenteUsuariosPonto'].value : false;
		model.somenteUsuariosComTempoTrabalhado = form.contains('somenteComTempoTrabalhado') ? form.controls['somenteComTempoTrabalhado'].value : false;		

		return model;
	}
    
	public criarNovo(params: any): FiltroRelatorio | undefined {
		if(!params)
			return undefined;

		let filtro = new FiltroRelatorio();

		if(params) {
			filtro.mes = params.mes as number;
			filtro.ano = params.ano as number;
            filtro.somenteApontamentosAteDiaAnterior = params.somenteApontamentosAteDiaAnterior;
			filtro.somenteApontamentosSincronizados = params.somenteApontamentosSincronizados;
			filtro.somenteUsuariosComCadastroNoPonto = params.somenteUsuariosComCadastroNoPonto;
            filtro.somenteUsuariosComTempoTrabalhado = params.somenteUsuariosComTempoTrabalhado;
            filtro.ordenacao = params.ordenacao as TipoOrdenacaoRelatorio;
		}

		return filtro;
	}


}