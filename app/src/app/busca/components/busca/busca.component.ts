import { Component, Injectable, OnInit } from '@angular/core';
import { MatPaginatorIntl, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { PaginaBusca } from 'src/app/core/models/pagina-busca';
import { TarefaService } from 'src/app/core/services/tarefa.service';
import { FiltroBusca } from '../../../core/models/filtro-busca';

@Injectable()
export class PaginatorPortugues implements MatPaginatorIntl {
	changes = new Subject<void>();

	firstPageLabel = 'Primeira página';
	itemsPerPageLabel = 'Items por página:';
	lastPageLabel = 'Última página';
	nextPageLabel = 'Próxima página';
	previousPageLabel = 'Página anterior';

	public getRangeLabel(page: number, pageSize: number, length: number): string {
		if (length === 0) {
			return `Página 1 de 1`;
		}

		const amountPages = Math.ceil(length / pageSize);

		return `Página ${page + 1} de ${amountPages}`;
	}
}

@Component({
	selector: 'app-busca',
	templateUrl: './busca.component.html',
	styleUrls: ['./busca.component.scss'],
	providers: [{
		provide: MatPaginatorIntl,
		useClass: PaginatorPortugues
	}],
})
export class BuscaComponent implements OnInit {

	public carregando: boolean = true;

	public filtro: FiltroBusca = new FiltroBusca();
	public pagina: PaginaBusca = new PaginaBusca();

	constructor(private servicoTarefa: TarefaService, private activeRoute: ActivatedRoute, private router: Router) { }

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

		if (this.filtro.palavraChave !== "") {
			this.servicoTarefa
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
			this.router.navigate(["home"]);
		}
	}
}