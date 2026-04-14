import { Component, OnInit, Input } from '@angular/core';
import { TimeSheetProfile } from '../../../models/pages/timesheet/timesheetProfile';
import { ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { MessageBase } from '../../../core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { WindowRef } from '../../../services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetBarMenu',
  templateUrl: './timesheetBarMenu.component.html',
  styleUrls: ['./timesheetBarMenu.component.css']
})
export class TimesheetBarMenuComponent extends MessageBase implements OnInit {
  @Input()
  public Profile: TimeSheetProfile;
  public isdebug = CONFIG.ISDEBUG();

  url: string;
  constructor(
    private route: ActivatedRoute,
    protected store: Store<fromRoot.State>,
    private winRef: WindowRef,
    public cs: CoreService,

  ) {
    super(store);
  }

  ngOnInit() {
    this.route.url.subscribe(
      (url) => {
        CONFIG.LOG(url, 'timesheet menubar init');
        this.url = url.toString();
      },
        (err:any)=>
        {
          super.PushErrorMessage(err);
        }
    )
  }

  test() {
    this.winRef.boing('/#/home/28/timesheet/safetymanual', 'test');
  }
  test1() {
    this.winRef.boing('/#/home/28/timesheet/vacation', 'v');
  }
}
