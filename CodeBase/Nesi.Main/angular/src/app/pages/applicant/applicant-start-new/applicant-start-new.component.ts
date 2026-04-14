import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { TokenService } from '../../../services/authentication/tokenService';
import { CONFIG } from '../../../configuration';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { WindowRef } from '../../../services/shared/windowRef';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { EmployeeFormBase } from '../../employees/_base/employeeFormBase';
import { EmployeeService } from '../../employees/_base/employeeService';

@Component({
  selector: 'nesi-applicant-start-new',
  templateUrl: './applicant-start-new.component.html',
  styleUrls: ['./applicant-start-new.component.css']
})
export class ApplicantStartNewComponent extends EmployeeFormBase implements OnInit {

  @Output() close = new EventEmitter();
  @Output() openEdit = new EventEmitter();
  public list: any[];
  public list_applicants: any[];
  public list_employees: any[];
  public has_checked = false;

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public ts: TokenService,
    private fb: FormBuilder,
    private win: WindowRef,
    private cf: ConfirmationService,
    public es: EmployeeService,
  ) {
    super(store, cs, es);
    super.InitEmployee(
      CONFIG.apiURL.page.applicant.startNew_existing,
      'N/A'
    );
  }


  createForm() {
    this.userform = this.fb.group({
      'label': '',
      'value': '',
    });
  }

  submitSuccess() {
    this.list_applicants = this.extraData.applicant_list;
    this.list_employees = this.extraData.employee_list;
    this.list = [...this.list_applicants, ...this.list_employees];
    this.has_checked = true;
  }

  click_item(row) {
    if (row.member_id) {
      this.win.boingNesi1('/#/opens/127/employees/' + row.member_id, 'employee_' + row.member_id);
    } else {
      this.openEdit.emit(row);
    }
  }

  start_new() {
    const entity = {
      id: 0,
      firstname: this.userform.get('label').value,
      lastname: this.userform.get('value').value
    };
    this.close.emit();
    this.openEdit.emit(entity);
  }

  get show_start_new() {
    return !(this.userform.get('label').value && this.userform.get('value').value);
  }
}
