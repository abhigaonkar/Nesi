import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import { CoreService } from '../../../services/shared/core.service';
import * as fromRoot from '../../../reducers';


export abstract class TimesheetExpenseBase extends FormMessageBase {

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
  ) {
    super(store, cs);
  }


  protected checkWorkOrderAndShop() {
    return (group: FormGroup): { [key: string]: any } => {
      const shop = group.controls['isShop'].value;
      const wo = group.controls['wo_number'].value;

      const start = group.controls['date_start'];
      const end = group.controls['date_end'];
      const workOrderOrShop = !((shop === true) || (wo && wo.toString().length > 0));
      const EndDateGreatThanStartDate = start && end && start.value > end.value;

      if (workOrderOrShop && EndDateGreatThanStartDate) {
        return {
          EndDateGreatThanStartDate: true,
          workOrderOrShop: true
        };
      } else if (workOrderOrShop) {
        return {
          workOrderOrShop: true
        };
      } else if (EndDateGreatThanStartDate) {
        return {
          EndDateGreatThanStartDate: true,
        };
      } else {
        return null;
      }
    }

  }
}

