import { NgModule } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { CommonModule } from '@angular/common';
import {MatButtonModule} from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

import { TarefaRoutingModule } from './tarefa-routing.module';
import { MinhasTerefasComponent } from './components/minhas-terefas/minhas-terefas.component';
import { CoreModule } from '../core/core.module';
import { LoaderModule } from '../loader/loader.module';

@NgModule({
	declarations: [		
 		MinhasTerefasComponent
	],
	imports: [		
		CommonModule,
		CoreModule,
		TarefaRoutingModule,
		MatExpansionModule,
		MatButtonModule,
		MatCardModule,
		LoaderModule
	]
})

export class TarefaModule { }