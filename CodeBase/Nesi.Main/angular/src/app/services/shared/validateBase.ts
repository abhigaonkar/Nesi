import { AbstractControl } from "@angular/forms";
import { Observable } from "rxjs/Observable";
import { LabelValueInt } from "../../models/Shared/labelValueString";

export class ValidateBase {
    public static debounceTime = 500;

    public static ConvertValidateResult(result: Observable<LabelValueInt>) {
        return result
            .map(res => {
                return !res ? null : { data: (res.value > 0 ? res.value + ' ' : '') + res.label };
            });
    }
}