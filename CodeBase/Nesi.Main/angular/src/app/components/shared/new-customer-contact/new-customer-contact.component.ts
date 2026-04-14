import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { CustomerContact } from '../../../models/Shared/customerContact';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-new-customer-contact',
  templateUrl: './new-customer-contact.component.html',
  styleUrls: ['./new-customer-contact.component.css']
})
export class NewCustomerContactComponent extends FormMessageBase implements OnInit {

  @Output() close = new EventEmitter();
  @Input() customerId: number;
  @Input() addressId: number;
  customer_contact:number;

  titles: LabelValueInt[];

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,

  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.shared.customerContactNew + this.customerId + '/' + this.addressId);
  }

  reInit(value: number) {
    this.customerId = value;
    this.postUrl = CONFIG.apiURL.page.shared.customerContactNew + this.customerId + '/' + this.addressId;
    this.userform.get('customer_id').setValue(value);
  }

  reLoad(value:number)
  {
    this.customer_contact=value;
    console.log(this.customer_contact,"this.customer_contact");
    
    this.postUrl = CONFIG.apiURL.page.shared.CustomerContactEdit + this.customer_contact;
    this.cs.getObject<CustomerContact>(CONFIG.apiURL.page.shared.CustomerContactEdit + this.customer_contact)
    .subscribe((res:CustomerContact) => {
        console.log(res,"CustomerContact");
        this.userform.get('name').setValue(res.name);
        if(res.title)
        this.userform.get('title').setValue(res.title);
        if(res.email)
        this.userform.get('email').setValue(res.email);
        if(res.phone)
        this.userform.get('phone').setValue(res.phone);
        this.userform.get('customer_id').setValue(res.customer_id);
      });

  }

  createForm() {
    this.userform = this.fb.group({
      'name': ['', Validators.required],
      'title': ['', Validators.required],
      'email': '',
      'extension': '',
      'phone': '',
      'customer_id': '',
    }, {
        validator: this.emailOrphone()
      });

    this.initFormvalue = {
      'name': '',
      'title': '',
      'email': '',
      'extension': '',
      'phone': '',
      'customer_id': this.customerId,
    };

  }

  emailOrphone() {
    return (group: FormGroup): { [key: string]: any } => {
      const e = group.controls['email'].value.toString();
      const p = group.controls['phone'].value.toString();
      const valide = (e !== '' && CONFIG.Regex.email.test(e));
      const validep = (p !== '');

      if ((p === '' && valide) || (e === '' && validep) || (valide && validep)) {
        return null;
      } else {
        return {
          emailorphone: true
        };
      }
    }
  }

  ngOnInit() {
    this.loadTitles();
  }

  loadTitles() {
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.shared.customerContactTitles)
      .subscribe((res: LabelValueInt[]) => {
        this.titles = res;
      });
  }

  clickCancel() {
    super.submitReset();
    this.close.emit();
  }
}
