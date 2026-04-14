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

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-offer-extras',
  templateUrl: './employee-offer-extras.component.html',
  styleUrls: ['./employee-offer-extras.component.css']
})
export class EmployeeOfferExtrasComponent extends EmployeeOfferFormBase implements OnInit {

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    public fb: FormBuilder,

  ) {
    super(store, cs, es);
    this.InitEmployee(CONFIG.apiURL.page.employee.offer.extra);
  }

  createForm() {
    this.userform = this.fb.group({
      'memberid': ['', [Validators.required]],
      'id': '',
      'applicantid': '',
      'business_unit_id': ['', [Validators.required]],
      'membertypeid': ['', [Validators.required]],
      'isapplicant': '',
      'gets_phone': '',
      'gets_laptop': '',
      'gets_vehicle': '',
      'gets_neemail': '',
      'gets_barcodescanner': '',
      'gets_businesscards': '',
      'gets_directdeposit': '',
      'gets_phoneext': '',
      'notes': '',
    });
  }

}
