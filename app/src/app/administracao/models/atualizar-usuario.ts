import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormModel } from 'src/app/common/models/form.model';
import 'moment/locale/pt-br';

import { ValidatorWrapper } from 'src/app/common/validators/validator-wrapper';
import { IModel } from 'src/app/common/models/model';
import { Funcionario } from 'src/app/core/models/funcionario';
import { UsuarioChannel } from 'src/app/core/models/usuarioChannel';
import { UsuarioTfs } from 'src/app/core/models/usuarioTfs';

export class AtualizarUsuario extends FormModel<AtualizarUsuario> implements IModel<AtualizarUsuario> {
	
    public idUsuarioTfs?: string;
    public idUsuarioChannel?: number;
    public idFuncionarioPonto?: number;
    public ehAdministrador?: boolean;

    public idUsuario: string = "";
    public nomeCompleto: string = "";

    private _usuarioTfs?: UsuarioTfs;
    public get usuarioTfs(): UsuarioTfs | undefined {
        return this._usuarioTfs;
    }

    public set usuarioTfs(usuario: UsuarioTfs | undefined) {
        this.idUsuarioTfs = usuario?.id;

        this._usuarioTfs = usuario;
    }

    private _usuarioChannel?: UsuarioChannel;
    public get usuarioChannel(): UsuarioChannel | undefined {
        return this._usuarioChannel;
    }
    
    public set usuarioChannel(usuario: UsuarioChannel | undefined) {
        this.idUsuarioChannel = usuario?.id;
        
        this._usuarioChannel = usuario;
    }

    private _funcionarioPonto?: Funcionario;
    public get funcionarioPonto(): Funcionario | undefined {
        return this._funcionarioPonto;
    }
    
    public set funcionarioPonto(funcionario: Funcionario | undefined) {
        this.idFuncionarioPonto = funcionario?.id;
        
        this._funcionarioPonto = funcionario;
    }

	public toForm(): FormGroup<any> {
		return new FormBuilder().group({
			idUsuario: [this.idUsuario, [ValidatorWrapper.isValid(Validators.required, "O id do usuário é obrigatório")]],
			usuarioTfs: [this.idUsuarioTfs, []],
			usuarioChannel: [this.idUsuarioChannel, []],
			funcionarioPonto: [this.idFuncionarioPonto, []],
            ehAdministrador: [this.ehAdministrador, []]
		});
	}

	public toModel(form: FormGroup<any>): AtualizarUsuario {
		let model = new AtualizarUsuario();

		model.idUsuario = form.contains('idUsuario') ? form.controls['idUsuario'].value : '';
		model.usuarioTfs = form.contains('usuarioTfs') ? form.controls['usuarioTfs'].value : '';
		model.usuarioChannel = form.contains('usuarioChannel') ? form.controls['usuarioChannel'].value : undefined;
		model.funcionarioPonto = form.contains('funcionarioPonto') ? form.controls['funcionarioPonto'].value : undefined;
        model.ehAdministrador = form.contains('ehAdministrador') ? form.controls['ehAdministrador'].value : undefined;

		return model;
	}

	public criarNovo(params: any): AtualizarUsuario | undefined {
		if (!params)
			return undefined;

		let usuario = new AtualizarUsuario();

		if (params) {
			usuario.idUsuario = params.idUsuario;
			usuario.usuarioTfs = params.usuarioTfs;
			usuario.usuarioChannel = params.usuarioChannel;
			usuario.funcionarioPonto = params.funcionarioPonto;
            usuario.nomeCompleto = params.nomeCompleto;
            usuario.ehAdministrador = params.ehAdministrador;
		}

		return usuario;
	}
}