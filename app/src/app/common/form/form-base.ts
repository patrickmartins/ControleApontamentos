import { FormGroup } from "@angular/forms";
import { filter } from "rxjs/operators";

import { Erro } from "../models/erro";
import { FormModel } from "../models/form.model";

export class FormBase<TModel extends FormModel<TModel>> {
    public formGroup!: FormGroup;
    public errors: { [source: string]: Erro[] } = { };

    private model!: TModel;    
    private status: string = "INVALID";

    constructor(modelType: (new () => TModel)) {
		this.model = new modelType();
		this.formGroup = this.model.toForm();

		this.subscribeFormStatusChange(this.formGroup);	
    }

	public setModel(model: TModel) {
		this.model = model;
		this.formGroup = model.toForm();
		this.errors = this.getFormValidationErrors(this.formGroup);

		this.subscribeFormStatusChange(this.formGroup);		
	}

    public getModel(): TModel {
        return this.model.toModel(this.formGroup);
    }

    public clearValidationErrors(){
        this.errors = { };
    }

    public addValidationErrors(errors: Erro[]) {
        errors.forEach(error => {
            this.addValidationError(error);
        });
    }

    public addValidationError(error: Erro) {
        let key = error.origem.toLowerCase();

        this.setFormValidationErrors(this.formGroup, [error]);

        if(this.errors[key]) {
            this.errors[key].push(error);
        }
        else {
            this.errors[key] = [error];
        }
    }

	public isValid(): boolean {
		return this.formGroup.valid;
	}

	private subscribeFormStatusChange(formGroup: FormGroup) {
		formGroup.statusChanges
			.pipe(filter((status) => !(this.formGroup.status == "VALID" && this.status == "VALID") && (status == "VALID" || status == "INVALID")))
			.subscribe(status => this.onStatusChanges(status));
	}

    private onStatusChanges(status: string) {                
        this.errors = this.getFormValidationErrors(this.formGroup);
        this.status = status;
    }

    private getFormValidationErrors(form: FormGroup): { [source: string]: Erro[] } {
        let errors: { [source: string]: Erro[] } = { };

        Object.keys(form.controls).forEach(input => {
            if(form.controls[input].errors) {
                let inputErrors = form.controls[input].errors;

                if(inputErrors) {
                    errors[input] = [];

                    Object.keys(inputErrors).forEach(id => {  
                        let error = new Erro();

                        error.origem = input;
                        error.descricao = inputErrors ? inputErrors[id].errorMessage : "";   
                        
                        errors[input].push(error);
                    });
                }                        
            }
        })

        return errors;
    }

    private setFormValidationErrors(form: FormGroup, errors: Erro[]) {
        Object.keys(form.controls).forEach(input => {
            let inputErrors = errors.filter(c => c.origem.toLowerCase() == input.toLowerCase());

            inputErrors.forEach(error => {  
                form.controls[input].setErrors({'app': error.descricao})
            });  
        });
    }
}