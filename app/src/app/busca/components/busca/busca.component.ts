import { Component, Injectable, OnInit } from '@angular/core';
import { MatPaginatorIntl, PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { BaseComponent } from 'src/app/common/components/base.component';
import { PaginaBusca } from 'src/app/core/models/pagina-busca';
import { ContaService } from 'src/app/core/services/conta.service';
import { FiltroBusca } from '../../../core/models/filtro-busca';
import { TfsService } from 'src/app/core/services/tfs.service';
import { PaginatorPortugues } from 'src/app/core/configs/paginator-portugues';

@Component({
	selector: 'app-busca',
	templateUrl: './busca.component.html',
	styleUrls: ['./busca.component.scss'],
	providers: [{
		provide: MatPaginatorIntl,
		useClass: PaginatorPortugues
	}],
})
export class BuscaComponent extends BaseComponent implements OnInit {

	public carregando: boolean = true;

	public filtro: FiltroBusca = new FiltroBusca();
	public pagina: PaginaBusca = new PaginaBusca();

	constructor(servicoConta: ContaService, snackBar: MatSnackBar, private servicoTfs: TfsService, private activeRoute: ActivatedRoute, private router: Router) { 
		super(servicoConta, snackBar);
	}

	public ngOnInit() {
		this.activeRoute
			.queryParamMap
			.subscribe((mapParams: any) => {
				this.filtro = new FiltroBusca().criarNovo(mapParams.params) ?? new FiltroBusca();

				this.buscarTarefas();
			});
	}

	public onEventoPagina(evento: PageEvent) {
		this.filtro.tamanhoPagina = evento.pageSize;
		this.filtro.pagina = evento.pageIndex + 1;

		this.buscarTarefas()
	}

	private buscarTarefas() {
		this.carregando = true;

		if(this.usuarioLogado?.possuiContaTfs) {
			if (this.filtro.palavraChave !== "") {
				this.servicoTfs
					.buscarTarefas(this.filtro)
					.subscribe({
						next: (pagina) => {
							this.pagina = pagina;
						},
						error: () => this.carregando = false,
						complete: () => this.carregando = false
					})
			}
			else {
				this.router.navigate(["tarefas"]);
			}
		}
		else {
			this.carregando = false
		}
	}
}