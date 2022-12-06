import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { TemaService } from './core/services/tema.service';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})

export class AppComponent {

	constructor(private temaService: TemaService) {
		this.temaService.aplicarTemaAtual();
		this.removerTokensAntigos();
	}	

	public removerTokensAntigos() {
		let valor = localStorage.getItem(environment.chaveStorageToken);

		if(!valor)
			return;

		let token = JSON.parse(valor);

		if(typeof token.validade === 'number') {
			localStorage.removeItem(environment.chaveStorageToken);
			localStorage.removeItem(environment.chaveStorageUsuario);
		}
	}
}
