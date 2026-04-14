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
import { LabelValueString } from '../../../models/Shared/labelValueInt';

@Component({
  selector: 'nesi-applicant-edit',
  templateUrl: './applicant-edit.component.html',
  styleUrls: ['./applicant-edit.component.css']
})
export class ApplicantEditComponent extends EmployeeFormBase implements OnInit {

  @Output() close = new EventEmitter();
  @Output() onSave = new EventEmitter();


  public statusList: LabelValueString[];

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
      CONFIG.apiURL.page.applicant.edit_profile
    );
    this.statusList = es.applicant_statusList;
  }


  createForm() {
    this.userform = this.fb.group({
      'id': '',
      'firstname': ['', [Validators.required]],
      'lastname': ['', [Validators.required]],
      'business_unit_id': ['', [Validators.required, Validators.min(1)]],
      'notes': '',
      'status': ['', [Validators.required]],
      'becomes_memberid': '',
      'address': '',
      'membertypeid': ['', [Validators.required, Validators.min(1)]],
      'city': '',
      'province': '',
      'country': '',
      'postal': '',
      'cellphone': ['', [Validators.required,Validators.maxLength(20)]],
      'email': ['', [Validators.required, Validators.email]],
      'homephone': '',
      'apt': '',
      'addedbymemberid': '',
      'dateentered': '',
    });

  }

  public isVaildPhone(): boolean {
    const control = this.userform.get('cellphone');
    if (control && control.dirty) {
      // tslint:disable-next-line:max-line-length
      const phoneRegExp = new RegExp(/^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/);
      if (phoneRegExp.test(control.value)) {
          return true;
      }
      return false;
    } else {
      return true;
    }
  }

 public isValidEmail(): boolean {
    const control = this.userform.get('email');
    if (control && control.dirty) {
      // tslint:disable-next-line:max-line-length
      const emailRegExp = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
      if (emailRegExp.test(control.value)) {
        let test = '';
        test = control.value;
        if (test.includes('nomail@')) {
          return false
        }
          return true;
      }
  return false;
    } else {
      return true;
    }
  }

  disableFields() {
    if (this.applicant_id === 0) {
      if (this.employee.firstname) {
        this.userform.get('firstname').setValue(this.employee.firstname);
        this.userform.get('firstname').disable();
      } else {
        this.userform.get('firstname').enable();

      }
      if (this.employee.lastname) {
        this.userform.get('lastname').setValue(this.employee.lastname);
        this.userform.get('lastname').disable();
      } else {
        this.userform.get('lastname').enable();

      }
    } else {
      this.userform.get('firstname').enable();
      this.userform.get('lastname').enable();
    }
  }

  submitFailed() {
    this.userform.patchValue(this.extraData);
  }
  submitSuccess() {
    this._applicant_id = this.extraData.id;
    this.onSave.emit(this.applicant_id);
  }

  AfterProfileLoaded() {
    super.AfterProfileLoaded();
    if (this.userform && this.profile.entity) {
      if (this.profile.entity.id === 0) {
        this.userform.get('business_unit_id').setValue(this.ts.currentUser.businessUnitId);
        this.userform.get('membertypeid').setValue(this.profile.memberTypeList[0].value);
      }
    }
  }
}
