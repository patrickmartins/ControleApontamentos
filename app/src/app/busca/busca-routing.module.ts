import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthorizeGuard } from '../core/guards/authorize.guard';
import { BuscaComponent } from './components/busca/busca.component';

const routes: Routes = [
	{ path: '', component: BuscaComponent, canActivate: [AuthorizeGuard]}
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class BuscaRoutingModule { }