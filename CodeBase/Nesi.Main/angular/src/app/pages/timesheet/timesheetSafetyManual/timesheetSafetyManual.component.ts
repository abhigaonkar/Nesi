import { Component, OnInit } from '@angular/core';
import { TimeSheetProfile } from '../../../models/pages/timesheet/timesheetProfile';
import { TimesheetPageBase } from '../interface/timesheetPageBase';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { TreeNode } from 'primeng/primeng';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetSafetyManual',
  templateUrl: './timesheetSafetyManual.component.html',
  styleUrls: ['./timesheetSafetyManual.component.css']
})
export class TimesheetSafetyManualComponent extends TimesheetPageBase implements OnInit {

  public safetyManualsURL: TreeNode[];

  constructor(
    protected tss: TimesheetService,
    protected store: Store<fromRoot.State>,

  ) {
    super(tss, store);
  }

  ngOnInit() {
    super.Init();
    this.tss.getList<TreeNode>(CONFIG.apiURL.core.fileManager.safetyFiles + '1')
      .subscribe(
      (
        res: TreeNode[]) => this.safetyManualsURL = res
      ),
      (err:any)=>{
        this.PushErrorMessage(err);
      };
  }

  afterProfileLoad(): void {

  }
}
