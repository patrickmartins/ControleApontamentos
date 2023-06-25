import { Component, Input } from '@angular/core';
import { RelatorioApontamentosUsuarioPorMes } from '../../models/relatorio-apontamentos-usuario-por-mes';
import { MatTableDataSource } from '@angular/material/table';

@Component({
    selector: 'grid-relatorio-apontamentos',
    templateUrl: './grid-relatorio-apontamentos.component.html',
    styleUrls: ['./grid-relatorio-apontamentos.component.scss']
})
export class GridRelatorioApontamentosComponent {

    @Input()
    public relatorioDataSource: MatTableDataSource<RelatorioApontamentosUsuarioPorMes> = new MatTableDataSource<RelatorioApontamentosUsuarioPorMes>();

    @Input()
    public tolerancia: number = 0;

    public colunasGridRelatorio: string[] = ["email", "nomeCompleto", "unidade", "gerente", "tempoTrabalhado", "tempoApontadoChannel", "tempoApontadoTfs", "diferenca", "situacao"];

    constructor() { }
}
