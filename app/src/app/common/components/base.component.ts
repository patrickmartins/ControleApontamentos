import { Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Observable, of, Subscription } from "rxjs";
import { Usuario } from "src/app/core/models/usuario";
import { ContaService } from "src/app/core/services/conta.service";
import { Erro } from "../models/erro";

@Component({ 
	template: ""
})
export abstract class BaseComponent implements OnDestroy {

	public usuarioLogado?: Usuario;
	public usuarioLogadoSubscription: Subscription;
	
	constructor(protected servicoConta: ContaService, protected snackBar: MatSnackBar) {		
		this.usuarioLogadoSubscription = this.servicoConta.obterUsuarioLogado().subscribe(usuario => { this.usuarioLogado = usuario; });
	}

	public ngOnDestroy(): void {
		this.usuarioLogadoSubscription.unsubscribe();
	}

    protected exibirMensagemDeErro(erro: Erro): void {
        this.snackBar.open(erro.descricao, "OK",  {
            duration: 5000,
            verticalPosition: "top", 
            horizontalPosition: "center",
            panelClass: "erro"
        });
    }
    
    
    protected pipeErrosDeNegocio(erros: Erro[]): Observable<any> {
        if(Array.isArray(erros)) {
            erros.forEach(erro => {
                if(erro.codigo == 400) {
                    this.exibirMensagemDeErro(erro);
                }
            });
        }

        return of(undefined);
    }
}