import { Component, Inject, OnDestroy } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { MsalGuardConfiguration, MsalService, MSAL_GUARD_CONFIG } from '@azure/msal-angular';
import { AuthenticationResult, PopupRequest } from '@azure/msal-browser';
import { Router } from '@angular/router';

import { BaseComponent } from 'src/app/common/components/base.component';
import { StatusLogin } from 'src/app/core/models/status-login';
import { ContaService } from 'src/app/core/services/conta.service';
import { Subscription } from 'rxjs';

@Component({
	selector: 'login-menu',
	templateUrl: './login-menu.component.html',
	styleUrls: ['./login-menu.component.scss']
})

export class LoginMenuComponent extends BaseComponent implements OnDestroy{

	public fotoUsuarioLogado?: SafeUrl;
	public entrando: boolean = false;
	public onStatusLoginAlteradoSubscription: Subscription;

	constructor(servicoConta: ContaService, 
		private router: Router, 		
		private servicoMsal: MsalService,
		private sanitizer: DomSanitizer,
		@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration) {
			super(servicoConta);

			this.onStatusLoginAlteradoSubscription = this.servicoConta.onStatusLoginAlterado.subscribe((status: StatusLogin) => this.onStatusLoginAlterado(status))
	}

	public override ngOnDestroy(): void {
        this.onStatusLoginAlteradoSubscription.unsubscribe();
		
		super.ngOnDestroy();
    }

	public login(): void {
		this.servicoMsal
			.loginPopup({ ...this.msalGuardConfig.authRequest } as PopupRequest)
			.subscribe((response: AuthenticationResult) => {
				this.servicoMsal.instance.setActiveAccount(response.account);				
				
				this.servicoConta.login().subscribe({
					next: () => {
						this.router.navigate(["/tarefas"]);
					}
				});
			});
	}

	public logout(): void {
		this.servicoMsal
			.logoutPopup()
			.subscribe(() => {
				this.servicoConta.logout();
				this.router.navigate(["/login"]);
			});
	}

	private onStatusLoginAlterado(status: StatusLogin): void {
		this.entrando = status == StatusLogin.Conectando;

		if(status == StatusLogin.Conectado) {
			this.obterFotoUsuarioLogado();
		}
	}

	private obterFotoUsuarioLogado() {
		this.servicoConta.obterFotoUsuarioLogado().subscribe({
			next: (resposta: any) => {
				const url = URL.createObjectURL(resposta.body);

				this.fotoUsuarioLogado = this.sanitizer.bypassSecurityTrustUrl(url);
			}
		});
	}
}
