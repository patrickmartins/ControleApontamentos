import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export class ValidatorWrapper {

    public static isValid(validator : ValidatorFn, errorMessage: string): ValidatorFn | null {
        return (control: AbstractControl) : ValidationErrors | null => {
            let result = validator(control);

            if(result) {
                let validationName = Object.keys(result)[0];

                if(typeof result[validationName] === "object") {
                    result[validationName].errorMessage = errorMessage;
                }
                else {
                    result[validationName] = {
                        errorMessage: errorMessage
                    }
                }
            }

            return result;            
        }
    }
}