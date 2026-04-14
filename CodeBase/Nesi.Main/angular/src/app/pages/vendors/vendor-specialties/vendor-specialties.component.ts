
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { DataExtra } from 'app/models/core/dataExtra';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { VendorFormBase } from '../_base/vendorFormBase';

@Component({
  selector: 'nesi-vendor-specialties',
  templateUrl: './vendor-specialties.component.html',
  styleUrls: ['./vendor-specialties.component.css']
})
export class VendorSpecialtiesComponent  extends VendorFormBase implements OnInit {

  @Output() cancel = new EventEmitter();

  selected: any;

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private cf: ConfirmationService,

  ) {
    super(store, cs);
    super.InitVendor(
      CONFIG.apiURL.page.vendors.specialties
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'id': 0,
      'value': ['', [Validators.required, Validators.maxLength(150)]],
    });

    this.initFormvalue = {
      id: 0,
      data: ''
    };
  }

  submitSuccess() {
    this.profile = this.extraData;
    this.selected = null;
  }

  delete(item) {
    this.cf.confirm({
      message: 'Do you really want to delete this item?',
      accept: () => {
        if (item) {
          this.cs.deleteObject<DataExtra>(this.getUrl(this.postUrl) + '/' + item.id)
            .subscribe(
            (res) => {
              if (this.PushResponseMessage(res.data)) {
                this.profile = res.extra;
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
      this.userform.get('id').setValue(event.data.id);
      this.userform.get('value').setValue(event.data.skill);
    }
  }
}
