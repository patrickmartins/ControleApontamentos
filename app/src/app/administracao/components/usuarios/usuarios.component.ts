import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { forkJoin } from 'rxjs';

import { UsuarioService } from '../../services/usuario.service';
import { TfsService } from 'src/app/core/services/tfs.service';
import { ChannelService } from 'src/app/core/services/channel.service';
import { PontoService } from 'src/app/apontamento/services/ponto.service';
import { UsuarioTfs } from 'src/app/core/models/usuarioTfs';
import { UsuarioChannel } from 'src/app/core/models/usuarioChannel';
import { Funcionario } from 'src/app/core/models/funcionario';
import { BaseComponent } from 'src/app/common/components/base.component';
import { ContaService } from 'src/app/core/services/conta.service';
import { PaginatorPortugues } from 'src/app/core/configs/paginator-portugues';
import { ModalSalvarUsuarioComponent } from '../modal-salvar-usuario/modal-salvar-usuario.component';
import { Usuario } from 'src/app/core/models/usuario';
import { AtualizarUsuario } from '../../models/atualizar-usuario';

@Component({
    selector: 'usuarios',
    templateUrl: './usuarios.component.html',
    styleUrls: ['./usuarios.component.scss'],
    providers: [{
        provide: MatPaginatorIntl,
        useClass: PaginatorPortugues
    }]
})
export class UsuariosComponent extends BaseComponent implements OnInit {

    public colunasGridUsuarios: string[] = ["email", "nomeCompleto", "usuarioTfs", "usuarioChannel", "numeroFolha", "administrador", "acoes"];

    public usuariosApp: MatTableDataSource<Usuario> = new MatTableDataSource<Usuario>([]);
    public usuariosTfs: UsuarioTfs[] = [];
    public usuariosChannel: UsuarioChannel[] = [];
    public funcionarios: Funcionario[] = [];

    public carregando: boolean = true;
    public salvando: boolean = false;

    @ViewChild(MatPaginator)
    public paginator!: MatPaginator;

    constructor(servicoConta: ContaService,
        snackBar: MatSnackBar,
        private dialog: MatDialog,
        private servicoUsuario: UsuarioService,
        private servicoTfs: TfsService,
        private servicoChannel: ChannelService,
        private servicoPonto: PontoService) {
        super(servicoConta, snackBar);
    }

    public ngOnInit(): void {
        this.carregando = true;

        forkJoin({
            usuariosApp: this.servicoUsuario.obterTodosUsuarios(),
            usuariosTfs: this.servicoTfs.obterTodosUsuarios(),
            usuariosChannel: this.servicoChannel.obterTodosUsuarios(),
            funcionarios: this.servicoPonto.obterTodosFuncionarios()
        })
        .subscribe({
            next: (resultado: any) => {
                let usuariosApp = resultado.usuariosApp;
                this.usuariosTfs = resultado.usuariosTfs;
                this.usuariosChannel = resultado.usuariosChannel;
                this.funcionarios = resultado.funcionarios;

                for (let usuario of usuariosApp) {
                    usuario.usuarioChannel = this.usuariosChannel.find(c => c.id == usuario.idUsuarioChannel);
                    usuario.usuarioTfs = this.usuariosTfs.find(c => c.id == usuario.idUsuarioTfs);
                    usuario.funcionarioPonto = this.funcionarios.find(c => c.id == usuario.idFuncionarioPonto);
                }

                this.usuariosApp = new MatTableDataSource(usuariosApp);
                this.usuariosApp.paginator = this.paginator;
            },
            complete: () => this.carregando = false
        });
    }

    public editarUsuario(usuario: Usuario) {
        let dialogRef = this.dialog.open(ModalSalvarUsuarioComponent, {
            width: '500px',
            height: '470px',
            disableClose: true,
            data: {
                usuario: new AtualizarUsuario().criarNovo({
                    idUsuario: usuario.id,
                    idUsuarioTfs: usuario.idUsuarioTfs,
                    idUsuarioChannel: usuario.idUsuarioChannel,
                    idFuncionarioPonto: usuario.idFuncionarioPonto,
                    ehAdministrador: usuario.ehAdministrador,
                    nomeCompleto: usuario.nomeCompleto,
                    usuarioTfs: usuario.usuarioTfs,
                    usuarioChannel: usuario.usuarioChannel,
                    funcionarioPonto: usuario.funcionarioPonto
                }),
                usuariosTfs: this.usuariosTfs,
                usuariosChannel: this.usuariosChannel,
                funcionarios: this.funcionarios
            }
        });

        dialogRef.afterClosed().subscribe((usuarioAlterado: AtualizarUsuario) => {
            if (usuarioAlterado) {
                this.salvando = true;

                this.servicoUsuario.salvarUsuario(usuarioAlterado).subscribe({
                    next: () => {
                        var usuario = this.usuariosApp.data.find(c => c.id == usuarioAlterado.idUsuario);

                        usuario!.usuarioTfs = usuarioAlterado.usuarioTfs;
                        usuario!.usuarioChannel = usuarioAlterado.usuarioChannel;
                        usuario!.funcionarioPonto = usuarioAlterado.funcionarioPonto;
                        usuario!.ehAdministrador = usuarioAlterado.ehAdministrador;

                        this.snackBar.open("Usuário salvo com sucesso!", "OK", {
                            duration: 5000,
                            verticalPosition: "bottom",
                            horizontalPosition: "center",
                            panelClass: "sucesso"
                        });
                    },
                    error: () => {
                        this.snackBar.open("Ocorreu um erro interno. Atualize a página e tente novamente.", "OK", {
                            duration: 5000,
                            verticalPosition: "top",
                            horizontalPosition: "center",
                            panelClass: "erro"
                        });
                    },
                    complete: () => this.salvando = false
                });
            }
        });
    }

    public filtrarGrid(event: Event): void {
        const valor = (event.target as HTMLInputElement).value;
        this.usuariosApp.filter = valor.trim().toLowerCase();
    }
}
