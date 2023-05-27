import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatRippleModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { TimerModule } from 'src/libs/timer/timer.module';

import { ContadorTarefaComponent } from './components/contador-tarefa/contador-tarefa.component';
import { DesignadoTarefaComponent } from './components/designado-tarefa/designado-tarefa.component';
import { ModalSalvarApontamentoComponent } from './components/modal-salvar-apontamento/modal-salvar-apontamento.component';
import { QuadroAtividadeComponent } from './components/quadro-atividade/quadro-atividade.component';
import { QuadroTarefaComponent } from './components/quadro-tarefa/quadro-tarefa.component';
import { TempoApontadoPipe } from './pipes/tempo-apontado.pipe';
import { TempoTrabalhadoPipe } from './pipes/tempo-trabalhado.pipe';
import { TempoPipe } from './pipes/tempo.pipe';
import { ContaService } from './services/conta.service';
import { JobService } from './services/job.service';
import { TemaService } from './services/tema.service';
import { ConsolidacaoService } from './services/consolidacao.service';
import { TfsService } from './services/tfs.service';
import { ChannelService } from './services/channel.service';

@NgModule({
	declarations: [		
		ModalSalvarApontamentoComponent,
		ContadorTarefaComponent,
		QuadroTarefaComponent,
		QuadroAtividadeComponent,
		DesignadoTarefaComponent,		
		TempoApontadoPipe,
		TempoTrabalhadoPipe,
		TempoPipe  		  				  		
	],
	imports: [
		CommonModule,
		MatExpansionModule,
		MatRippleModule,
		FormsModule,	
		ReactiveFormsModule,
		MatCardModule,		
		MatButtonModule,		
		MatIconModule,
		MatDatepickerModule,
		TimerModule,
		MatDialogModule,
		MatInputModule,
		MatDividerModule,
		MatProgressBarModule	
	],
	providers: [
		ContaService,
        TfsService,
        ChannelService,
        ConsolidacaoService,
		JobService,
		TemaService
	],
	exports: [
		ModalSalvarApontamentoComponent,
		ContadorTarefaComponent,
		QuadroTarefaComponent,
		QuadroAtividadeComponent,
		DesignadoTarefaComponent,			
		TempoApontadoPipe,
		TempoTrabalhadoPipe,
		TempoPipe
	]
})

export class CoreModule { }

