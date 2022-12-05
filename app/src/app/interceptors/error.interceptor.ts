import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';

import { ContaService } from '../core/services/conta.service';
import { Erro } from '../common/models/erro';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(private router: Router, private contaService: ContaService, private snackBar: MatSnackBar) {}

    public intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        return next.handle(request)
                    .pipe(catchError((response: HttpErrorResponse) => {								
                                let error = new Erro();
                        
                                if (response.status === 0) {
                                    error.source = "app";
                                    error.description = "Ocorreu um erro desconhecido. Verifique sua conexão com a internet.";

									this.snackBar.open(error.description, "OK",  {
										duration: 5000,
										verticalPosition: "top", 
										horizontalPosition: "center",
										panelClass: "erro"
									});

                                    return throwError([error]);
                                }

								if (response.status === 401) {
									const token = this.contaService.obterTokenAcesso();
									
									if(!this.contaService.estaAutenticado() || token?.Expirou()) {
										this.contaService.logout();
										this.router.navigate(["/home"]);
									}
									else {
										this.router.navigate(["/acesso-negado"]);
									}

                                    return throwError([response]);
                                }
                        
                                if (response.status === 500) {
                                    error.source = "app";
                                    error.description = "Ocorreu um erro interno. Tente novamente mais tarde.";

									this.snackBar.open(error.description, "OK",  {
										duration: 5000,
										verticalPosition: "top", 
										horizontalPosition: "center",
										panelClass: "erro"
									});
                                    return throwError([error]);
                                }   
                                
                                if(response.error)
                                    return throwError(response.error);
                                
                                return throwError(response);
                            })
                    );
    };  
}

export const httpErrorInterceptorProviders = [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
];
