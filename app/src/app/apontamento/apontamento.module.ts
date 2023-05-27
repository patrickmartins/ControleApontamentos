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
import { MatCardModule } from '@angular/material/card';
import annotationPlugin from 'chartjs-plugin-annotation';
import { Chart } from 'chart.js';

import { ApontamentosPorDiaComponent } from './components/apontamentos-por-dia/apontamentos-por-dia.component';
import { ApontamentoRoutingModule } from './apontamento-routing.module';
import { GraficoResumoDiaComponent } from './components/grafico-resumo-dia/grafico-resumo-dia.component';
import { ApontamentosPorMesComponent } from './components/apontamentos-por-mes/apontamentos-por-mes.component';
import { GraficoResumoMesComponent } from './components/grafico-resumo-mes/grafico-resumo-mes.component';
import { LoaderModule } from '../loader/loader.module';
import { PontoService } from './services/ponto.service';
import { ResumoApontamentosComponent } from './components/resumo-apontamentos/resumo-apontamentos.component';

@NgModule({
    declarations: [
        ApontamentosPorDiaComponent,
        ApontamentosPorMesComponent,
        GraficoResumoDiaComponent,
        GraficoResumoMesComponent,
        ResumoApontamentosComponent
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
    ],
    exports: [
        GraficoResumoDiaComponent,
        GraficoResumoMesComponent,
        ResumoApontamentosComponent
    ],
    providers: [        
        PontoService
    ]
})
export class ApontamentoModule {

    constructor() {
        Chart.register(annotationPlugin);        
    }
}
