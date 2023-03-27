import { Component, NgZone, OnInit, ViewChild } from '@angular/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { ActivatedRoute, Router } from '@angular/router';
import { DateAdapter } from '@angular/material/core';
import * as moment from 'moment';
import { forkJoin, map, of, startWith, tap } from 'rxjs';

import { ApontamentosChannelDia } from 'src/app/apontamento/models/apontamentos-channel-dia';
import { ApontamentosChannelMes } from 'src/app/apontamento/models/apontamentos-channel-mes';
import { ApontamentosTfsDia } from 'src/app/apontamento/models/apontamentos-tfs-dia';
import { ApontamentosTfsMes } from 'src/app/apontamento/models/apontamentos-tfs-mes';
import { BatidasPontoMes } from 'src/app/apontamento/models/batidas-ponto-mes';
import { ApontamentoService } from 'src/app/apontamento/services/apontamento.service';
import { PontoService } from 'src/app/apontamento/services/ponto.service';
import { BaseComponent } from 'src/app/common/components/base.component';
import { JobInfo } from 'src/app/core/models/job-info';
import { ContaService } from 'src/app/core/services/conta.service';
import { JobService } from 'src/app/core/services/job.service';
import { FormControl } from '@angular/forms';
import { UsuarioService } from '../../services/usuario.service';
import { Usuario } from 'src/app/core/models/usuario';

@Component({
    selector: 'app-apontamentos-usuario-mes',
    templateUrl: './apontamentos-usuario-mes.component.html',
    styleUrls: ['./apontamentos-usuario-mes.component.scss']
})
export class ApontamentosUsuarioMesComponent extends BaseComponent implements OnInit {

    public carregando: boolean = true;

    public get tempoTotalTrabalhadoNoMes(): number {
        return this.batidas ? this.batidas.tempoTotalTrabalhadoNoMes : 0
    }

    public get tempoTotalApontadoNoMes(): number {
        let tempo = 0;

        if (this.apontamentosTfsMes)
            tempo = this.apontamentosTfsMes.tempoTotalApontadoNoMes;

        if (this.apontamentosChannelMes)
            tempo += this.apontamentosChannelMes.tempoTotalApontadoNoMes;

        return tempo;
    }

    public get tempoTotalApontadoSincronizadoNoMes(): number {
        return (this.apontamentosTfsMes ? this.apontamentosTfsMes.tempoTotalApontadoSincronizadoChannel : 0) +
            (this.apontamentosChannelMes ? this.apontamentosChannelMes.tempoTotalApontadoNoMes : 0);
    }

    public get tempoTotalApontadoNaoSincronizadoNoMes(): number {
        return this.apontamentosTfsMes ? this.apontamentosTfsMes.tempoTotalApontadoNaoSincronizadoChannel : 0;
    }

    public dataAtual: Date = new Date();

    public mesSelecionado: Date = new Date();
    public diaSelecionado: Date = new Date();

    public apontamentosTfsDiaSelecionado?: ApontamentosTfsDia;
    public apontamentosChannelDiaSelecionado?: ApontamentosChannelDia;

    public apontamentosTfsMes?: ApontamentosTfsMes;
    public apontamentosChannelMes?: ApontamentosChannelMes;
    public batidas?: BatidasPontoMes;

    public infoJobCarga?: JobInfo;
    public formUsuario = new FormControl();
    public formDataSelecionada = new FormControl();
    
    public usuarios: Usuario[] = [];    
    public usuariosFiltrado: Usuario[] = [];
    public usuarioSelecionado?: Usuario;

    constructor(servicoConta: ContaService,
        private servicoApontamento: ApontamentoService,
        private servicoPonto: PontoService,
        private usuarioService: UsuarioService,
        private servicoJob: JobService,
        private dataAdapter: DateAdapter<any>,
        private zone: NgZone,
        private activeRoute: ActivatedRoute,
        private router: Router) {

        super(servicoConta);

        this.dataAdapter.setLocale('pt-br');
    }

