import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './components/menu/menu.component';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BrowserModule } from '@angular/platform-browser';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { CoreModule } from '../core/core.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoaderModule } from '../loader/loader.module';
import { ContaModule } from '../conta/conta.module';
import { NgChartsModule } from 'ng2-charts';

@NgModule({
	declarations: [
		MenuComponent
	],
	imports: [
		ContaModule,
		LoaderModule,
		RouterModule,
		BrowserModule,
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatIconModule,
		MatListModule,
		MatMenuModule,
		MatSidenavModule,
		MatSlideToggleModule,
		MatToolbarModule,
		FlexLayoutModule,
		MatRippleModule,
		MatNativeDateModule,
		MatExpansionModule,
		MatInputModule,
		MatCheckboxModule,
		MatRadioModule,
		CoreModule,
        NgChartsModule	
	],
	exports: [
		MenuComponent
	]
})

export class NavegacaoModule { }
