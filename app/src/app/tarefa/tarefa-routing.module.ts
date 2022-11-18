import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorizeGuard } from '../core/guards/authorize.guard';
import { MinhasTerefasComponent } from './components/minhas-terefas/minhas-terefas.component';

const routes: Routes = [	
	{ path: 'minhas-tarefas', component: MinhasTerefasComponent, canActivate: [AuthorizeGuard] },
	{ path: '', redirectTo: 'minhas-tarefas', pathMatch: 'full'}
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class TarefaRoutingModule { }
