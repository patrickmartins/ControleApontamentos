import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorizeGuard } from '../core/guards/authorize.guard';
import { ApontamentosPorDiaComponent } from './components/apontamentos-por-dia/apontamentos-por-dia.component';
import { ApontamentosPorMesComponent } from './components/apontamentos-por-mes/apontamentos-por-mes.component';

const routes: Routes = [	
	{ path: 'por-dia', component: ApontamentosPorDiaComponent, canActivate: [AuthorizeGuard] },
	{ path: 'por-mes', component: ApontamentosPorMesComponent, canActivate: [AuthorizeGuard] },
	{ path: '', redirectTo: '', pathMatch: 'full'}
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ApontamentoRoutingModule { }
