import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HTTP_INTERCEPTORS } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { ContaService } from "../core/services/conta.service";

@Injectable()

export class DefaultInterceptor implements HttpInterceptor {

    constructor(private contaService: ContaService) { }

    public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		let urlBase = environment.urlApiBase;

		if(!request.url.includes(urlBase))
			return next.handle(request);

        if (this.contaService.estaAutenticado()) {
            let token = this.contaService.obterTokenAcesso();

			if(token) {
				request = request.clone({
					setHeaders: {
						Authorization: `Bearer ${token.tokenAcesso}`
					}
				});
			}
        }

        return next.handle(request);
    }
}

export const httpDefaultInterceptorProviders = [
    { provide: HTTP_INTERCEPTORS, useClass: DefaultInterceptor, multi: true },
];