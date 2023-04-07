import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DateAdapter } from '@angular/material/core';
import { FormControl } from '@angular/forms';
import { catchError, forkJoin, of } from 'rxjs';
import * as moment from 'moment';

import { ApontamentosChannelDia } from 'src/app/apontamento/models/apontamentos-channel-dia';
import { ApontamentosTfsDia } from 'src/app/apontamento/models/apontamentos-tfs-dia';
import { BatidasPontoDia } from 'src/app/apontamento/models/batidas-ponto-dia';
import { ApontamentoService } from 'src/app/apontamento/services/apontamento.service';
import { PontoService } from 'src/app/apontamento/services/ponto.service';
import { BaseComponent } from 'src/app/common/components/base.component';
import { JobInfo } from 'src/app/core/models/job-info';
import { ContaService } from 'src/app/core/services/conta.service';
import { JobService } from 'src/app/core/services/job.service';
import { UsuarioService } from '../../services/usuario.service';
import { Usuario } from 'src/app/core/models/usuario';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-apontamentos-usuario-dia',
  templateUrl: './apontamentos-usuario-dia.component.html',
  styleUrls: ['./apontamentos-usuario-dia.component.scss']
})
export class ApontamentosUsuarioDiaComponent extends BaseComponent implements OnInit {

	public carregando: boolean = true;

	public get tempoTotalTrabalhadoNoDia() : number {
		return this.batidas ? this.batidas.tempoTotalTrabalhadoNoDia : 0
	}

	public get tempoTotalApontadoNoDia() : number {
		let tempo = 0;

		if(this.apontamentosTfsDia)
			tempo = this.apontamentosTfsDia.tempoTotalApontadoNoDia;

		if(this.apontamentosChannelDia)
			tempo += this.apontamentosChannelDia.tempoTotalApontadoNoDia;

		return tempo;
	}

	public get tempoTotalApontadoSincronizadoNoDia() : number {
        return (this.apontamentosTfsDia ? this.apontamentosTfsDia.tempoTotalApontadoSincronizadoChannel : 0) + 
                    (this.apontamentosChannelDia ? this.apontamentosChannelDia.tempoTotalApontadoNoDia : 0);
	}

	public get tempoTotalApontadoNaoSincronizadoNoDia() : number {
		return this.apontamentosTfsDia ? this.apontamentosTfsDia.tempoTotalApontadoNaoSincronizadoChannel : 0;
	}

	public dataAtual: Date = new Date();
	public dataSelecionada: Date = new Date();

	public apontamentosTfsDia?: ApontamentosTfsDia;
	public apontamentosChannelDia?: ApontamentosChannelDia;
	public batidas?: BatidasPontoDia;

	public infoJobCarga?: JobInfo;
    public formUsuario = new FormControl();
    public formDataSelecionada = new FormControl();

    public usuarios: Usuario[] = [];    
    public usuariosFiltrado: Usuario[] = [];
    public usuarioSelecionado?: Usuario;
    
	constructor(servicoConta: ContaService,
        snackBar: MatSnackBar,
		private servicoApontamento: ApontamentoService,
		private servicoPonto: PontoService,
        private usuarioService: UsuarioService,
		private servicoJob: JobService,
		private dataAdapter: DateAdapter<any>,
		private activeRoute: ActivatedRoute, 
		private router: Router) {

		super(servicoConta, snackBar);
		this.dataAdapter.setLocale('pt-br');
	}
	
	public ngOnInit(): void {
        this.usuarioService.obterTodosUsuarios().subscribe(usuarios => { 
            this.usuariosFiltrado = this.usuarios = usuarios;

            this.activeRoute
                .queryParamMap
                .subscribe((mapParams: any) => {			
                    let data = moment(mapParams.params.data, 'DD-MM-YYYY');

                    this.usuarioSelecionado = this.usuariosFiltrado.find(c => c.id == mapParams.params.usuario);
                    this.dataSelecionada = data.isValid() ? data.toDate() : new Date();

                    if(this.usuarioSelecionado) {
                        this.formUsuario.setValue(this.usuarioSelecionado, { emitEvent: false });
                        this.formDataSelecionada.setValue(this.dataSelecionada, { emitEvent: false });
                        
                        this.obterBatidasEApontamentosPorDia(this.usuarioSelecionado.id, this.dataSelecionada);
                    }
                });		
        });

        this.formUsuario.valueChanges.subscribe(valor => {
            if(typeof valor === 'string') {
                this.usuariosFiltrado = this.filtrarUsuarios(valor || '')
            }
            else {
                this.usuarioSelecionado = valor;
                
                this.router
                    .navigate([],
                        {
                            queryParams:
                            {
                                usuario: this.usuarioSelecionado?.id
                            }
                        });
            }
        });
	}

	public onDataAlterada(data: Date): void {
		this.dataSelecionada = data;

		this.router
            .navigate([], 
            { 
                queryParams: 
                { 
                    data: moment(this.dataSelecionada).format('DD-MM-YYYY'),
                    usuario: this.usuarioSelecionado?.id 
                }
            });
	}

	public obterBatidasEApontamentosPorDia(idUsuario: string, data: Date): void {
		this.apontamentosChannelDia = undefined;
		this.apontamentosTfsDia = undefined;

		this.carregando = true;

        forkJoin({
            apontamentosTfsDia: this.usuarioSelecionado?.possuiContaTfs ? this.servicoApontamento.obterApontamentosTfsDeUsuarioPorDia(idUsuario, data).pipe(catchError(e => this.pipeErrosDeNegocio(e))) : of(undefined),
            apontamentosChannelDia: this.usuarioSelecionado?.possuiContaChannel ? this.servicoApontamento.obterApontamentosChannelDeUsuarioPorDia(idUsuario, data).pipe(catchError(e => this.pipeErrosDeNegocio(e))) : of(undefined),
            batidas: this.usuarioSelecionado?.possuiContaPonto ? this.servicoPonto.obterBatidasDeUsuarioPorDia(idUsuario, data).pipe(catchError(e => this.pipeErrosDeNegocio(e))) : of(undefined),
            infoJobCarga: this.servicoJob.obterJobCarga()
        })		
        .subscribe({
            next: (resultado: any) => {
                this.apontamentosTfsDia = resultado.apontamentosTfsDia;
                this.apontamentosChannelDia = resultado.apontamentosChannelDia;
                this.batidas = resultado.batidas;
                this.infoJobCarga = resultado.infoJobCarga;

                this.servicoApontamento.consolidarTarefasEAtividades(this.apontamentosTfsDia, this.apontamentosChannelDia);
            },
            complete: () => this.carregando = false
        });
	}

	public eHoje(data: Date): boolean {
		const hoje = new Date()

		return data.getDate() == hoje.getDate() &&
			data.getMonth() == hoje.getMonth() &&
			data.getFullYear() == hoje.getFullYear()
	}
    
    public obterNomeCompletoUsuario(usuario: Usuario): string {
        return usuario ? usuario.nomeCompleto : "";
    }

    private filtrarUsuarios(nome: string): Usuario[] {
        return this.usuarios.filter(c => c.nomeCompleto.toLowerCase().includes(nome.toLowerCase()));
    }
}
