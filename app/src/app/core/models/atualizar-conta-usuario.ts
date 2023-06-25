import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormModel } from 'src/app/common/models/form.model';
import 'moment/locale/pt-br';

import { ValidatorWrapper } from 'src/app/common/validators/validator-wrapper';
import { IModel } from 'src/app/common/models/model';
import { Funcionario } from 'src/app/core/models/funcionario';
import { UsuarioChannel } from 'src/app/core/models/usuarioChannel';
import { UsuarioTfs } from 'src/app/core/models/usuarioTfs';
import { Unidade } from './unidade';
import { Usuario } from './usuario';

export class AtualizarContaUsuario extends FormModel<AtualizarContaUsuario> implements IModel<AtualizarContaUsuario> {
	
    public idUsuarioTfs?: string;
    public idUsuarioChannel?: number;
    public idFuncionarioPonto?: number;
    public idUnidade?: string;
    public idGerente?: string;
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

    private _unidade?: Unidade;
    public get unidade(): Unidade | undefined {
        return this._unidade;
    }
    
    public set unidade(unidade: Unidade | undefined) {
        this.idUnidade = unidade?.id;
        
        this._unidade = unidade;
    }

    private _gerente?: Usuario;
    public get gerente(): Usuario | undefined {
        return this._gerente;
    }
    
    public set gerente(gerente: Usuario | undefined) {
        this.idGerente = gerente?.id;
        
        this._gerente = gerente;
    }

	public toForm(): FormGroup<any> {
		return new FormBuilder().group({
			idUsuario: [this.idUsuario, [ValidatorWrapper.isValid(Validators.required, "O id do usuário é obrigatório")]],
			usuarioTfs: [this.idUsuarioTfs, []],
			usuarioChannel: [this.idUsuarioChannel, []],
			funcionarioPonto: [this.idFuncionarioPonto, []],
            unidade: [this.idUnidade, []],
            gerente: [this.idGerente, []],
            ehAdministrador: [this.ehAdministrador, []]
		});
	}

	public toModel(form: FormGroup<any>): AtualizarContaUsuario {
		let model = new AtualizarContaUsuario();

		model.idUsuario = form.contains('idUsuario') ? form.controls['idUsuario'].value : '';
		model.usuarioTfs = form.contains('usuarioTfs') ? form.controls['usuarioTfs'].value : '';
		model.usuarioChannel = form.contains('usuarioChannel') ? form.controls['usuarioChannel'].value : undefined;
		model.funcionarioPonto = form.contains('funcionarioPonto') ? form.controls['funcionarioPonto'].value : undefined;
        model.unidade = form.contains('unidade') ? form.controls['unidade'].value : undefined;
        model.gerente = form.contains('gerente') ? form.controls['gerente'].value : undefined;
        model.ehAdministrador = form.contains('ehAdministrador') ? form.controls['ehAdministrador'].value : undefined;

		return model;
	}

	public criarNovo(params: any): AtualizarContaUsuario | undefined {
		if (!params)
			return undefined;

		let usuario = new AtualizarContaUsuario();

		if (params) {
			usuario.idUsuario = params.idUsuario;
			usuario.usuarioTfs = params.usuarioTfs;
			usuario.usuarioChannel = params.usuarioChannel;
			usuario.funcionarioPonto = params.funcionarioPonto;
            usuario.unidade = params.unidade;
            usuario.gerente = params.gerente;
            usuario.nomeCompleto = params.nomeCompleto;
            usuario.ehAdministrador = params.ehAdministrador;
		}

		return usuario;
	}
}