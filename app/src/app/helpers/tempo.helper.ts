import * as moment from "moment";

export class TempoHelper {

	public static converterParaHorasEmExtenso(minutos: number | undefined): string {
		if(!minutos)
			return "";
			
		let resultado = "";

		let totalHoras = Math.floor(minutos / 60);
		let totalMinutos = minutos % 60;

		if (totalHoras > 0)
			resultado = totalHoras == 1 ? "1 hora" : `${totalHoras} horas`;

		if (totalMinutos > 0) {
			resultado += totalHoras > 0 ? " e " : "";
			resultado += totalMinutos == 1 ? "1 minuto" : `${totalMinutos} minutos`;	
		}

		return resultado;
	}

    public static converterParaFormatoEmExtensoCurto(minutos: number | undefined): string {
		if(!minutos)
			return "";
			
        if(minutos == 0)
            return "0";
            
		let resultado = "";

		let totalHoras = Math.floor(minutos / 60);
		let totalMinutos = minutos % 60;

		if (totalHoras > 0) {
			resultado = `${(minutos / 60).toFixed(1)} H`;
        }         
		else if (totalMinutos > 0) {
			resultado += `${totalMinutos} M`;
		}
        else if (totalHoras == 0 && totalMinutos == 0) {
            resultado = "0";
        }

		return resultado;
	}

    public static minutosParaString(minutos: number): string
    public static minutosParaString(minutos: number, format?: string): string {
        format = typeof format == 'undefined' ? 'HH:mm' : format;
		
        return moment.utc(minutos*1000).format(format);
	}

	public static stringParaMinutos(valor: any): number {
		const tempo = moment.duration(valor);

		return tempo.minutes() + (tempo.hours() * 60);
	}
}