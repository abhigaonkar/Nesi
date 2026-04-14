import { TimeSheetProfile } from '../../../models/pages/timesheet/timesheetProfile';
import { OnInit } from '@angular/core';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { CONFIG } from '../../../configuration';
import { FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from '../../../core/messageBaseComponent';

export abstract class TimesheetPageBase extends MessageBase {
  public Profile: TimeSheetProfile;


  constructor(
    protected tss: TimesheetService,
    protected store: Store<fromRoot.State>,
  ) {
    super(store);
    this.Profile = tss.Profile;
  }

  abstract afterProfileLoad(): void;

  protected Init() {
    //  CONFIG.LOG(this.tss.Profile, 'timesheet page base init');
    if (!this.tss.Profile) {
      this.tss.TimeSheetProfile()
        .subscribe((res: TimeSheetProfile) => {
          this.Profile = res;
          this.tss.Profile = this.Profile;
          this.afterProfileLoad();
        });
    } else {
      this.afterProfileLoad();
    }
  }
}
