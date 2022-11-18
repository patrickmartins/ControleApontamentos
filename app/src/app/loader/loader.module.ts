import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { ContentLoaderModule } from '@ngneat/content-loader';

import { QuadroTarefaLoaderComponent } from './components/quadro-tarefa-loader/quadro-tarefa-loader.component';
import { LoginMenuLoaderComponent } from './components/login-menu-loader/login-menu-loader.component';
import { CardLoaderComponent } from './components/card-loader/card-loader.component';
import { GraficoResumoDiaLoaderComponent } from './components/grafico-resumo-dia-loader/grafico-resumo-dia-loader.component';
import { GraficoResumoMesLoaderComponent } from './components/grafico-resumo-mes-loader/grafico-resumo-mes-loader.component';

@NgModule({
	declarations: [
		QuadroTarefaLoaderComponent,
		LoginMenuLoaderComponent,
  		CardLoaderComponent,
    	GraficoResumoDiaLoaderComponent,
     	GraficoResumoMesLoaderComponent
	],
	imports: [
		MatCardModule,
		CommonModule,
		ContentLoaderModule
	],
	exports: [
		QuadroTarefaLoaderComponent,
		LoginMenuLoaderComponent,
		CardLoaderComponent,
		GraficoResumoDiaLoaderComponent,
		GraficoResumoMesLoaderComponent
	]
})
export class LoaderModule { }
