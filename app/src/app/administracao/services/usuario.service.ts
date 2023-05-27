import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from 'src/app/common/services/base.service';
import { Usuario } from 'src/app/core/models/usuario';
import { environment } from 'src/environments/environment';
import { AtualizarUsuario } from '../models/atualizar-usuario';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService extends BaseService {

	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

    public obterTodosUsuarios(): Observable<Usuario[]> {
        return this.get<Usuario[]>(`${environment.urlApiBase}usuario`, Usuario, { });
    }

    public salvarUsuario(usuario: AtualizarUsuario): Observable<any> {
        return this.post<any>(`${environment.urlApiBase}usuario`, usuario);
    }
}
