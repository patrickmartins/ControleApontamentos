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
}