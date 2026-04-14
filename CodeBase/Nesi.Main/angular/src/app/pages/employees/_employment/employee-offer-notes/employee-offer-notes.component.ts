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
  selector: 'nesi-employee-offer-notes',
  templateUrl: './employee-offer-notes.component.html',
  styleUrls: ['./employee-offer-notes.component.css']
})
export class EmployeeOfferNotesComponent extends EmployeeOfferFormBase implements OnInit {

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,

) {
    super(store, cs, es);
}
}
