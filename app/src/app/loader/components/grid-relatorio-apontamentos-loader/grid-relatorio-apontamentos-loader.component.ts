import { Component, Input } from '@angular/core';

@Component({
    selector: 'grid-relatorio-apontamentos-loader',
    templateUrl: './grid-relatorio-apontamentos-loader.component.html',
    styleUrls: ['./grid-relatorio-apontamentos-loader.component.scss']
})
export class GridRelatorioApontamentosLoaderComponent {

    @Input()
    public quantidadeLinhas: number = 5;

    constructor() { }

}
