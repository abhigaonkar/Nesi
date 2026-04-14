
import { Component, OnInit,Input} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';
import { DataExtra } from 'app/models/core/dataExtra';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { WindowRef } from 'app/services/shared/windowRef';
import * as fromMessage from '../../../actions/layout/growlMessage';
@Component({
  selector: 'nesi-customer-contact-list',
  templateUrl: './customer-contact-list.component.html',
  styleUrls: ['./customer-contact-list.component.css']
})
export class CustomerContactListComponent extends CustomerFormBase implements OnInit {

  
  @Input() disabled:boolean;
  addnewDisplay = false;
  lists: any;
  data: any[];

  selected_row: any;

  new_contact: any;

  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private cf: ConfirmationService,
    private win: WindowRef,

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.edit.contact
    );
  }

  AfterProfileLoaded() {
    if (this.profile) {
      this.data = this.profile.data;
      this.lists = this.profile.lists;
    }
  }


  add_new() {
    this.addnewDisplay = !this.addnewDisplay;
    this.new_contact = {
      'customer_id': this.customer_id,
      'address_id': this.address_id,
      'contact_id': 0,
      'contact_name': '',
      'name_first': '',
      'name_last': '',
      'contact_cellphone': '',
      'contact_title': '',
      'contact_directline': '',
      'contact_extension': '',
      'contact_email': '',
      'contact_password': '',
      'contact_status': 'Active',
      'login_enabled': false,
      'stopsurveys': false,
      'contact_status_id': 0,
      'facebook': '',
      'twitter': '',
      'linkedin': '',
    };
  }

  addNewToList(event) {
    this.addnewDisplay = false;
    this.loadData();
  }

  updateContact(event) {
    this.loadData();
  }

  delete_contact(contact) {
    CONFIG.LOG(contact.contact_id, 'contact in delete contact');
    this.cf.confirm({
      message: 'Are you sure to delete this contact?',
      accept: () => {
        this.cs.deleteObject<DataExtra>(this.getUrl(this.postUrl) + '/' + contact.contact_id)
          .subscribe((res) => {

            if (this.CheckResponseMessage(res.data)) {
              this.loadData();
            } else {
              this.store.dispatch(new fromMessage.PushWarnMessage(res.data));
            }
          });
      },
      reject: () => {

      }
    });
  }

  open_privileges() {
    this.win.boingNesi1('/sections/hr/member_access/index.aspx?type=contact&business_unit_id=0', 'nesi privileges');
  }
}