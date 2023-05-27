import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from 'src/app/common/services/base.service';
import { environment } from 'src/environments/environment';
import { ApontamentosChannelDia } from '../../apontamento/models/apontamentos-channel-dia';
import { ApontamentosChannelMes } from '../../apontamento/models/apontamentos-channel-mes';
import { UsuarioChannel } from '../models/usuarioChannel';

@Injectable({
  providedIn: 'root'
})
export class ChannelService extends BaseService {

    constructor(httpClient: HttpClient) {
        super(httpClient);
    }
    
    public obterApontamentosChannelPorDia(data: Date): Observable<ApontamentosChannelDia> {
        return this.get<any>(`${environment.urlApiBase}channel/apontamento/por-dia`, ApontamentosChannelDia, {}, { data: `${data.getFullYear()}-${data.getMonth() + 1}-${data.getDate()}` });
    }

    public obterApontamentosChannelDeUsuarioPorDia(id: string, data: Date): Observable<ApontamentosChannelDia> {
        return this.get<any>(`${environment.urlApiBase}channel/apontamento/${id}/por-dia`, ApontamentosChannelDia, {}, { data: `${data.getFullYear()}-${data.getMonth() + 1}-${data.getDate()}` });
    }

    public obterApontamentosChannelPorMes(mes: number, ano: number): Observable<ApontamentosChannelMes> {
        return this.get<any>(`${environment.urlApiBase}channel/apontamento/por-mes`, ApontamentosChannelMes, {}, { mes: mes, ano: ano });
    }

    public obterApontamentosChannelDeUsuarioPorMes(id: string, mes: number, ano: number): Observable<ApontamentosChannelMes> {
        return this.get<any>(`${environment.urlApiBase}channel/apontamento/${id}/por-mes`, ApontamentosChannelMes, {}, { mes: mes, ano: ano });
    }

    public obterTodosUsuarios(): Observable<UsuarioChannel[]> {
		return this.get<UsuarioChannel[]>(`${environment.urlApiBase}channel/usuarios`, UsuarioChannel);
	}
}
