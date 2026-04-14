
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-contact',
  templateUrl: './customer-contact.component.html',
  styleUrls: ['./customer-contact.component.css']
})
export class CustomerContactComponent extends CustomerFormBase implements OnInit {
  @Input() lists: any;
  @Input() disable:boolean;

  @Input() set contact(value: any) {
    if (value) {
      this.userform.patchValue(value);
    }
  }

  @Output() cancel = new EventEmitter();

  duplicate_name = false;
  duplicate_email = false;
  duplicate_cellphone = false;
  duplicate_directline = false;
  contact_password_required = false;
  email_required = false;

  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.edit.contact,
      'N/A'
    );
  }

  get url_is_duplciate(): string {
    return this.getUrl(CONFIG.apiURL.page.customers.edit.contactIsDuplicate) + '/' + this.userform.get('contact_id').value;
  }

  createForm() {
    this.userform = this.fb.group({
      'customer_id': this.customer_id,
      'address_id': this.address_id,
      'contact_id': 0,
      'contact_name': ['', [Validators.required, Validators.maxLength(120)]],
      'name_first': '',
      'name_last': '',
      'contact_cellphone': ['', [Validators.maxLength(20)]],
      'contact_title': ['', [Validators.maxLength(200)]],
      'contact_directline': ['', [Validators.maxLength(20)]],
      'contact_extension': ['', [Validators.maxLength(200)]],
      'contact_email': ['', [Validators.maxLength(100)]],
      'contact_password': ['', [Validators.maxLength(50)]],
      'contact_status': 'Active',
      'login_enabled': false,
      'stopsurveys': false,
      'contact_status_id': 8,
      'facebook': ['', [Validators.maxLength(200)]],
      'twitter': ['', [Validators.maxLength(200)]],
      'linkedin': ['', [Validators.maxLength(200)]]
    });
  }

  setDefaultName() {
    const name = this.userform.get('name_first').value + ' ' + this.userform.get('name_last').value;
    this.userform.get('contact_name').setValue(name);
    this.check_duplicate_name();
  }

  fillPassword() {
    const v = this.userform.get('login_enabled').value;
    const p = this.userform.get('contact_password').value;
    if (v && !p) {
      this.cs.getString(CONFIG.apiURL.page.customers.edit.contactRandomPassword)
        .subscribe(
          (res) => {
            this.userform.get('contact_password').setValue(res);
          }
        );
    }
  }

  formValidateBefore() {
    super.formValidateBefore();
    this.submitValidate();
  }

  submitValidate(): boolean {
    let r = true;
    if (this.duplicate_name || this.duplicate_cellphone || this.duplicate_directline || this.duplicate_email) {
      r = false;
    }
    const v = this.userform.get('login_enabled').value;
    const p = this.userform.get('contact_password').value;
    const e = this.userform.get('contact_email').value;
    if (v && !p) {
      this.contact_password_required = true;
      r = false;
    }
    if (v && !e) {
      this.email_required = true;
      r = false;
    }
    return r;
  }

  check_duplicate_name() {
    const v = this.userform.get('contact_name').value;
    if (!v) {
      this.duplicate_name = false;
      return;
    }
    this.cs.postData<boolean>(this.url_is_duplciate,
      {
        id: 1,
        value: v
      }
    ).subscribe(
      (res) => {
        this.duplicate_name = res;
      }
    );
  }

  check_duplicate_email() {
    const v = this.userform.get('contact_email').value;
    if (!v || String(v).length === 0) {
      this.duplicate_email = true;
      return;
    }
    this.cs.postData<boolean>(this.url_is_duplciate,
      {
        id: 2,
        value: v
      }
    ).subscribe(
      (res) => {
        this.duplicate_email = res;
      }
    );
  }

  check_duplicate_cellphone() {
    const v = this.userform.get('contact_cellphone').value;
    if (!v || String(v).length === 0) {
      this.duplicate_cellphone = false;
      return;
    }
    this.cs.postData<boolean>(this.url_is_duplciate,
      {
        id: 3,
        value: v
      }
    ).subscribe(
      (res) => {
        this.duplicate_cellphone = res;
      }
    );
  }

  check_duplicate_directline() {
    const v = this.userform.get('contact_directline').value;
    if (!v || String(v).length === 0) {
      this.duplicate_directline = false;
      return;
    }
    this.cs.postData<boolean>(this.url_is_duplciate,
      {
        id: 4,
        value: v
      }
    ).subscribe(
      (res) => {
        this.duplicate_directline = res;
      }
    );
  }
}

