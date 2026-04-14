import { Component, OnInit, Input, ViewChild, EventEmitter, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';
import { QuoteEditGeneralCustomerComponent } from '../quote-edit-general-customer/quote-edit-general-customer.component';
import { QuoteEditGeneralDetailComponent } from 'app/pages/quotes/quote-edit-general-detail/quote-edit-general-detail.component';
import { analyzeAndValidateNgModules } from '@angular/compiler';
import { resolve } from 'dns';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-edit-general',
  templateUrl: './quote-edit-general.component.html',
  styleUrls: ['./quote-edit-general.component.css']
})
export class QuoteEditGeneralComponent extends QuoteEditFormBase implements OnInit {
  @ViewChild(QuoteEditGeneralCustomerComponent)
  customer: QuoteEditGeneralCustomerComponent;
  @ViewChild(QuoteEditGeneralDetailComponent)
  detial: QuoteEditGeneralDetailComponent;

  @Output()
  QuoteChanged = new EventEmitter();

  get is_read_for_print() {
    return !this.dirty && this.valid;
  }



  @Input()
  quoted_price: number;


  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    private route: ActivatedRoute,
  ) {
    super(winRef, store, cs, ts);
    // super.Init(CONFIG.apiURL.page.quotes.new);
  }
  createForm() {
    this.userform = this.fb.group({
    });

    this.initFormvalue = {
    }
  }

  ngOnInit() {

  }

  checkValid() {
    if (!this.valid) {
      this.detial.formSubmitAttempt = true;
    }
  }

  public get valid(): boolean {
    if (this.edit_disabled) {
      return true;
    }
    return this.detial.userform.valid;
  }

  public get dirty(): boolean {

    return this.detial.userform.dirty;
  }

  quoteChanged(event: any) {
    if (event) {
      this.QuoteChanged.emit(event);
    }
  }

  validationMediator(event: any) {
    let result = this.detial.validation();
    const mediator = <any>event;
    if (result) {
      mediator.resolve(true);
    } else {
      mediator.reject(true);
    }

  }

  addressChanged(event) {
    super.LOG(event, 'on address changed quote-edit-general');
    this._q.address_id = event.address_id;
    this.detial._q.address_id = event.address_id;
    this.detial.selectedAddressId = event.address_id;
    //  this.detial.loadContacts();
  }

  onCustomerChanged(event: any) {
    super.LOG(event, 'on customer changed quote-edit-general');
    this._q.address_options = event.addresses;
    this._q.customer_id = event.customer_id;
    this._q.address_id = event.address_id;
    this._q.bdm = event.bdm;
    this._q.customer_name = event.customer_name;

    this.customer.initQ();
    this.CustomerChanged.emit(event);
  }

  onContactChanged(event: any) {
    super.LOG(event, 'on contact changed quote-edit-general');
    if(event.contact_id)
    {
      this.q.contact_id = event.contact_id;
      this.cs.getObject<any>(CONFIG.apiURL.page.quotes.editContact + this.q.contact_id)
        .subscribe(
          (res: any) => {
            this.q.contact_extension = res.contact_extension;
            this.q.contact_cellPhone = res.contact_cellPhone;
            this.q.contact_email = res.contact_email;
          },
          (err:any)=>{
            this.PushErrorMessage(err);
          }
        );
    }
    else
    {
      this.q.contact_extension = '';
      this.q.contact_cellPhone = '';
      this.q.contact_email = '';
    }
  }

  onOpportunityChanged(event: any) {
    super.LOG(event, 'on Opportunity changed quote-edit-general');
    this.q.opportunity_internal_id = event.opportunity_internal_id;
  }
}
