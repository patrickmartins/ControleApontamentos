import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BuscaComponent } from './components/busca/busca.component';
import { BuscaRoutingModule } from './busca-routing.module';
import { CoreModule } from '../core/core.module';
import { MatPaginatorModule } from '@angular/material/paginator';
import { LoaderModule } from '../loader/loader.module';
import { MatCardModule } from '@angular/material/card';

@NgModule({
	declarations: [
		BuscaComponent
	],
	imports: [
		CommonModule,
		CoreModule,
		LoaderModule,
		BuscaRoutingModule,
		MatPaginatorModule,
		MatCardModule
	]
})
export class BuscaModule { }
