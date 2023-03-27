import { MsalBroadcastService, MsalGuard, MsalInterceptor, MsalService, MSAL_GUARD_CONFIG, MSAL_INSTANCE, MSAL_INTERCEPTOR_CONFIG } from '@azure/msal-angular';
import { MsalFactory } from './conta/factories/msal.factory';
import { APP_INITIALIZER, LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { registerLocaleData } from '@angular/common';
import { NgChartsModule } from 'ng2-charts';
import localePT from '@angular/common/locales/pt';
import * as moment from 'moment';

import { NavegacaoModule } from './navegacao/navegacao.module';
import { CoreModule } from './core/core.module';
import { DefaultInterceptor } from './interceptors/default.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { AcessoNegadoComponent } from './acesso-negado/acesso-negado.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ContaModule } from './conta/conta.module';

registerLocaleData(localePT);

@NgModule({
	declarations: [
		AppComponent,
  		AcessoNegadoComponent
	],
	imports: [
		NavegacaoModule,
		ContaModule,
		CoreModule,
		CommonModule,
		BrowserModule,
		AppRoutingModule,
		HttpClientModule,
		BrowserAnimationsModule,
		MatButtonModule,
		MatIconModule,
		MatListModule,
		MatMenuModule,
		MatSidenavModule,
		MatToolbarModule,
		FlexLayoutModule,
		MatRippleModule,
		MatNativeDateModule,
		MatInputModule,
		MatSnackBarModule,
  		NgChartsModule
	],
	providers: [
		{
			provide: HTTP_INTERCEPTORS,
			useClass: MsalInterceptor,
			multi: true
		},
		{
			provide: HTTP_INTERCEPTORS,
			useClass: DefaultInterceptor,
			multi: true
		},
		{
			provide: HTTP_INTERCEPTORS,
			useClass: ErrorInterceptor,
			multi: true
		},
		{
			provide: MSAL_INTERCEPTOR_CONFIG,
			useFactory: MsalFactory.MSALInterceptorConfigFactory
		},
		{
			provide: MSAL_INSTANCE,
			useFactory: MsalFactory.MSALInstanceFactory
		},
		{
			provide: MSAL_GUARD_CONFIG,
			useFactory: MsalFactory.MSALGuardConfigFactory
		},
		{
			provide: APP_INITIALIZER,
			useFactory: configurarLocalizacaoMoment,
			multi: true,
		},
		{
			provide: LOCALE_ID, 
			useValue: 'pt-BR'
		},
		MsalService,
		MsalGuard,
        MsalBroadcastService
	],
	bootstrap: [AppComponent]
})

export class AppModule { }

export function configurarLocalizacaoMoment(): () => Promise<void> {
	return () =>
		new Promise((resolve) => {
			moment.locale('pt-br');
			resolve();
		});
}
