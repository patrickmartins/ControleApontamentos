import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AcessoNegadoComponent } from './acesso-negado/acesso-negado.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'home', redirectTo: '', pathMatch: 'full' },
	{ path: 'acesso-negado', component: AcessoNegadoComponent },
	{ path: 'tarefa', loadChildren: () => import('./tarefa/tarefa.module').then(m => m.TarefaModule) },
	{ path: 'apontamento', loadChildren: () => import('./apontamento/apontamento.module').then(m => m.ApontamentoModule) },
	{ path: 'busca', loadChildren: () => import('./busca/busca.module').then(m => m.BuscaModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
