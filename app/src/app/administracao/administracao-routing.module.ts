import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorizeGuard } from '../core/guards/authorize.guard';
import { ApontamentosUsuarioDiaComponent } from './components/apontamentos-usuario-dia/apontamentos-usuario-dia.component';
import { ApontamentosUsuarioMesComponent } from './components/apontamentos-usuario-mes/apontamentos-usuario-mes.component';

const routes: Routes = [		
	{ path: 'apontamentos-usuario/por-mes', component: ApontamentosUsuarioMesComponent, canActivate: [AuthorizeGuard], data: { autorizado: true, anonimo: false } },
    { path: 'apontamentos-usuario/por-dia', component: ApontamentosUsuarioDiaComponent, canActivate: [AuthorizeGuard], data: { autorizado: true, anonimo: false } },
    { path: 'apontamentos-usuario', redirectTo: 'apontamentos-usuario/por-dia', pathMatch: 'full' },
	{ path: '', redirectTo: 'apontamentos-usuario/por-dia', pathMatch: 'full'}
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class AdministracaoRoutingModule { }
