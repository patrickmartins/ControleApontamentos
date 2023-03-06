import { Component, OnDestroy, OnInit } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { Subscription } from "rxjs";
import { Usuario } from "src/app/conta/models/usuario";
import { ContaService } from "src/app/core/services/conta.service";

@Component({ 
	template: ""
})
export abstract class BaseComponent implements OnDestroy {

	public usuarioLogado?: Usuario;
	public usuarioLogadoSubscription: Subscription;
	
	constructor(protected servicoConta: ContaService) {		
		this.usuarioLogadoSubscription = this.servicoConta.obterUsuarioLogado().subscribe(usuario => { this.usuarioLogado = usuario; });
	}

	public ngOnDestroy(): void {
		this.usuarioLogadoSubscription.unsubscribe();
	}
}