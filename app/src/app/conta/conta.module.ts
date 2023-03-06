import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FlexLayoutModule } from "@angular/flex-layout";
import { MatButtonModule } from "@angular/material/button";
import { MatRippleModule } from "@angular/material/core";
import { MatIconModule } from "@angular/material/icon";
import { MatMenuModule } from "@angular/material/menu";

import { LoaderModule } from "../loader/loader.module";
import { LoginMenuComponent } from "./components/login-menu/login-menu.component";
import { LoginComponent } from './components/login/login.component';

@NgModule({
	declarations: [
		LoginMenuComponent,
  		LoginComponent
	],
	imports: [
		LoaderModule,
		CommonModule,
		MatButtonModule,
		MatIconModule,
		MatMenuModule,
		FlexLayoutModule,
		MatRippleModule
	],
	exports: [		
		LoginMenuComponent
	]
})

export class ContaModule { }
