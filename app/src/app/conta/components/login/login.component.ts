import { Component, Inject, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { MsalGuardConfiguration, MsalService, MSAL_GUARD_CONFIG } from '@azure/msal-angular';
import { AuthenticationResult, PopupRequest } from '@azure/msal-browser';
import { Subscription } from 'rxjs';
import { ContaService } from 'src/app/core/services/conta.service';
import { TemaService } from 'src/app/core/services/tema.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnDestroy {

	public ehTemaEscuro?: boolean;
	public entrando: boolean = false;
    public onTemaAlteradoSubscription: Subscription;

  	constructor(private router: Router, 
        private servicoConta: ContaService, 
        private servicoTema: TemaService,
        private servicoMsal: MsalService,
		@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration) 
	{
		this.onTemaAlteradoSubscription = this.servicoTema.onTemaAlterado.subscribe(() => this.ehTemaEscuro = this.servicoTema.ehTemaEscuro());
	}

    public ngOnDestroy(): void {
        this.onTemaAlteradoSubscription.unsubscribe();
    }

	public login(): void {
        this.entrando = true;

		this.servicoMsal
			.loginPopup({ ...this.msalGuardConfig.authRequest } as PopupRequest)
			.subscribe({
                next: (response: AuthenticationResult) => {
                    this.servicoMsal.instance.setActiveAccount(response.account);				
                    
                    this.servicoConta.login().subscribe({
                        next: () => {
                            this.router.navigate(["/tarefas"]);
                        },
                        error: () => this.entrando = false
                    });
			    },
                error: () => this.entrando = false
            });
	}
}
