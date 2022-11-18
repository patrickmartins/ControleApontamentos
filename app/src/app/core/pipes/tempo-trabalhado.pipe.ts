import { Pipe, PipeTransform } from '@angular/core';
import { TempoHelper } from 'src/app/helpers/tempo.helper';

@Pipe({
	name: 'tempoTrabalhado'
})

export class TempoTrabalhadoPipe implements PipeTransform {

	public transform(value?: number, ...args: string[]): string {
		if(!value)
			return "";

		let totalHoras = Math.floor(value / 60);
		let totalMinutos = value % 60;

		let resultado = TempoHelper.converterParaHorasEmExtenso(value);

		if (totalMinutos > 0) {
			resultado += totalMinutos == 1 ? " trabalhado" : ` trabalhados`;
		}
		else {
			if (totalHoras > 0)
				resultado += totalHoras == 1 ? " trabalhada" : " trabalhadas";		
		}

		return resultado;
	}
}
