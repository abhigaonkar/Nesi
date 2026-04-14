import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { FormBuilder, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { EmployeeService } from '../../_base/employeeService';
import { EmployeeOfferFormBase } from '../employeeOfferBase';
import { isNumber } from 'util';

@Component({
  selector: 'nesi-employee-offer-wage',
  templateUrl: './employee-offer-wage.component.html',
  styleUrls: ['./employee-offer-wage.component.css']
})
export class EmployeeOfferWageComponent extends EmployeeOfferFormBase implements OnInit {

  isZeroWageAllowed = false;

  @Output() refresh = new EventEmitter();

  get wage() {
    const w = this.userform.get('wage');
    return w;
  }

  private minWage = 0;

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    public fb: FormBuilder,

  ) {
    super(store, cs, es);
    this.InitEmployee(CONFIG.apiURL.page.employee.offer.wage);
  }

  ngOnInit() {
    this.onChange.subscribe(
      (v) => {
        if (v.result.entity.comp_details) {
          this.refresh.emit();
        }
      },
      (err:any)=>
      {
        this.PushErrorMessage(err);
      }
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'memberid': ['', [Validators.required]],
      'id': '',
      'applicantid': '',
      'business_unit_id': ['', [Validators.required]],
      'membertypeid': ['', [Validators.required]],
      'isapplicant': '',
      'wage': ['', [Validators.required, this.zeroWageValidator(this.minWage)]],
      'paytype_id': ['', [Validators.required]],
      'vendor_id': ['', [Validators.required]],
      'part_time': '',
      'is_CDN': '',
      'bonus_type': '',
      'startdate': '',
      'start_wage': '',
      'previous_wage': '',
      'current_wage': '',
      'bonus_amount': '',
      'bonus_margin_threshold': '',
      'bonus_netincome_threshold': '',
      'bonus_netincome_highwater': '',
      'bonus_revenue_threshold': '',
      'comp_details': '',
      'contract_details': '',
      'benefits_startdate': '',
      'vacation_amount_1': ['', [Validators.required]],
      'vacation_amount_2': ['', [Validators.required]],
      'vacation_amount_3': ['', [Validators.required]],
      'vacation_interval_1': ['', [Validators.required]],
      'vacation_interval_2': ['', [Validators.required]],
      'vacation_interval_3': ['', [Validators.required]],
    });

    this.userform.get('wage').valueChanges
      .debounceTime(500)
      .subscribe((value) => {
        if (value && isNumber(value)) {
          this.getWageNotes();
        }
      },
      (err:any)=>
      {
        this.PushErrorMessage(err);
      }
      );
    this.userform.get('paytype_id').valueChanges
      .debounceTime(500)
      .subscribe((value) => {
        if (value && isNumber(value)) {
          this.getWageNotes();

          this.isZeroWageAllowed = false;
          if (value === 6) {
            // Zero wage is allowed for co-op students.
            this.isZeroWageAllowed = true;
          }
        }
      },
      (err:any)=>
      {
        this.PushErrorMessage(err);
      }
      );

  }

  getWageNotes() {
    const postdata = {
      value: this.userform.get('wage').value,
      id: this.userform.get('paytype_id').value
    };
    if (!postdata.value || !postdata.id) {
      return;
    }
    this.cs.postObject(this.getUrl(CONFIG.apiURL.page.employee.offer.wageNotes), postdata)
      .subscribe((res) => {
        this.profile.entity.wage_notes = res;
      },
      (err:any)=>
      {
        this.PushErrorMessage(err);
      });
  }

  zeroWageValidator(minWage: number): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any} | null => {
      if (control.value === null  || control.value === '') {
        return null;
      }

      console.log(`isZeroWageAllowed = ${this.isZeroWageAllowed}`);
      if (!this.isZeroWageAllowed && control.value <= minWage) {
        return {'minWage': {value: control.value}};
      }
      return  null;
    };
  }
}
