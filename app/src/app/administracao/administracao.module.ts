import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { NgChartsModule } from 'ng2-charts';

import { ApontamentosUsuarioMesComponent } from './components/apontamentos-usuario-mes/apontamentos-usuario-mes.component';
import { LoaderModule } from '../loader/loader.module';
import { ApontamentoModule } from '../apontamento/apontamento.module';
import { CoreModule } from '../core/core.module';
import { AdministracaoRoutingModule } from './administracao-routing.module';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import { MatPseudoCheckboxModule } from '@angular/material/core';
import { ApontamentosUsuarioDiaComponent } from './components/apontamentos-usuario-dia/apontamentos-usuario-dia.component';

@NgModule({
	declarations: [
		ApontamentosUsuarioMesComponent,
        ApontamentosUsuarioDiaComponent
	],
	imports: [
        AdministracaoRoutingModule,
		LoaderModule,
		CoreModule,
		ApontamentoModule,
		CommonModule,
		LoaderModule,
		MatExpansionModule,
		MatDatepickerModule,
		MatInputModule,
		MatButtonModule,
		MatIconModule,
		MatCardModule,
		NgChartsModule,
        MatAutocompleteModule,
        MatPseudoCheckboxModule,
        FormsModule,
        ReactiveFormsModule,
	]
})
export class AdministracaoModule { }
