import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CoreModule } from '../core/core.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { NgChartsModule } from 'ng2-charts';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { ApontamentosPorDiaComponent } from './components/apontamentos-por-dia/apontamentos-por-dia.component';
import { ApontamentoRoutingModule } from './apontamento-routing.module';
import { GraficoResumoDiaComponent } from './components/grafico-resumo-dia/grafico-resumo-dia.component';
import { ApontamentosPorMesComponent } from './components/apontamentos-por-mes/apontamentos-por-mes.component';
import { GraficoResumoMesComponent } from './components/grafico-resumo-mes/grafico-resumo-mes.component';
import { MatCardModule } from '@angular/material/card';
import { LoaderModule } from '../loader/loader.module';

@NgModule({
	declarations: [
		ApontamentosPorDiaComponent,
		ApontamentosPorMesComponent,
		GraficoResumoDiaComponent,
		GraficoResumoMesComponent
	],
	imports: [
		CoreModule,
		CommonModule,
		FormsModule,
		LoaderModule,
		ReactiveFormsModule,
		ApontamentoRoutingModule,
		MatExpansionModule,
		MatDatepickerModule,
		MatInputModule,
		MatButtonModule,
		MatIconModule,
		MatCardModule,
		NgChartsModule
	]
})
export class ApontamentoModule { }