    public ngOnInit(): void {
        this.usuarioService.obterTodosUsuarios().subscribe(usuarios => { 
            this.usuariosFiltrado = this.usuarios = usuarios;

            this.activeRoute
                .queryParamMap
                .subscribe((mapParams: any) => {
                    let data = moment(`01-${mapParams.params.mes}-${mapParams.params.ano}`, 'DD-MM-YYYY');
                    
                    this.usuarioSelecionado = this.usuariosFiltrado.find(c => c.id == mapParams.params.usuario);

                    this.mesSelecionado = data.isValid() ? data.toDate() : new Date();

                    if(this.usuarioSelecionado) {
                        this.formUsuario.setValue(this.usuarioSelecionado, { emitEvent: false });

                        this.obterBatidasEApontamentosPorMes(this.usuarioSelecionado.id, this.mesSelecionado);
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

    public onDataAlterada(data: Date, picker: MatDatepicker<any>): void {
        picker.close();

        this.mesSelecionado = data;

        this.router
            .navigate([],
                {
                    queryParams:
                    {
                        mes: data.getMonth() + 1,
                        ano: data.getFullYear(),
                        usuario: this.usuarioSelecionado?.id
                    }
                });
    }

    public onDiaClicado(dia: number) {
        this.zone.run(() => {
            this.diaSelecionado = new Date(this.mesSelecionado.getFullYear(), this.mesSelecionado.getMonth(), dia);

            this.apontamentosTfsDiaSelecionado = this.apontamentosTfsMes?.obterApontamentosPorDia(dia);
            this.apontamentosChannelDiaSelecionado = this.apontamentosChannelMes?.obterApontamentosPorDia(dia);
        });
    }

    public obterBatidasEApontamentosPorMes(idUsuario: string, dataReferencia: Date): void {
        this.apontamentosChannelMes = undefined;
        this.apontamentosTfsMes = undefined;

        this.carregando = true;

        let mes = dataReferencia.getMonth() + 1;
        let ano = dataReferencia.getFullYear();

        forkJoin({
            apontamentosTfsMes: !this.usuarioSelecionado?.possuiContaTfs ? of(undefined) : this.servicoApontamento.obterApontamentosTfsDeUsuarioPorMes(idUsuario, mes, ano),
            apontamentosChannelMes: !this.usuarioSelecionado?.possuiContaChannel ? of(undefined) : this.servicoApontamento.obterApontamentosChannelDeUsuarioPorMes(idUsuario, mes, ano),
            batidas: !this.usuarioSelecionado?.possuiContaPonto ? of(undefined) : this.servicoPonto.obterBatidasDeUsuarioPorMes(idUsuario, mes, ano),
            infoJobCarga: this.servicoJob.obterJobCarga()
        })		
        .subscribe({ 
            next: (resultado: any) => {
                this.apontamentosTfsMes = resultado.apontamentosTfsMes;
                this.apontamentosChannelMes = resultado.apontamentosChannelMes;
                this.batidas = resultado.batidas;
                this.infoJobCarga = resultado.jobInfo;

                this.servicoApontamento.consolidarTarefasEAtividadesMes(this.apontamentosTfsMes, this.apontamentosChannelMes);

                this.selecionarUltimosApontamentos();
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

    private selecionarUltimosApontamentos(): void {
        let dataUltimoApontamentoTfs: Date | undefined;
        let dataUltimoApontamentoChannel: Date | undefined;

        if (this.apontamentosTfsMes) {
            let apontamentosTfsDiaSelecionado = this.apontamentosTfsMes.obterApontamentosDoUltimoDia();

            dataUltimoApontamentoTfs = apontamentosTfsDiaSelecionado ? apontamentosTfsDiaSelecionado.dataReferencia : undefined;
        }

        if (this.apontamentosChannelMes) {
            let apontamentosChannelDiaSelecionado = this.apontamentosChannelMes.obterApontamentosDoUltimoDia();

            dataUltimoApontamentoChannel = apontamentosChannelDiaSelecionado ? apontamentosChannelDiaSelecionado.dataReferencia : undefined;
        }

        if (dataUltimoApontamentoTfs && dataUltimoApontamentoChannel)
            this.diaSelecionado = dataUltimoApontamentoChannel > dataUltimoApontamentoTfs ? dataUltimoApontamentoChannel : dataUltimoApontamentoTfs;

        if (!dataUltimoApontamentoTfs)
            this.diaSelecionado = dataUltimoApontamentoChannel!;

        if (!dataUltimoApontamentoChannel)
            this.diaSelecionado = dataUltimoApontamentoTfs!;

        if (!dataUltimoApontamentoTfs && !dataUltimoApontamentoChannel)
            this.diaSelecionado = new Date();

        if (this.diaSelecionado)
            this.onDiaClicado(this.diaSelecionado.getDate());
    }

    public obterNomeCompletoUsuario(usuario: Usuario): string {
        return usuario ? usuario.nomeCompleto : "";
    }

    private filtrarUsuarios(nome: string): Usuario[] {
        return this.usuarios.filter(c => c.nomeCompleto.toLowerCase().includes(nome.toLowerCase()));
    }
}
