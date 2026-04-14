import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { EmployeeFormBase } from '../_base/employeeFormBase';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { TokenService } from '../../../services/authentication/tokenService';
import { CONFIG } from '../../../configuration';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { WindowRef } from '../../../services/shared/windowRef';
import { EmployeeService } from '../_base/employeeService';
import { ValidateEmployee, isBackOffice, isBenefitRequired } from '../_user-information/validateEmployee';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-user-information',
  templateUrl: './employee-user-information.component.html',
  styleUrls: ['./employee-user-information.component.css']
})
export class EmployeeUserInformationComponent extends EmployeeFormBase implements OnInit {

  @Output() terminated = new EventEmitter();

  public dulicateSIN = false;
  public reports_to_error = false;
  public termianteDisplay = false;

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
      CONFIG.apiURL.page.employee.userInfo.base
    );
  }


  get photoUrl() {
    return this.win.generateNesi1Url('/_tools/member_photo/index.aspx?member_id=' + this.memberid);
  }

  print_barcode() {
    this.submitting = true;
    this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.employee.userInfo.printBarcode), { data: 1 })
      .subscribe(async res => {
        await this.PushResponseMessage(res.data);
        this.submitting = false;
      },
      (err:any)=>{
        super.PushErrorMessage(err);
      });
  }


  createForm() {
    this.userform = this.fb.group({
      'id': '',
      'benefits_id': ['', [isBenefitRequired(this.es)]],
      'life_insurance_id': '',
      'business_unit_id': '',
      'payrollNo': ['', [isBackOffice(this.es)]],
      'benefits_startdate': '',
      'firstName': ['', [Validators.required]],
      'lastName': ['', [Validators.required]],
      'nickname': '',
      'middleInitial': '',
      'phoneAreaCode': '',
      'phoneFirst': '',
      'phoneLast': '',
      'city': '',
      'postalCode': '',
      'address': '',
      'neCellPhoneArea': '',
      'neCellPhoneFirst': '',
      'neCellPhoneLast': '',
      'phoneExtension': '',
      'neTruck': '',
      'driversLicence': '',
      'username': '',
      'password': '',
      'apprentice_contract': '',
      'electricalLicence': '',
      'birthDate': '',
      'prov': '',
      'memberTypeID': '',
      'is_CAN_boardmember': '',
      'is_US_boardmember': '',
      'tswatch': '',
      'status': ['', [Validators.required]],
      'hasDependants': '',
      'member_default_location': '',
      'country': '',
      'paytype_id': ['', [Validators.required]],
      // tslint:disable-next-line:max-line-length
      'reports_to': ['', [Validators.required]],
      'payroll_handler': '',
      'hrstatus_id': '',
      'neEmail': '',
      'email': '',
      'start_Date': '',
      'terminateDate': '',
      'emergencyFirstName1': '',
      'emergencyLastName1': '',
      'emergencyPhoneArea1': '',
      'emergencyPhoneFirst1': '',
      'emergencyPhoneLast1': '',
      'pecell_area': '',
      'pecell_pref': '',
      'pecell_suff': '',
      'empnotes': '',
      'part_time': '',
      'memberwage': '',
      'reporting_text': '',
      'is_backoffice': ''
    }, { validator: this.validation() });

    const reportto = this.userform.get('reports_to');
    
    reportto.valueChanges
      .debounceTime(500)
      .filter(val => !!val)
      .switchMap(x => this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.employee.userInfo.CheckReportsTo), { data: x }))
      .subscribe(res => {
        if (!this.CheckResponseMessage(res.data)) {
          this.reports_to_error = true;
          reportto.setErrors({ data: res.extra });
        } else {
          reportto.setErrors(null);
          this.reports_to_error = false;
        }
        this.userform.get('reporting_text').setValue(res.extra);
      },
      (err:any)=>{
        super.PushErrorMessage(err);
      });
  }

  validation() {
    return (group: FormGroup) => {
      const errors = {
        // member_default_location: null,
        emerg_first_name: null,
        emerg_last_name: null,
        emerg_phone: null,
        benefits_id: null,
      };
      const member_default_location = !group.get('member_default_location').value
        && Number(group.get('business_unit_id').value) !== 11
        && Number(group.get('business_unit_id').value) !== 48
        && Number(group.get('hrstatus_id').value) > 1;
      // errors.member_default_location = member_default_location ? true : null;
      const hr_status = Number(group.get('hrstatus_id').value);
      const is_backoffice = Boolean(group.get('is_backoffice').value);
      const need_emerg = is_backoffice && (hr_status === 3 || hr_status === 6);
      const benefits_id = group.get('benefits_id').value;
      errors.benefits_id = !benefits_id && new Date(this.profile.entity.benefits_startdate) <= new Date();
      if (need_emerg) {
        errors.emerg_first_name = !group.get('emergencyFirstName1').value;
        errors.emerg_last_name = !group.get('emergencyLastName1').value;
        const emerg_area = !group.get('emergencyPhoneArea1').value;
        const emerg_pref = !group.get('emergencyPhoneFirst1').value;
        const emerg_suff = !group.get('emergencyPhoneLast1').value;
        errors.emerg_phone = emerg_area || emerg_pref || emerg_suff;
      }

      if (errors.emerg_phone || errors.emerg_first_name || errors.emerg_last_name
        || (errors.benefits_id && this.profile.personal_detail_visible)
        // || errors.member_default_location
      ) {
        return errors;
      } else {
        return null;
      }
    }
  }



  disableFields() {
    if (this.profile.entity.status === 'Active') {
      this.userform.get('status').disable();
    }
    if (!this.profile.reports_to_enabled) {
      this.userform.get('reports_to').disable();
    }
    if (!this.profile.membertype_enabled) {
      this.userform.get('memberTypeID').disable();
    }
    if (!this.profile.businessUnit_enabled) {
      this.userform.get('business_unit_id').disable();
    }
  }

  get working_cell(): string {
    // tslint:disable-next-line:max-line-length
    return `(${this.userform.get('neCellPhoneArea').value}) - ${this.userform.get('neCellPhoneFirst').value} ${this.userform.get('neCellPhoneLast').value}`;
  }

  print_off() {
    this.win.boingNesi1('/sections/hr/member/print_off.aspx?id=' + this.memberid, 'member_print_off_' + this.memberid, '800,960');
  }

  rest_to_do(type) {
    this.cs.postString(this.getUrl(CONFIG.apiURL.page.employee.userInfo.reset_todo), { data: type })
      .subscribe(
        res => {
          this.profile.payroll_verified_enabled = false;
        }
      );
  }


  submitValidate(): boolean {
    if (this.reports_to_error) {
      this.PushWarnMessage('Reports To Selection Error: Choosing this employee would result in a circular reports to chain.');
      return false;
    }
    return super.submitValidate();
  }

  terminate() {
    this.cf.confirm({
      message: 'Are you sure you want to TERMINATE ' + this.profile.entity.fullName + '?',
      accept: () => {
        this.termianteDisplay = true;
      }
    });
  }

  go_terminate() {
    this.termianteDisplay = false;
    this.submitting = true;
    setTimeout(() => {
      this.terminated.emit();
      this.submitting = false;
    }, 1000);
  }

  get disabled_save() {
    return this.profile.entity.hrstatus_id === 5;
  }


  getChargeOut() {
    const buid = this.userform.get('business_unit_id').value;
    const mtid = this.userform.get('memberTypeID').value;
    if (buid && mtid) {
      this.cs.getData<number>(CONFIG.apiURL.page.employee.userInfo.chargeout +
        buid + '/' + mtid).subscribe(res => {
          this.userform.get('memberwage').setValue(res);
        });
    }
  }

}
