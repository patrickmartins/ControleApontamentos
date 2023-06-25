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
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatPseudoCheckboxModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSortModule } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatRippleModule } from '@angular/material/core';
import { MatButtonToggleModule } from '@angular/material/button-toggle';

import { LoaderModule } from '../loader/loader.module';
import { ApontamentoModule } from '../apontamento/apontamento.module';
import { CoreModule } from '../core/core.module';
import { AdministracaoRoutingModule } from './administracao-routing.module';
import { ApontamentosUsuarioDiaComponent } from './components/apontamentos-usuario-dia/apontamentos-usuario-dia.component';
import { UsuariosComponent } from './components/usuarios/usuarios.component';
import { RelatorioApontamentosComponent } from './components/relatorio-apontamentos/relatorio-apontamentos.component';
import { QuadroRelatorioApontamentosComponent } from './components/quadro-relatorio-apontamentos/quadro-relatorio-apontamentos.component';
import { ModalSalvarUsuarioComponent } from './components/modal-salvar-usuario/modal-salvar-usuario.component';
import { ApontamentosUsuarioMesComponent } from './components/apontamentos-usuario-mes/apontamentos-usuario-mes.component';
import { GridRelatorioApontamentosComponent } from './components/grid-relatorio-apontamentos/grid-relatorio-apontamentos.component';

@NgModule({
	declarations: [
		ApontamentosUsuarioMesComponent,
        ApontamentosUsuarioDiaComponent,
        UsuariosComponent,
        ModalSalvarUsuarioComponent,
        RelatorioApontamentosComponent,
        QuadroRelatorioApontamentosComponent,
        GridRelatorioApontamentosComponent
	],
	imports: [        
        AdministracaoRoutingModule,
		LoaderModule,
		CoreModule,
		ApontamentoModule,
		CommonModule,
		MatExpansionModule,
		MatDatepickerModule,
		MatInputModule,
		MatButtonModule,
		MatIconModule,
		MatCardModule,
        MatCheckboxModule,
        MatTableModule,
		NgChartsModule,
        MatDialogModule,
        MatDividerModule,
        MatFormFieldModule,
        MatSelectModule,
        MatProgressSpinnerModule,
        MatAutocompleteModule,
        MatPseudoCheckboxModule,   
        MatPaginatorModule,   
        MatSortModule,  
        MatRippleModule,
        MatButtonToggleModule,
        FormsModule,
        ReactiveFormsModule,
	]
})
export class AdministracaoModule { }
