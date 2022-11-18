import { Pipe, PipeTransform } from '@angular/core';
import { TempoHelper } from 'src/app/helpers/tempo.helper';

@Pipe({
	name: 'tempo'
})
export class TempoPipe implements PipeTransform {

	public transform(value?: number, ...args: string[]): string {
		if(!value)
			return "";
			
		return TempoHelper.converterParaHorasEmExtenso(value);
	}
}
