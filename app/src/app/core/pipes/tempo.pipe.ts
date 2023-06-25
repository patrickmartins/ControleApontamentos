import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
import { TempoHelper } from 'src/app/helpers/tempo.helper';

@Pipe({
	name: 'tempo'
})
export class TempoPipe implements PipeTransform {

	public transform(value?: number, ...args: string[]): string {
        let tipo = "extenso-completo";
        let formato = "HH:mm";

        if(value == null || typeof value == 'undefined')
            return "";

        if(args && args.length > 0) {
            tipo = args[0];
            
            if(args.length > 1)
                formato = args[1];
        }

        if(tipo != "extenso-completo" && tipo != "extenso-curto" && tipo != "customizado")
            tipo = "extenso-completo";
			
        if(tipo == "customizado")
            return moment.duration(value, "minutes").format(formato, { trim: 'mid'});
        
        if(tipo == "extenso-completo")
		    return TempoHelper.converterParaHorasEmExtenso(value);
        
        return TempoHelper.converterParaFormatoEmExtensoCurto(value);
	}
}
