import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBase } from 'src/app/common/form/form-base';
import { ModalSalvarApontamentoComponent } from 'src/app/core/components/modal-salvar-apontamento/modal-salvar-apontamento.component';
import { AtualizarContaUsuario } from '../../../core/models/atualizar-conta-usuario';
import { Funcionario } from 'src/app/core/models/funcionario';
import { UsuarioChannel } from 'src/app/core/models/usuarioChannel';
import { UsuarioTfs } from 'src/app/core/models/usuarioTfs';
import { Usuario } from 'src/app/core/models/usuario';
import { Unidade } from 'src/app/core/models/unidade';

@Component({
  selector: 'app-modal-salvar-usuario',
  templateUrl: './modal-salvar-usuario.component.html',
  styleUrls: ['./modal-salvar-usuario.component.scss']
})
export class ModalSalvarUsuarioComponent implements OnInit {

	public form = new FormBase<AtualizarContaUsuario>(AtualizarContaUsuario);

    public usuario!: Usuario;
    
    public usuariosTfs: UsuarioTfs[] = [];
    public usuariosChannel: UsuarioChannel[] = [];
    public funcionarios: Funcionario[] = [];
    public unidades: Unidade[] = [];
    public gerentes: Usuario[] = [];
    
    public usuariosTfsFiltrado: UsuarioTfs[] = [];
    public usuariosChannelFiltrado: UsuarioChannel[] = [];
    public funcionariosFiltrado: Funcionario[] = [];
    public unidadesFiltrada: Unidade[] = [];
    public gerentesFiltrado: Usuario[] = [];

	constructor(public dialogRef: MatDialogRef<ModalSalvarApontamentoComponent>, @Inject(MAT_DIALOG_DATA) private data: any) {
		this.form.formGroup.controls['idUsuario'].setValue(data.usuario.idUsuario);
		this.form.formGroup.controls['usuarioTfs'].setValue(data.usuario.usuarioTfs);
		this.form.formGroup.controls['usuarioChannel'].setValue(data.usuario.usuarioChannel);
		this.form.formGroup.controls['funcionarioPonto'].setValue(data.usuario.funcionarioPonto);
        this.form.formGroup.controls['unidade'].setValue(data.usuario.unidade);
        this.form.formGroup.controls['gerente'].setValue(data.usuario.gerente);
        this.form.formGroup.controls['ehAdministrador'].setValue(data.usuario.ehAdministrador);

        this.usuario = this.data.usuario;
        this.usuariosTfs = this.usuariosTfsFiltrado = this.data.usuariosTfs;
        this.usuariosChannel = this.usuariosChannelFiltrado = this.data.usuariosChannel;
        this.funcionarios = this.funcionariosFiltrado = this.data.funcionarios;
        this.unidades = this.unidadesFiltrada = this.data.unidades;
        this.gerentes = this.gerentesFiltrado = this.data.gerentes;
	}

    public ngOnInit(): void {
        this.form.formGroup.controls['usuarioTfs'].valueChanges.subscribe(valor => {
            this.usuariosTfsFiltrado = this.filtrarUsuariosTfs(valor || '');            
        });

        this.form.formGroup.controls['usuarioChannel'].valueChanges.subscribe(valor => {
            this.usuariosChannelFiltrado = this.filtrarUsuariosChannel(valor || '');            
        });

        this.form.formGroup.controls['funcionarioPonto'].valueChanges.subscribe(valor => {
            this.funcionariosFiltrado = this.filtrarFuncionarios(valor || '');            
        });
    }

	public salvarUsuario(): void {
		if (this.form.isValid()) {
			const model = this.form.getModel();

			this.dialogRef.close(model);
		}
	}
    
    public obterNomeCompletoUsuario(usuario: any): string {
        return usuario ? usuario.nomeCompleto : "";
    }

    public obterNomeUnidade(unidade: any): string {
        return unidade ? unidade.nome : "";
    }
    
    private filtrarUsuariosTfs(nome: string): UsuarioTfs[] {
        if(!nome || typeof nome !== 'string') 
            return this.usuariosTfs;

        return this.usuariosTfs.filter(c => c.nomeCompleto.toLowerCase().includes(nome.toLowerCase()));
    }

    private filtrarUsuariosChannel(nome: string): UsuarioChannel[] {
        if(!nome || typeof nome !== 'string') 
            return this.usuariosChannel;

        return this.usuariosChannel.filter(c => c.nomeCompleto.toLowerCase().includes(nome.toLowerCase()));
    }

    private filtrarFuncionarios(nome: string): Funcionario[] {
        if(!nome || typeof nome !== 'string') 
            return this.funcionarios;

        return this.funcionarios.filter(c => c.nomeCompleto.toLowerCase().includes(nome.toLowerCase()));
    }    
}
