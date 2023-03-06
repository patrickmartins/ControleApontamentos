import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from 'src/app/common/services/base.service';
import { environment } from 'src/environments/environment';
import { JobInfo } from '../models/job-info';

@Injectable({
  providedIn: 'root'
})
export class JobService extends BaseService {

	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	public obterJobCarga(): Observable<JobInfo> {
		return this.get<JobInfo>(`${environment.urlApiBase}hangfire/obter-job-carga`, JobInfo);
	}
}
