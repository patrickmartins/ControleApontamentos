import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { BaseService } from 'src/app/common/services/base.service';
import { LocalStorageHelper } from 'src/app/helpers/local-storage.helper';
import { Usuario } from 'src/app/conta/models/usuario';
import { LoginSucesso } from 'src/app/conta/models/login-sucess';
import { JwtToken } from 'src/app/conta/models/jwt.token';

@Injectable({
    providedIn: 'root'
})
export class ContaService extends BaseService {

	private _usuarioLogadoSubscription : Subject<Usuario | undefined>;
	private _usuarioLogado : Observable<Usuario | undefined>;

    constructor(httpClient: HttpClient) {
        super(httpClient);

		this._usuarioLogadoSubscription = new BehaviorSubject(LocalStorageHelper.obterDados(environment.chaveStorageUsuario, Usuario));
		this._usuarioLogado = this._usuarioLogadoSubscription.asObservable();
    }

    public estaAutenticado(): boolean {
        return LocalStorageHelper.dadoExiste(environment.chaveStorageToken) &&
		LocalStorageHelper.obterDados(environment.chaveStorageToken, JwtToken) != null;        
    }

    public login(): Observable<LoginSucesso> {
        return this.post<LoginSucesso>(`${environment.urlApiBase}conta/login`, { }, undefined, LoginSucesso).pipe(map(this.onLoginSucesso, this));
    }

    public logout() {
        if (LocalStorageHelper.dadoExiste(environment.chaveStorageToken)) { 
            LocalStorageHelper.removerDados(environment.chaveStorageToken);
		}

		if (LocalStorageHelper.dadoExiste(environment.chaveStorageUsuario)) { 
            LocalStorageHelper.removerDados(environment.chaveStorageUsuario);
		}

		this._usuarioLogadoSubscription.next(undefined);
    }

    public obterUsuarioLogado(): Observable<Usuario | undefined> {
        return this._usuarioLogado;
    }

    public obterTokenAcesso(): JwtToken | undefined {
        return LocalStorageHelper.obterDados(environment.chaveStorageToken, JwtToken);
    }

	public obterFotoUsuarioLogado(): any {
		return this.getAny(`${environment.urlGraphAzure}/photo/$value`, { observe: 'response', responseType: 'blob' });
	}

    private onLoginSucesso(sucess: LoginSucesso): LoginSucesso {
        LocalStorageHelper.salvarDados(environment.chaveStorageToken, sucess.token);
		LocalStorageHelper.salvarDados(environment.chaveStorageUsuario, sucess.usuario);

		this._usuarioLogadoSubscription.next(sucess.usuario);
		
        return sucess;
    }
	
}