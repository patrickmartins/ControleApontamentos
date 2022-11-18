import { Pipe, PipeTransform } from '@angular/core';
import { TempoHelper } from 'src/app/helpers/tempo.helper';

@Pipe({
	name: 'tempoApontado'
})
export class TempoApontadoPipe implements PipeTransform {

	public transform(value?: number, ...args: string[]): string {
		if(!value)
			return "";

		let argumentoSincronizacao = args.length > 0 ? args[0] : "";
		let argumentoApontamento = args.length > 1 ? args[1] : "apt";

		if (argumentoSincronizacao !== 'sync' && argumentoSincronizacao !== 'nsync')
			return "";

		let sincronizados = argumentoSincronizacao === 'sync';
		let apontados = argumentoApontamento === 'apt';

		let totalHoras = Math.floor(value / 60);
		let totalMinutos = value % 60;

		let resultado = TempoHelper.converterParaHorasEmExtenso(value);

		if (totalMinutos > 0) {
			if (!sincronizados) {
				resultado += apontados 
							  ?	(totalMinutos == 1 ? " apontado não sincronizado no Channel" : ` apontados não sincronizados no Channel`)
							  : (totalMinutos == 1 ? " não sincronizado no Channel" : ` não sincronizados no Channel`);
			} 
			else if (apontados){
				resultado += totalMinutos == 1 ? " apontado" : ` apontados`;
			}
		}
		else {
			if (!sincronizados) {
				resultado += apontados 
							  ? (totalHoras == 1 ? " apontada não sincronizada no Channel" : " apontadas não sincronizadas no Channel")
							  : (totalHoras == 1 ? " não sincronizada no Channel" : " não sincronizadas no Channel");
			} 
			else if (totalHoras > 0 && apontados) {
					resultado += totalHoras == 1 ? " apontada" : " apontadas";
			}
		}

		return resultado;
	}
}
