import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';

import { RelatorioService } from '../../services/relatorio.service';
import { BaseComponent } from 'src/app/common/components/base.component';
import { ContaService } from 'src/app/core/services/conta.service';
import { RelatorioApontamentosUsuarioPorMes } from '../../models/relatorio-apontamentos-usuario-por-mes';
import { FiltroRelatorio } from '../../models/filtro-relatorio';
import { FormBase } from 'src/app/common/form/form-base';
import { TempoHelper } from 'src/app/helpers/tempo.helper';
import { PaginatorPortugues } from 'src/app/core/configs/paginator-portugues';
import { UnidadeService } from '../../services/unidade.service';
import { Unidade } from 'src/app/core/models/unidade';
import { UsuarioService } from '../../services/usuario.service';
import { forkJoin } from 'rxjs';
import { Usuario } from 'src/app/core/models/usuario';
import { SituacaoApontamentos } from '../../models/situacao-apontamentos';
import { MatExpansionPanel } from '@angular/material/expansion';
import { MatSelectChange } from '@angular/material/select';

@Component({
    selector: 'relatorio-apontamentos',
    templateUrl: './relatorio-apontamentos.component.html',
    styleUrls: ['./relatorio-apontamentos.component.scss'],
    providers: [{
        provide: MatPaginatorIntl,
        useClass: PaginatorPortugues
    }]
})
export class RelatorioApontamentosComponent extends BaseComponent implements OnInit {
    public form = new FormBase<FiltroRelatorio>(FiltroRelatorio);

    public anos: number[] = [];
    public meses: any[] = [];

    public unidades: Unidade[] = [];
    public gerentes: Usuario[] = [];

    public unidadeSelecionada?: Unidade;
    public gerenteSelecionado?: Usuario;
    public nomeUsuarioBusca?: string;
    public situacaoSelecionada: string = "0";
    public tipoExibicaoSelecionado: string = "quadros";

    public carregandoRelatorio: boolean = false;
    public carregandoFiltros: boolean = false;

    public relatorio: MatTableDataSource<RelatorioApontamentosUsuarioPorMes> = new MatTableDataSource<RelatorioApontamentosUsuarioPorMes>([]);

    public toleranciaModel: string = "06:00";
    public get tolerancia(): number {
        return TempoHelper.stringParaMinutos(this.toleranciaModel);
    }

    public anoAtual: number = new Date().getFullYear();
    public mesAtual: number = new Date().getMonth() + 1;

    @ViewChild(MatPaginator)
    public paginator!: MatPaginator;

    @ViewChild("painelParametros")
    public painelParametros!: MatExpansionPanel;

    constructor(servicoConta: ContaService, snackBar: MatSnackBar, private servicoUsuario: UsuarioService, private servicoRelatorio: RelatorioService, private servicoUnidade: UnidadeService) {
        super(servicoConta, snackBar);

        this.form.formGroup.controls['ano'].setValue(this.anoAtual);
        this.form.formGroup.controls['mes'].setValue(this.mesAtual);
        this.form.formGroup.controls['ordenacao'].setValue("0");
        this.form.formGroup.controls['somenteSincronizados'].setValue(false);
        this.form.formGroup.controls['somenteUsuariosPonto'].setValue(true);
        this.form.formGroup.controls['somenteComTempoTrabalhado'].setValue(true);
        this.form.formGroup.controls['somenteAteDiaAnterior'].setValue(true);
        
        this.anos = this.obterOpcoesFiltroAnos();
        this.meses = this.obterOpcoesFiltroMeses(this.anoAtual);
    }

