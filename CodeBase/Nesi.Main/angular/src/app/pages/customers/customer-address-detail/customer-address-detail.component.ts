
import { Component, OnInit ,Input} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-address-detail',
  templateUrl: './customer-address-detail.component.html',
  styleUrls: ['./customer-address-detail.component.css']
})
export class CustomerAddressDetailComponent extends CustomerFormBase implements OnInit {

  @Input() disabled:boolean;
  table_list = [
    { label: 'Customer', value: 'Customer' }
   
  ];
  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.edit.address + '/$address_id'
    );
  }


  createForm() {
    this.userform = this.fb.group({
      'customer_id': '',
      'address_id': '',
      'table': [{value:'',disabled: true}, [Validators.required]],
      'description': '',
      'address_line_1': ['', [Validators.required, Validators.maxLength(200)]],
      'address_line_2': ['', [Validators.maxLength(200)]],
      'address_line_3': ['', [Validators.maxLength(200)]],
      'address_line_4': ['', [Validators.maxLength(200)]],
      'address_city': ['', [Validators.required, Validators.maxLength(50)]],
      'address_prov': [{value:'',disabled: true}, [Validators.required]],
      'address_postalcode': ['', [Validators.required, Validators.maxLength(10)]],
      'address_country': [{value:'',disabled: true}, [Validators.required]],
      'phone_area': ['', [Validators.required, Validators.maxLength(3), Validators.minLength(3)]],
      'phone_prefix': ['', [Validators.required, Validators.maxLength(3), Validators.minLength(3)]],
      'phone_suffix': ['', [Validators.required, Validators.maxLength(4), Validators.minLength(4)]],
      'phone_ext': ['', [Validators.maxLength(5)]],
      'fax_area': ['', [Validators.maxLength(3)]],
      'fax_prefix': ['', [Validators.maxLength(3)]],
      'fax_suffix': ['', [Validators.maxLength(4)]],
      'gps_coordinates': ['', [Validators.maxLength(50)]],
      'website': ['', [Validators.maxLength(200)]],
      'facebook': ['', [Validators.maxLength(200)]],
      'twitter': ['', [Validators.maxLength(200)]],
      'linkedin': ['', [Validators.maxLength(200)]],
      'active':false,
    });
  }

  AfterProfileLoaded() {
    if (this.profile.address.address_id <= 0) {
      this.initFormvalue = this.profile.address;
    } else {
      this.userform.patchValue(this.profile.address);
    }
    if (!this.customer_base_profile.is_admin_authenticated) {
      this.userform.disable();
    }
  }

}
