import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { BaseService } from 'src/app/common/services/base.service';
import { environment } from 'src/environments/environment';
import { RelatorioApontamentosUsuarioPorMes } from '../models/relatorio-apontamentos-usuario-por-mes';
import { FiltroRelatorio } from '../models/filtro-relatorio';

@Injectable({
  providedIn: 'root'
})
export class RelatorioService extends BaseService {

	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

    public obterRelatorioApontamentosPorMes(filtro: FiltroRelatorio): Observable<RelatorioApontamentosUsuarioPorMes[]> {
        return this.get<RelatorioApontamentosUsuarioPorMes[]>(`${environment.urlApiBase}relatorio/apontamentos-por-mes`, RelatorioApontamentosUsuarioPorMes, {}, filtro);
    }
}
