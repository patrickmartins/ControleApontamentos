import { Component, Input } from '@angular/core';

@Component({
    selector: 'usuarios-loader',
    templateUrl: './usuarios-loader.component.html',
    styleUrls: ['./usuarios-loader.component.scss']
})
export class UsuariosLoaderComponent {

    @Input()
    public quantidadeLinhas: number = 5;
    
    constructor() { }
}
