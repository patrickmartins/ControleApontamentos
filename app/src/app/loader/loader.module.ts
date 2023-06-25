import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { ContentLoaderModule } from '@ngneat/content-loader';

import { QuadroTarefaLoaderComponent } from './components/quadro-tarefa-loader/quadro-tarefa-loader.component';
import { LoginMenuLoaderComponent } from './components/login-menu-loader/login-menu-loader.component';
import { CardLoaderComponent } from './components/card-loader/card-loader.component';
import { GraficoResumoDiaLoaderComponent } from './components/grafico-resumo-dia-loader/grafico-resumo-dia-loader.component';
import { GraficoResumoMesLoaderComponent } from './components/grafico-resumo-mes-loader/grafico-resumo-mes-loader.component';
import { UsuariosLoaderComponent } from './components/usuarios-loader/usuarios-loader.component';
import { QuadroRelatorioApontamentosLoaderComponent } from './components/quadro-relatorio-apontamentos-loader/quadro-relatorio-apontamentos-loader.component';
import { GridRelatorioApontamentosLoaderComponent } from './components/grid-relatorio-apontamentos-loader/grid-relatorio-apontamentos-loader.component';

@NgModule({
	declarations: [
		QuadroTarefaLoaderComponent,
		LoginMenuLoaderComponent,
  		CardLoaderComponent,
    	GraficoResumoDiaLoaderComponent,
     	GraficoResumoMesLoaderComponent,
        UsuariosLoaderComponent,
        QuadroRelatorioApontamentosLoaderComponent,
        GridRelatorioApontamentosLoaderComponent
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
		GraficoResumoMesLoaderComponent,
        UsuariosLoaderComponent,
        QuadroRelatorioApontamentosLoaderComponent,
        GridRelatorioApontamentosLoaderComponent
	]
})
export class LoaderModule { }
