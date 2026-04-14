
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';
import { DataExtra } from 'app/models/core/dataExtra';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';

@Component({
  selector: 'nesi-customer-contact-phone',
  templateUrl: './customer-contact-phone.component.html',
  styleUrls: ['./customer-contact-phone.component.css']
})
export class CustomerContactPhoneComponent extends CustomerFormBase implements OnInit {

  @Input() disabled:boolean;
  @Output() cancel = new EventEmitter();

  selected: any;
  phoneTypeList = [
    { label: 'Fax', value: 'Fax' },
    { label: 'LandLine', value: 'LandLine'},
  ];
  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private cf: ConfirmationService,

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.edit.phoneNumber
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'phone_numbers_id': 0,
      'comm_type': ['Fax', [Validators.required, Validators.min(1)]],
      'number': ['', [Validators.required, Validators.maxLength(12), Validators.minLength(10)]],
      'is_default': false,
      'is_active': true,
      'type': 'Address',
      'phone_numbers_table_id': [this.address_id, [Validators.required, Validators.min(1)]],
    });

    this.initFormvalue = {
      phone_numbers_id: 0,
      comm_type: 'Fax',
      number: '',
      is_default: false,
      is_active: true,
      type: 'Address',
      phone_numbers_table_id: this.address_id,
    };
  }


  submitSuccess() {
    this.profile.phoneList = this.extraData;
    this.selected = null;
  }

  formValidateBefore() {
    this.userform.get('phone_numbers_table_id').setValue(this.address_id);
    this.userform.get('type').setValue('Address');

  }
  delete(item) {
    this.cf.confirm({
      message: 'Do you really want to delete this item?',
      accept: () => {
        if (item) {
          this.cs.deleteObject<DataExtra>(this.getUrl(this.postUrl) + '/' + item.phone_numbers_id)
            .subscribe(
            (res) => {
              if (this.PushResponseMessage(res.data)) {
                this.profile.phoneList = res.extra;
                this.submitReset();
                this.selected=null;
              }
            }
            );
        }
      },
      reject: () => { }
    });
  }

  select(event) {
    if (event && event.data) {
      this.userform.patchValue(event.data);
    }
  }
}