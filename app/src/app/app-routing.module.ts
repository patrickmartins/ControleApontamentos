import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AcessoNegadoComponent } from './acesso-negado/acesso-negado.component';
import { LoginComponent } from './conta/components/login/login.component';
import { AuthorizeGuard } from './core/guards/authorize.guard';

const routes: Routes = [
	{ path: 'acesso-negado', component: AcessoNegadoComponent },
	{ path: 'login', component: LoginComponent, canActivate: [AuthorizeGuard], data: { autorizado: false, anonimo: true } },
	{ path: 'tarefa', loadChildren: () => import('./tarefa/tarefa.module').then(m => m.TarefaModule) },
	{ path: 'apontamento', loadChildren: () => import('./apontamento/apontamento.module').then(m => m.ApontamentoModule) },
    { path: 'administracao', loadChildren: () => import('./administracao/administracao.module').then(m => m.AdministracaoModule) },
	{ path: 'busca', loadChildren: () => import('./busca/busca.module').then(m => m.BuscaModule) },
	{ path: '**', redirectTo: 'tarefa' }
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})

export class AppRoutingModule { }
