import { Component, OnInit, Input } from '@angular/core';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { MessageBase } from '../../../core/messageBaseComponent';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { EmployeeOfferFormBase } from '../_employment/employeeOfferBase';
import { Validators, FormBuilder } from '@angular/forms';
import { EmployeeService } from '../_base/employeeService';
import { ConfirmationService } from 'primeng/primeng';
import { DataExtra } from '../../../models/core/dataExtra';
import { WindowRef } from '../../../services/shared/windowRef';

@Component({
  selector: 'nesi-employee-reviews',
  templateUrl: './employee-reviews.component.html',
  styleUrls: ['./employee-reviews.component.css']
})
export class EmployeeReviewsComponent extends EmployeeOfferFormBase implements OnInit {
  selected: any;
  display = false;

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    private fb: FormBuilder,
    private cf: ConfirmationService,
    private win: WindowRef,

  ) {
    super(store, cs, es);
    super.InitEmployee(
      CONFIG.apiURL.page.employee.review.listProfile
    );
  }

  createForm() {
  }

  add_new() {
    this.selected = { id: 0, memberid: this.memberid };
    this.display = true;
  }

  edit(entity) {
    this.selected = entity;
    this.display = true;
  }

  print(entity) {
    this.print_by_id(entity.id, entity.locked, entity.locked || entity.t);
    if (entity.t) {
      this.loadData();
    }
  }

  print_by_id(id: number, locked: boolean, t: boolean) {
    this.win.boingNesi1(CONFIG.Nesi1URL.printReview.
      replace('@reviewid', id.toString()).
      replace('@locked', locked ? '1' : '0')
      .replace('@t', t ? '0' : '1'),
      'print_review_' + id.toString());
  }

  delete(entity) {
    this.cf.confirm({
      message: 'Are you sure you want to delete this review?',
      accept: () => {
        this.display = false;
        this.submitting = true;
        this.cs.deleteObject<DataExtra>(this.getUrl(this.postUrl) + '/' + entity.id)
          .subscribe(
            (res) => {
              if (this.PushResponseMessage(res.data)) {
                this.profile = res.extra;
                this.submitting = false;
              }
            },
            (err:any)=>{
              super.PushErrorMessage(err);
            }
          );
      }
    });
  }

}
