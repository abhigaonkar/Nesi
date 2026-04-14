import { Component, OnInit, Input } from '@angular/core';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { EmployeeService } from '../../_base/employeeService';
import { EmployeeOfferFormBase } from '../employeeOfferBase';
import { DataExtra } from '../../../../models/core/dataExtra';
import { WindowRef } from '../../../../services/shared/windowRef';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-offer-milestones',
  templateUrl: './employee-offer-milestones.component.html',
  styleUrls: ['./employee-offer-milestones.component.css']
})
export class EmployeeOfferMilestonesComponent extends EmployeeOfferFormBase implements OnInit {

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    private fb: FormBuilder,
    private win: WindowRef,
  ) {
    super(store, cs, es);
    this.InitEmployee(CONFIG.apiURL.page.employee.offer.mileStone);
  }

  createForm() {
    this.userform = this.fb.group({
      'id': '',
      'milestone': ['', [Validators.required]],
      'due': ['', [Validators.required]],
    });
  }


  save(row, refresh = false) {
    setTimeout(() => {
      this.cs.postDataExtra(this.getUrl(this.postUrl), row)
        .subscribe(res => {
          if (refresh) { this.profile = res.extra; }
        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
        });
    }, 300);
  }

  //
  // This may be a bug from p-calendar.
  // Based on the current implementation, the above 'save' event will be called when 'blur' happending,
  // but will not be called if selected a new date from calendar after switching to another month.
  // so here is to add one more event to handle this based on primeng docs.
  //
  saveOnSelect(row) {
    this.save(row, false);
  }

  submitSuccess() {
    this.profile = this.extraData;
  }

  delete(row) {
    this.cs.deleteObject<DataExtra>(this.getUrl(this.postUrl) + '/' + row.id)
      .subscribe(res => {
        if (this.PushResponseMessage(res.data)) {
          this.profile = res.extra;
        }
      },
      (err:any)=>
      {
        this.PushErrorMessage(err);
      });
  }

  open_ticket(row) {
    this.win.boingNesi1(CONFIG.Nesi1URL.openticket.replace('@ticketid', row.ticketid), 'open_ticket_' + row.ticketid);
  }

  // convert_ticket(row) {
  //   this.win.boingNesi1(CONFIG.Nesi1URL.createticket.replace('@id', row.id), 'create_ticket_' + row.id);
  //   // setTimeout(() => {
  //   //   this.loadData();
  //   // }, 2000);
  // }
}
