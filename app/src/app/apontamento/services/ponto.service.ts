import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from 'src/app/common/services/base.service';
import { environment } from 'src/environments/environment';
import { BatidasPontoDia } from '../models/batidas-ponto-dia';
import { BatidasPontoMes } from '../models/batidas-ponto-mes';

@Injectable({
	providedIn: 'root'
})
export class PontoService extends BaseService {

	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	public obterBatidasPorDia(data: Date): Observable<BatidasPontoDia> {
        return this.get<any>(`${environment.urlApiBase}ponto/por-dia`, BatidasPontoDia, { }, { data: `${data.getFullYear()}-${data.getMonth()+1}-${data.getDate()}` });
    }

	public obterBatidasPorMes(mes: number, ano: number): Observable<BatidasPontoMes> {
        return this.get<any>(`${environment.urlApiBase}ponto/por-mes`, BatidasPontoMes, { }, { mes: mes, ano: ano });
    }
}
