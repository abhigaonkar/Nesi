import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { MessageBase } from '../../../core/messageBaseComponent';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { ConfirmationService } from 'primeng/primeng';
import { DataExtra } from '../../../models/core/dataExtra';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-employment',
  templateUrl: './employee-employment.component.html',
  styleUrls: ['./employee-employment.component.css']
})
export class EmployeeEmploymentComponent extends MessageBase implements OnInit {

  @Output() loading = new EventEmitter();
  @Output() loaded = new EventEmitter();

  submitting = false;
  list: any[];

  memberid: number;
  applicantid: number;
  is_applicant = false;
  _employee: any;
  displayOffer = false;

  selected_offer: any;

  @Input() set employee(value: any) {
    if (value) {
      this._employee = value;
      this.memberid = this.employee.memberid || this.employee.member_id || 0;
      this.applicantid = this.employee.applicantid || this.employee.applicant_id || 0;
      this.is_applicant = this.employee.is_applicant || this.employee.isapplicant || false;
      this.getList();
    }
  }

  get employee(): any {
    return this._employee || {};
  }

  constructor(
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    private cf: ConfirmationService,
  ) {
    super(store);

  }
  ngOnInit() {
  }


  getList() {
    this.submitting = true;
    this.loading.emit();
    let o: string;
    if (this.is_applicant) {
      o = CONFIG.apiURL.page.employee.employment.applicantList + this.applicantid;
    } else {
      o = CONFIG.apiURL.page.employee.employment.List + this.memberid;
    }
    this.cs.getList(o).subscribe((res) => {
      this.list = res;
      this.loaded.emit();
      this.submitting = false;
    },
    (err: any) => {
      super.PushErrorMessage(err);
      this.loaded.emit();
      this.submitting = false;
    }
    );
  }

  add_new() {
    this.selected_offer = { id: 0, memberid: this.memberid, is_applicant: this.is_applicant, applicant_id: this.applicantid };
    this.displayOffer = true;
  }

  edit(entity) {
    entity.is_applicant = this.is_applicant;
    entity.applicant_id = this.applicantid;
    entity.memberid = this.memberid;
    this.selected_offer = entity;
    this.displayOffer = true;
  }

  delete(entity) {
    this.cf.confirm({
      message: 'Are you sure you want to delete this offer?',
      accept: () => {
        if (entity.is_signed) {
          this.PushWarnMessage('You can not delete offers that have already been signed');
          return;
        }
        if (entity.status !== 'In Development' && entity.status !== 'Closed') {
          this.PushWarnMessage('"You can not delete offers unless they are of the status \'In Development\' or \'Closd\'');
          return;
        }
        this.submitting = true;
        let o: string;
        if (this.is_applicant) {
          o = CONFIG.apiURL.page.employee.employment.applicantList + this.applicantid + '/' + entity.id;
        } else {
          o = CONFIG.apiURL.page.employee.employment.List + this.memberid + '/' + entity.id;
        }
        this.cs.deleteObject<DataExtra>(o)
          .subscribe(
            (res) => {
              if (this.PushResponseMessage(res.data)) {
                this.list = res.extra;
                this.submitting = false;
              }
            },
            (err:any)=>
            {
              this.PushErrorMessage(err);
              this.submitting = false;
            }
          );
      }
    });
  }
}
