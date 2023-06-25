import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from 'src/app/common/services/base.service';
import { Unidade } from 'src/app/core/models/unidade';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UnidadeService extends BaseService {

	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

    public obterTodasUnidades(): Observable<Unidade[]> {
        return this.get<Unidade[]>(`${environment.urlApiBase}administracao/unidade`, Unidade, { });
    }
}
