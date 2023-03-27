import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { IModel } from '../models/model';

export abstract class BaseService {

    constructor(public httpClient: HttpClient) { }

	protected post<TType>(url: string, data: any): Observable<TType>;
	protected post<TType>(url: string, data: any, options: any, returnType?: (new () => IModel<any>)): Observable<TType>;
    protected post<TType>(url: string, data: any, options?: any, returnType?: (new () => IModel<any>)): Observable<TType> {
        return this.httpClient
            .post(url, data, options)
            .pipe(
                map(response => this.sucessHandler(response, returnType))
            );
    }

	protected postAny(url: string, data: any, options?: any): Observable<any> {
        return this.httpClient.post(url, data, options);
    }

    protected get<TType>(url: string, returnType: (new () => IModel<any>), options?: any): Observable<TType>;
	protected get<TType>(url: string, returnType: (new () => IModel<any>), options: any, params?: any): Observable<TType>;
	protected get<TType>(url: string, returnType: (new () => IModel<any>), options?: any, params?: any): Observable<TType> {
		if(params) {
			let httpParams = this.converterParaHttpParams(params);

			if(!options) {
				options = {};
			}

			options.params = httpParams;
		}

        return this.httpClient
            .get<TType>(url, options)
            .pipe(
                map(response => this.sucessHandler(response, returnType))
            );
    }

	protected getAny(url: string, options?: any, params?: any): Observable<any> {
		if(params) {
			let httpParams = this.converterParaHttpParams(params);

			if(!options) {
				options = {};
			}

			options.params = httpParams;
		}

        return this.httpClient.get(url, options);
    }

	private converterParaHttpParams(params: any): HttpParams {
		let httpParams = new HttpParams();

		Object.keys(params).forEach(key => {
			let value = (params as any)[key];
			
			if(value) {
				if(Array.isArray(value)) {
					value.forEach(c => httpParams = httpParams.append(key, c));
				} else {
					httpParams = httpParams.append(key, value);
				}				
			}
		});

		return httpParams;
	}

    private sucessHandler<TType>(response: any, returnType?: (new () => IModel<any>)): TType {
        if (response && returnType) {
			let entity = new returnType();

			if(Array.isArray(response)) {			
				let array = new Array<TType>();
				
				response.forEach(item => array.push(entity.criarNovo(item)));

				return array as any;
			}			

			return entity.criarNovo(response);
		}
		
        return response;
    }
}