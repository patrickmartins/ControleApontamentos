import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorizeGuard } from '../core/guards/authorize.guard';
import { ApontamentosUsuarioDiaComponent } from './components/apontamentos-usuario-dia/apontamentos-usuario-dia.component';
import { ApontamentosUsuarioMesComponent } from './components/apontamentos-usuario-mes/apontamentos-usuario-mes.component';
import { UsuariosComponent } from './components/usuarios/usuarios.component';
import { RelatorioApontamentosComponent } from './components/relatorio-apontamentos/relatorio-apontamentos.component';

const routes: Routes = [		
	{ path: 'usuario/apontamento/por-mes', component: ApontamentosUsuarioMesComponent, canActivate: [AuthorizeGuard], data: { autorizado: true, anonimo: false } },
    { path: 'usuario/apontamento/por-dia', component: ApontamentosUsuarioDiaComponent, canActivate: [AuthorizeGuard], data: { autorizado: true, anonimo: false } },
    { path: 'relatorio/apontamentos-por-mes', component: RelatorioApontamentosComponent, canActivate: [AuthorizeGuard], data: { autorizado: true, anonimo: false } },
    { path: 'usuario/apontamento', redirectTo: 'usuario/apontamento/por-dia', pathMatch: 'full' },
    { path: 'usuario', component: UsuariosComponent, canActivate: [AuthorizeGuard], data: { autorizado: true, anonimo: false } },
	{ path: '', redirectTo: 'usuario/apontamento/por-dia', pathMatch: 'full'}
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class AdministracaoRoutingModule { }
