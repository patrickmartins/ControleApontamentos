import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { forkJoin } from 'rxjs';

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
import { AtualizarContaUsuario } from '../../../core/models/atualizar-conta-usuario';
import { Erro } from 'src/app/common/models/erro';
import { UnidadeService } from '../../services/unidade.service';
import { Unidade } from 'src/app/core/models/unidade';
import { UsuarioService } from '../../services/usuario.service';

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

    public colunasGridUsuarios: string[] = ["email", "nomeCompleto", "unidade", "gerente", "usuarioTfs", "usuarioChannel", "numeroFolha", "administrador", "acoes"];

    public usuariosApp: MatTableDataSource<Usuario> = new MatTableDataSource<Usuario>([]);
    public usuariosTfs: UsuarioTfs[] = [];
    public usuariosChannel: UsuarioChannel[] = [];
    public funcionarios: Funcionario[] = [];
    public unidades: Unidade[] = [];
    public gerentes: Usuario[] = [];

    public carregando: boolean = true;
    public salvando: boolean = false;

    @ViewChild(MatPaginator)
    public paginator!: MatPaginator;

    constructor(servicoConta: ContaService,
        snackBar: MatSnackBar,
        private dialog: MatDialog,
        private servicotfs: TfsService,
        private servicoChannel: ChannelService,
        private servicoPonto: PontoService,
        private servicoUsuario: UsuarioService,
        private servicoUnidade: UnidadeService) {
        super(servicoConta, snackBar);
    }

    public ngOnInit(): void {
        this.carregando = true;

        forkJoin({
            usuariosApp: this.servicoConta.obterTodasContas(),
            usuariosTfs: this.servicotfs.obterTodosUsuarios(), 
            usuariosChannel: this.servicoChannel.obterTodosUsuarios(),
            funcionarios: this.servicoPonto.obterTodosFuncionarios(),
            unidades: this.servicoUnidade.obterTodasUnidades(),
            gerentes: this.servicoUsuario.obterTodosGerentes()
        })
        .subscribe({
            next: (resultado: any) => {
                let usuariosApp = resultado.usuariosApp;
                this.usuariosTfs = resultado.usuariosTfs;
                this.usuariosChannel = resultado.usuariosChannel;
                this.funcionarios = resultado.funcionarios;
                this.unidades = resultado.unidades;
                this.gerentes = resultado.gerentes;
                
                for (let usuario of usuariosApp) {
                    usuario.usuarioChannel = this.usuariosChannel.find(c => c.id == usuario.idUsuarioChannel);
                    usuario.usuarioTfs = this.usuariosTfs.find(c => c.id == usuario.idUsuarioTfs);
                    usuario.funcionarioPonto = this.funcionarios.find(c => c.id == usuario.idFuncionarioPonto);
                    usuario.unidade = this.unidades.find(c => c.id == usuario.idUnidade);
                    usuario.gerente = this.gerentes.find(c => c.id == usuario.idGerente);
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
            height: '600px',
            disableClose: true,
            data: {
                usuario: new AtualizarContaUsuario().criarNovo({
                    idUsuario: usuario.id,
                    idUsuarioTfs: usuario.idUsuarioTfs,
                    idUsuarioChannel: usuario.idUsuarioChannel,
                    idFuncionarioPonto: usuario.idFuncionarioPonto,
                    ehAdministrador: usuario.ehAdministrador,
                    nomeCompleto: usuario.nomeCompleto,
                    usuarioTfs: usuario.usuarioTfs,
                    usuarioChannel: usuario.usuarioChannel,
                    funcionarioPonto: usuario.funcionarioPonto,
                    gerente: usuario.gerente,
                    unidade: usuario.unidade
                }),
                usuariosTfs: this.usuariosTfs,
                usuariosChannel: this.usuariosChannel,
                funcionarios: this.funcionarios,
                unidades: this.unidades,
                gerentes: this.gerentes
            }
        });

        dialogRef.afterClosed().subscribe((usuarioAlterado: AtualizarContaUsuario) => {
            if (usuarioAlterado) {
                this.salvando = true;

                this.servicoConta.salvarContaUsuario(usuarioAlterado).subscribe({
                    next: () => {
                        var usuario = this.usuariosApp.data.find(c => c.id == usuarioAlterado.idUsuario);

                        usuario!.usuarioTfs = usuarioAlterado.usuarioTfs;
                        usuario!.usuarioChannel = usuarioAlterado.usuarioChannel;
                        usuario!.funcionarioPonto = usuarioAlterado.funcionarioPonto;
                        usuario!.unidade = usuarioAlterado.unidade;
                        usuario!.gerente = usuarioAlterado.gerente;
                        usuario!.ehAdministrador = usuarioAlterado.ehAdministrador;                        

                        this.snackBar.open("Usuário salvo com sucesso!", "OK", {
                            duration: 5000,
                            verticalPosition: "bottom",
                            horizontalPosition: "center",
                            panelClass: "sucesso"
                        });
                    },
                    error: (erros: Erro[]) => {
                        for(let erro of erros) {
                            this.snackBar.open(erro.descricao, "OK", {
                                duration: 5000,
                                verticalPosition: "top",
                                horizontalPosition: "center",
                                panelClass: "erro"
                            });
                        }

                        this.salvando = false;
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
