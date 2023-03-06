import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { Usuario } from 'src/app/conta/models/usuario';
import { ContaService } from '../services/conta.service';

@Injectable({
	providedIn: 'root'
})
export class AuthorizeGuard implements CanActivate {

	private _usuarioLogado?: Usuario;
	
	constructor(private servicoConta: ContaService, private router: Router) {
		this.servicoConta.obterUsuarioLogado().subscribe(usuario => { this._usuarioLogado = usuario; });
	}

	public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		if (route.data.autorizado && !this.servicoConta.estaAutenticado())
			this.router.navigate(["/login"]);

		if (route.data.anonimo && this.servicoConta.estaAutenticado())
            this.router.navigate(["/tarefa"]);

		const roles = Array.isArray(route.data.roles) ? route.data.roles : [];

		if (roles.length > 0 && !roles.some(c => this._usuarioLogado?.possuiRole(c)))
			this.router.navigate(["/acesso-negado"]);

		return true;
	}
}
