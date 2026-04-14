import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CONFIG } from '../../../../configuration';
import { CoreService } from '../../../../services/shared/core.service';
import { MessageBase } from '../../../../core/messageBaseComponent';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { EmployeeOfferFormBase } from '../../_employment/employeeOfferBase';
import { Validators, FormBuilder } from '@angular/forms';
import { EmployeeService } from '../../_base/employeeService';
import { ConfirmationService } from 'primeng/primeng';
import { DataExtra } from '../../../../models/core/dataExtra';
import { EmployeeReviewBase } from '../employeeReviewBase';
import { WindowRef } from '../../../../services/shared/windowRef';

@Component({
  selector: 'nesi-employee-review',
  templateUrl: './employee-review.component.html',
  styleUrls: ['./employee-review.component.css']
})
export class EmployeeReviewComponent extends EmployeeReviewBase implements OnInit {

  @Output() onDelete = new EventEmitter();
  @Output() onPrint = new EventEmitter();

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
      CONFIG.apiURL.page.employee.review.profile
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'member_id': ['', Validators.required],
      'id': ['', Validators.required],
      'date': ['', Validators.required],
      'reviewed_by_id': ['', Validators.required],
      'offerid': ['', Validators.required],
      'locked': false,
    });
  }

  disableFields() {
    if (this.review_id > 0) {
      this.userform.get('offerid').disable();
    }
    if (this.save_disabled) {
      this.userform.disable();
    }
  }
  get save_disabled(): boolean {
    return this.profile.entity.status == "Closed" || this.profile.entity.status == "Delivered"
      || (this.profile.entity.locked && (this.profile.entity.was_printed || !this.profile.entity.is_supervisor))
      ;
  }

  get unlcoked_message() {
    if (this.profile.entity.locked) {
      if (this.profile.entity.was_printed) {
        return 'Because the final printout was printed, this review can\'t be unlocked.';
      } else if (!this.profile.entity.is_supervisor) {
        return 'This can be unlocked by: ' + this.profile.reports_to_name;
      }
    } else {
      return null;
    }
  }
  get delete_visible(): boolean {
    return !this.profile.entity.locked && this.review_id > 0;
  }

  get print_copy_visible() {
    return this.profile.entity.status == "Ready to Deliver";
  }


  get print_enabled() {
    return this.review_id > 0;
  }

  print(t: boolean) {
    this.onPrint.emit({ id: this.review_id, locked: this.userform.get('locked').value ? 1 : 0, t: t });
  }

  print_copy() {
    this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.employee.review.print_copy), { data: this.review_id })
      .subscribe((res) => {
        if (this.CheckResponseMessage(res.data)) {
          this.profile = res.extra;
          this.loadProfile();
          this.print(true);
        }
      });
  }

  delete() {
    this.onDelete.emit(this.review);
  }

  reviewItem_changed(data) {
    this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.employee.review.saveReview), data)
      .subscribe(
        (res) => {
          if (this.CheckResponseMessage(res.data)) {
            this.profile.score1 = res.extra.score1;
            this.profile.score2 = res.extra.score2;
          }
        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
        }
      );
  }

  milestone_changed(data) {
    this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.employee.review.saveMileStone), data)
      .subscribe(
        (res) => {
          if (!this.CheckResponseMessage(res.data)) {
            this.PushErrorMessage(res.data);
          }
        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
        }
      );
  }

}
