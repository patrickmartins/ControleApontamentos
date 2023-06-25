import { IModel } from "src/app/common/models/model";
import { Usuario } from "src/app/core/models/usuario";
import { SituacaoApontamentos } from "./situacao-apontamentos";

export class RelatorioApontamentosUsuarioPorMes implements IModel<RelatorioApontamentosUsuarioPorMes> {

    public mesReferencia: number = 0;
    public anoReferencia: number = 0;
    public tempoTotalTrabalhadoNoMes: number = 0;
    public tempoTotalApontadoNoChannelNoMes: number = 0;
    public tempoTotalApontadoNaoSincronizadoNoTfsNoMes: number = 0;
    public diferencaTempoTrabalhadoApontado: number = 0;

	public usuario!: Usuario;

	constructor() { }

    public criarNovo(params: any): RelatorioApontamentosUsuarioPorMes | undefined {
        if (!params)
            return undefined;

        let relatorio = new RelatorioApontamentosUsuarioPorMes();

        if (params) {
            relatorio.mesReferencia = params.mesReferencia;
            relatorio.anoReferencia = params.anoReferencia;
            relatorio.tempoTotalTrabalhadoNoMes = params.tempoTotalTrabalhadoNoMes;
            relatorio.tempoTotalApontadoNoChannelNoMes = params.tempoTotalApontadoNoChannelNoMes;
            relatorio.tempoTotalApontadoNaoSincronizadoNoTfsNoMes = params.tempoTotalApontadoNaoSincronizadoNoTfsNoMes;
            relatorio.diferencaTempoTrabalhadoApontado = params.diferencaTempoTrabalhadoApontado;

            relatorio.usuario = new Usuario().criarNovo(params.usuario)!;
        }

        return relatorio;
    }

    public calcularSituacao(tolerancia: number): SituacaoApontamentos {
        if(this.diferencaTempoTrabalhadoApontado > 0) {
            return this.diferencaTempoTrabalhadoApontado <= tolerancia ? SituacaoApontamentos.Ok : SituacaoApontamentos.Verificar;
        }
        else {
            return (this.diferencaTempoTrabalhadoApontado * -1) <= tolerancia ? SituacaoApontamentos.Ok : SituacaoApontamentos.Verificar;
        }            
    }
}