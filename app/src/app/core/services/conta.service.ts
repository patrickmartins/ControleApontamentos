import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { BaseService } from 'src/app/common/services/base.service';
import { LocalStorageHelper } from 'src/app/helpers/local-storage.helper';
import { Usuario } from 'src/app/conta/models/usuario';
import { LoginSucesso } from 'src/app/conta/models/login-sucess';
import { JwtToken } from 'src/app/conta/models/jwt.token';
import { StatusLogin } from '../models/status-login';

@Injectable({
    providedIn: 'root'
})
export class ContaService extends BaseService {
    private _token?: JwtToken;

    private _onStatusLoginAlteradoSubscription : Subject<number>;
	private _usuarioLogadoSubscription : Subject<Usuario | undefined>;
	private _usuarioLogado : Observable<Usuario | undefined>;    

	public onStatusLoginAlterado : Observable<StatusLogin>;

    constructor(httpClient: HttpClient) {
        super(httpClient);

		this._usuarioLogadoSubscription = new BehaviorSubject(LocalStorageHelper.obterDados(environment.chaveStorageUsuario, Usuario));
		this._usuarioLogado = this._usuarioLogadoSubscription.asObservable();

        this._onStatusLoginAlteradoSubscription = new BehaviorSubject(this.estaAutenticado() ? StatusLogin.Conectado : StatusLogin.Desconectado);
		this.onStatusLoginAlterado = this._onStatusLoginAlteradoSubscription.asObservable();
    }

    public estaAutenticado(): boolean {
        return LocalStorageHelper.dadoExiste(environment.chaveStorageToken) &&
				LocalStorageHelper.dadoExiste(environment.chaveStorageUsuario);        
    }

    public login(): Observable<LoginSucesso> {
        this._onStatusLoginAlteradoSubscription.next(StatusLogin.Conectando);

        var observable = this.post<LoginSucesso>(`${environment.urlApiBase}conta/login`, { }, undefined, LoginSucesso).pipe(map(this.onLoginSucesso, this));

        observable.subscribe({
            next: () => {
                this._onStatusLoginAlteradoSubscription.next(StatusLogin.Conectado);
            },
            error: () => {
                this._onStatusLoginAlteradoSubscription.next(StatusLogin.Desconectado);
            }
        });

        return observable;
    }

    public logout(): void {
        if (LocalStorageHelper.dadoExiste(environment.chaveStorageToken)) { 
            LocalStorageHelper.removerDados(environment.chaveStorageToken);
		}

		if (LocalStorageHelper.dadoExiste(environment.chaveStorageUsuario)) { 
            LocalStorageHelper.removerDados(environment.chaveStorageUsuario);
		}

		this._usuarioLogadoSubscription.next(undefined);
        this._onStatusLoginAlteradoSubscription.next(StatusLogin.Desconectado);

		this._token = undefined;
    }

    public obterUsuarioLogado(): Observable<Usuario | undefined> {
        return this._usuarioLogado;
    }

    public obterTokenAcesso(): JwtToken | undefined {
		if(!this._token)
			this._token = LocalStorageHelper.obterDados(environment.chaveStorageToken, JwtToken);

        return this._token;
    }

	public obterFotoUsuarioLogado(): any {
		return this.getAny(`${environment.urlGraphAzure}photo/$value`, { observe: 'response', responseType: 'blob' });
	}

    private onLoginSucesso(sucess: LoginSucesso): LoginSucesso {
        LocalStorageHelper.salvarDados(environment.chaveStorageToken, sucess.token);
		LocalStorageHelper.salvarDados(environment.chaveStorageUsuario, sucess.usuario);

		this._usuarioLogadoSubscription.next(sucess.usuario);
		
        return sucess;
    }	
}