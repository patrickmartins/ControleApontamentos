import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
import * as moment from "moment";

export class ValidatorTimeMin {

    public static min(min: number): ValidatorFn {
        return (control: AbstractControl) : ValidationErrors | null => {
			let valor = moment.duration(control.value);
			let valido = (valor.minutes() + (valor.hours() * 60)) >= min;

			return !valido ? { min: true } : null;
        }
    }
}