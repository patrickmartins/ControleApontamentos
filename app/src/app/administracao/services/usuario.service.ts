import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from 'src/app/common/services/base.service';
import { Usuario } from 'src/app/core/models/usuario';
import { environment } from 'src/environments/environment';

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
}
