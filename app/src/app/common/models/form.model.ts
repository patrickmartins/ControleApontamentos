import { FormGroup } from "@angular/forms";

export abstract class FormModel<TModel> {
    abstract toForm(): FormGroup
    abstract toModel(form: FormGroup): TModel
}