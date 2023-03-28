import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FiltroBusca } from 'src/app/core/models/filtro-busca';
import { BaseComponent } from 'src/app/common/components/base.component';
import { StatusTarefa } from 'src/app/core/models/status-tarefa';
import { ContaService } from 'src/app/core/services/conta.service';
import { TemaService } from 'src/app/core/services/tema.service';
import { Tema } from 'src/app/core/models/configuracoes';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
	selector: 'menu',
	templateUrl: './menu.component.html',
	styleUrls: ['./menu.component.scss']
})

export class MenuComponent extends BaseComponent {

	public filtros = {
			palavraChave: "",
			colecao: this.usuarioLogado ? this.usuarioLogado.colecoes[0] : "",
			status: [
			{
				nome: "Proposto",
				status: StatusTarefa.proposto,
				marcado: false
			},
			{
				nome: "Ativo",
				status: StatusTarefa.ativo,
				marcado: true
			},
			{
				nome: "Resolvido",
				status: StatusTarefa.resolvido,
				marcado: false
			},
			{
				nome: "Fechado",
				status: StatusTarefa.fechado,
				marcado: false
			}
		]
	}

	public temaAplicacaoAtual: Tema;

	constructor(servicoConta: ContaService, snackBar: MatSnackBar, private servicoTema: TemaService, private router: Router) {
		super(servicoConta, snackBar);

		this.temaAplicacaoAtual = this.servicoTema.obterTemaAtual();
	}

	public buscarTarefas() {
		if(this.filtros.palavraChave !== "") {
			let filtroBusca = new FiltroBusca();

			filtroBusca.palavraChave = this.filtros.palavraChave;
			filtroBusca.colecao = this.filtros.colecao;
			filtroBusca.status =  this.filtros.status.filter(c => c.marcado).map(c => c.status);

			this.router.navigate(['/busca'], { queryParams: filtroBusca});		
		}
	}

	public alterarTemaAplicacao(tema: Tema) {
		this.servicoTema.alterarTemaAtual(tema);
		this.servicoTema.aplicarTemaAtual();

		this.temaAplicacaoAtual = tema;
	}
}
