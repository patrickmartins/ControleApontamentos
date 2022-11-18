import { Component, EventEmitter, Inject, OnInit, Output } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { MsalGuardConfiguration, MsalService, MSAL_GUARD_CONFIG } from '@azure/msal-angular';
import { AuthenticationResult, PopupRequest } from '@azure/msal-browser';
import { BaseComponent } from 'src/app/common/components/base.component';
import { ContaService } from 'src/app/core/services/conta.service';

@Component({
	selector: 'login-menu',
	templateUrl: './login-menu.component.html',
	styleUrls: ['./login-menu.component.scss']
})

export class LoginMenuComponent extends BaseComponent implements OnInit{

	@Output()
	public onLoginEntrando = new EventEmitter;

	@Output()
	public onLoginSucesso = new EventEmitter;

	@Output()
	public onLoginErro = new EventEmitter;

	@Output()
	public onLogout = new EventEmitter;

	public fotoUsuarioLogado?: SafeUrl;

	constructor(servicoConta: ContaService, 
		private router: Router, 
		@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration, 
		private servicoMsal: MsalService,
		private sanitizer: DomSanitizer) {
			super(servicoConta);
	}
	
	public ngOnInit(): void {
		if(this.usuarioLogado)
			this.obterFotoUsuarioLogado();		
	}

	public login(): void {

		this.servicoMsal
			.loginPopup({ ...this.msalGuardConfig.authRequest } as PopupRequest)
			.subscribe((response: AuthenticationResult) => {
				this.servicoMsal.instance.setActiveAccount(response.account);

				this.onLoginEntrando.emit();
				
				this.obterFotoUsuarioLogado();

				this.servicoConta.login().subscribe({
					next: (login) => {
						this.usuarioLogado = login.usuario;
					},
					error: () => {
						this.onLoginErro.emit();
					},
					complete: () => {
						this.onLoginSucesso.emit();
					}
				});
			});
	}

	public logout(): void {

		this.servicoMsal
			.logoutPopup()
			.subscribe(() => {
				this.servicoConta.logout();

				this.onLogout.emit();

				this.router.navigate(["/home"]);
			});
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