    public ngOnInit(): void {
        this.carregandoFiltros = true;

        forkJoin({
            unidades: this.servicoUnidade.obterTodasUnidades(),
            gerentes: this.servicoUsuario.obterTodosGerentes()
        })
            .subscribe({
                next: resultado => {
                    var unidades = resultado.unidades;
                    var gerentes = resultado.gerentes;

                    this.unidadeSelecionada = new Unidade().criarNovo({
                        id: "0",
                        nome: "Todas"
                    });

                    this.gerenteSelecionado = new Usuario().criarNovo({
                        id: "0",
                        nomeCompleto: "Todos"
                    });

                    this.unidades = [this.unidadeSelecionada!].concat(unidades);
                    this.gerentes = [this.gerenteSelecionado!].concat(gerentes);
                },
                complete: () => this.carregandoFiltros = false
            });
    }

    public visualizarRelatorio(): void {
        if (this.form.isValid()) {
            this.carregandoRelatorio = true;

            const filtro = this.form.getModel();

            this.painelParametros.close();

            this.servicoRelatorio
                .obterRelatorioApontamentosPorMes(filtro)
                .subscribe({
                    next: (relatorios: RelatorioApontamentosUsuarioPorMes[]) => {
                        this.relatorio = new MatTableDataSource<RelatorioApontamentosUsuarioPorMes>(relatorios);
                        this.relatorio.paginator = this.paginator;

                        this.relatorio.filterPredicate = this.filtrarGrid;

                        this.onFiltrarGrid();
                    },
                    complete: () => this.carregandoRelatorio = false
                });
        }
    }

    public onFiltrarGrid(): void {
        this.relatorio.filter = JSON.stringify({
            nomeCompleto: this.nomeUsuarioBusca,
            idUnidade: this.unidadeSelecionada && this.unidadeSelecionada?.id != "0" ? this.unidadeSelecionada?.id : undefined,
            idGerente: this.gerenteSelecionado && this.gerenteSelecionado?.id != "0" ? this.gerenteSelecionado?.id : undefined,
            situacao: this.situacaoSelecionada != "0" ? this.situacaoSelecionada : undefined,
            tolerancia: this.tolerancia
        });
    }

    public onAnoAlterado(evento: MatSelectChange): void {
        this.meses = this.obterOpcoesFiltroMeses(evento.value);
    }

    private filtrarGrid(relatorio: RelatorioApontamentosUsuarioPorMes, filtroJson: string): boolean {
        var filtro = JSON.parse(filtroJson);
        var situacao = filtro.situacao ? (filtro.situacao == "1" ? SituacaoApontamentos.Ok : SituacaoApontamentos.Verificar) : undefined;

        return (!filtro.nomeCompleto || filtro.nomeCompleto == "" || relatorio.usuario.nomeCompleto.toLowerCase().indexOf(filtro.nomeCompleto) >= 0) &&
            (!filtro.idUnidade || filtro.idUnidade == "" || relatorio.usuario.unidade?.id == filtro.idUnidade) &&
            (!filtro.idGerente || filtro.idGerente == "" || relatorio.usuario.idGerente == filtro.idGerente) &&
            (!situacao || relatorio.calcularSituacao(filtro.tolerancia) == situacao);
    }

    private obterOpcoesFiltroAnos(): number[] {
        return Array(this.anoAtual - (this.anoAtual - 10)).fill('').map((v, i) => this.anoAtual - i).sort((n1, n2) => n1 - n2);
    }

    private obterOpcoesFiltroMeses(ano: number): any[] {
        return ([{ mes: 1, nome: "Janeiro" },
                { mes: 2, nome: "Fevereiro" },
                { mes: 3, nome: "Março" },
                { mes: 4, nome: "Abril" },
                { mes: 5, nome: "Maio" },
                { mes: 6, nome: "Junho" },
                { mes: 7, nome: "Julho" },
                { mes: 8, nome: "Agosto" },
                { mes: 9, nome: "Setembro" },
                { mes: 10, nome: "Outubro" },
                { mes: 11, nome: "Novembro" },
                { mes: 12, nome: "Dezembro" }]).filter(c => ano < this.anoAtual || c.mes <= this.mesAtual);
    }
}
