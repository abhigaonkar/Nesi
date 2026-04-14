import {Component, OnInit, Input, EventEmitter, Output} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-edit-general-detail',
  templateUrl: './quote-edit-general-detail.component.html',
  styleUrls: ['./quote-edit-general-detail.component.css']
})
export class QuoteEditGeneralDetailComponent extends QuoteEditFormBase implements OnInit {

  showPriceToTextBox = false;
  showCustomerNameTextBox = true;
  minDate:Date;
  status:number;
  chance_winning_list = [
    { label: '90% - We are the only contractor', value: '90' },
    { label: '60% - We are one of 3 contractors', value: '60' },
    { label: '30% - Budget quote or bid tender', value: '30' },
  ];

  control2_list = ['completion_date', 'chance_winning', 'chance_winning_reason', 'chance_winning_note', 'quoted_by','quoted_business_unit_id','exp_podate','date_expected_start','period_valid','bdm','acting_bdm','opportunity_internal_id'];

  opportunities: LabelValueInt[];

  @Output() OpportunityChanged = new EventEmitter();

  @Input()
  public set quoted_price(value: number) {
    if (this.userform && this.userform.get('quoted_price')) {
      if (value) {
        this.userform.get('quoted_price').setValue(value);
      } else {
        this.userform.get('quoted_price').setValue(this._q.quoted_price);
      }
    }
  }

  readonly houseAccountId: number = 958927;

  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    private route: ActivatedRoute,
    private cf: ConfirmationService,


  ) {
    super(winRef, store, cs, ts);
    super.Init(CONFIG.apiURL.page.quotes.editUpdateAll);
  }
  createForm() {
    this.userform = this.fb.group({
      'quote_id': '',
      'revision': '',
      'customer': ['', Validators.required],
      'customer_id': ['', Validators.required],
      'customer_name': ['', Validators.required],
      'customer_contact': ['', Validators.required],
      'date_due': ['', Validators.required],
      'date_expected_start': ['', Validators.required],
      'exp_podate': '',
      'chance_winning': ['', Validators.required],
      'chance_winning_reason': '',
      'chance_winning_note': '',
      'completion_date': ['', Validators.required],
      'job_description': ['', [Validators.required]],
      'follow_up': '',
      'is_tm': '',
      'cust_spec_doc': '',
      'quoted_by': ['', Validators.required],
      'quoted_business_unit_id': ['', Validators.required],
       // 'revenue_line_id': ['', Validators.required],
      'opportunity': '',
      'opportunity_internal_id': '',
      'parallel_bid': '',
      'pricetype_id':  [0, Validators.required],
      'quoted_price': ['', [Validators.required, Validators.min(0.01)]],
      'price_to': '',
      'expected_value': ['', [Validators.required, Validators.min(0.01)]],
      'us_currency': '',
      'inflation_term': '',
      'percent_down': '',
      'net_due': '',
      'customer_term': '',
      'address_id': ['', Validators.required],
      'bdm': '',
      'acting_bdm': '',
      'ts_ticks': '',
      'quoter_locked_bool': '',
      'period_valid': ''
    });
  }

  initQ() {
      if(this._q && this._q.pricetype_id==='')
      {
        this._q.pricetype_id=0;
      }
    this.status=Number(this._q.status_id);
    this.customerContacts = this._q.contact_select;
    this.selectedCustomerId = this._q.customer_id;
    this.businessUnits = this._q.company_list;
    // this.revenueLines = this._q.revenueLines;
    this.users = this._q.initial_quoted_by_list;
    this.selectedAddressId = this._q.address_id;
    this.customerBDM = this._q.bdm;
    this.customerName = this._q.customer_name;
    this.showPriceToTextBox = Number(this._q.pricetype_id) === 6;
    this.initFormvalue = {
      'quote_id': this._q && this._q.quote_id,
      'revision': this._q && this._q.revision,
      'customer': {
        label: this._q && '(' + this._q.customer_id + ') ' + this._q.customer_name,
        value: this._q.customer_id
      },
      'customer_id': this._q && this._q.customer_id,
      'address_id': this._q && this._q.address_id,
      'customer_name': this._q && this._q.customer_name,
      'customer_contact': this._q && this._q.customer_contact,
      'date_due': this._q && this._q.date_due,
      'date_expected_start': this._q && this._q.date_expected_start,
      'exp_podate': this._q && this._q.exp_podate,
      'chance_winning': this._q && this._q.chance_winning,
      'chance_winning_reason': this._q && this._q.chance_winning_reason,
      'chance_winning_note': this._q && this._q.chance_winning_note,
      'completion_date': this._q && this._q.completion_date,
      'job_description': this._q && this._q.job_description,
      'follow_up': this._q && this._q.follow_up,
      'is_tm': this._q && this._q.is_tm,
      'cust_spec_doc': this._q && this._q.cust_spec_doc,
      'quoted_by': this._q && this._q.quoted_by,
      'quoted_business_unit_id': this._q && this._q.quoted_business_unit_id,
      // 'revenue_line_id':this._q && this._q.revenue_line_id,
      'opportunity_internal_id': this._q && this._q.opportunity_internal_id > 0 && this._q.opportunity_internal_id || null,
      'parallel_bid': this._q && this._q.parallel_bid,
      'pricetype_id': Number(this._q && this._q.pricetype_id) || 0,
      'quoted_price': this._q && this._q.quoted_price,
      'price_to': this._q && this._q.price_to,
      'expected_value': this._q && this._q.expected_value,
      'us_currency': this._q && this._q.us_currency,
      'inflation_term': this._q && this._q.inflation_term,
      'percent_down': this._q && this._q.percent_down,
      'net_due': this._q && this._q.net_due,
      'customer_term': this._q && this._q.customer_term || '',
      'ts_ticks': this._q && this._q.ts_ticks,
      'quoter_locked_bool': this._q && this._q.quoter_locked_bool || false,
      'period_valid': this._q && this._q.period_valid,
      'bdm': this._q && this._q.bdm || 0,
      'acting_bdm': this._q && this._q.acting_bdm || 0
    }
    this.userform.reset(this.initFormvalue);

    this.validateBusinessFlowFields();
    this.disableFields();
    this.submitReset();
    // this.loadChangeEvent();
  }

  validateBusinessFlowFields() {
    if(this._q && this._q.bdm == 0 ){
      this.userform.get('bdm').markAsTouched({onlySelf: true});
    }
    if(this._q && this._q.quoted_price < 0.01 ){
      this.userform.get('quoted_price').markAsTouched({onlySelf: true});
    }
  }

  ngOnInit() {
    // tslint:disable-next-line:forin
    this.minDate = new Date(this._q && this._q.open_date);
    this.filterOpportunities(this._q && this._q.customer_id);
  }

  contact_changed(event) {
    this.ContactChanged.emit(
      {
        contact_id: this.userform.get('customer_contact').value,
      }
    );
  }

  disableFields() {
    if (this._q && this._q.quoter_locked_bool) {
      this.userform.get('quoted_by').disable();
    }
    if(Number(this._q.status_id)>1)
    {
      this.userform.get('quoted_price').disable();
      this.userform.get('price_to').disable();
      this.userform.get('pricetype_id').disable();
    }
    if(Number(this._q.revision)>1 && this._q.is_tm){
      this.userform.get('is_tm').disable();
    }
    //#2808 Prevent changing of the business unit on a quote IF the quote has integrated into NetSuite
    if(this._q.isIntegratedWithNs)
    {
      this.userform.get('quoted_business_unit_id').disable();
    }
    if (this._q.isBdmReadOnly) {
      this.userform.get('bdm').disable();
      this.userform.get('acting_bdm').disable();
    }

    if (this.disabled_control) {
      // tslint:disable-next-line:forin
      for (const prop in this.userform.controls) {
        CONFIG.LOG(this.control2_list.indexOf(String(prop)), 'prop name in disable fields');
        CONFIG.LOG(this._q.status_id, 'status id  in disable fields');
        if(Number(this._q.status_id) === 2){
          CONFIG.LOG(String(prop), 'log status id');
          if (String(prop) === 'opportunity_internal_id') {
            this.userform.controls[prop].enable();
            CONFIG.LOG(String(prop), 'log opportunity field');
          }
          else{
            this.userform.controls[prop].disable();
            CONFIG.LOG(String(prop), 'log other fields');
          }
        }
        else{
        if (this.control2_list.indexOf(String(prop)) > -1 && !this.disabled_control2) {
          this.userform.controls[prop].enable();
        } else {
          this.userform.controls[prop].disable();
        }
      }
      }
    }
  }

  get disabled_control() {
    return this.edit_disabled;
  }

  get disabled_control2() {
    return Number(this._q.status_id) !== 3 && Number(this._q.status_id) !== 4;
  }

  get disabled_control3() {
    return Number(this._q.status_id) !== 3 && Number(this._q.status_id) !== 4 && Number(this._q.status_id) !== 2;
  }

  formValidateBefore() {
    // tslint:disable-next-line:forin
    if(this.selectedAddressId > 0)
    {
      this.userform.get('address_id').setValue(this.selectedAddressId);
    }
    if (!this.userform.get('exp_podate').value) {
      this.userform.get('exp_podate').setValue(null);
    }
  }

  submitReset() {
    this.formSubmitAttempt = false;
  }

  submitSuccess() {
    this._q.ts_ticks = this.extraData;
    this.userform.get('ts_ticks').setValue(this.extraData);
  }

  submitFailed() {
    this.submitReset();
    switch (this.extraData) {
      case 'refresh':
        this.cf.confirm({
          message: this.responseMessageAfterPost + '<br>Do you want to reload the page?',
          accept: () => {
            window.location.reload();
          },
          reject: () => { }
        });
        break;
      case 'close':
        break;
    }
  }


  lockedChange() {
    if (this.userform.get('quoter_locked_bool').value) {
      this.userform.get('quoted_by').disable();
    } else {
      this.userform.get('quoted_by').enable();
    }
  }


  loadContacts() {
    this.customerContacts = null;
    this.userform.get('customer_contact').setValue(null);
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.quotes.newCustomerAddressContact + this.selectedAddressId)
      .subscribe(
        (res) => {
          this.customerContacts = res;
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
      );
  }

  price_type_changed(event) {
    const price_type = Number(this.userform.get('pricetype_id').value);
    this.showPriceToTextBox = price_type === 6;
  }
  public handle_customer_request(){
    console.log("handle");
    const url = CONFIG.Nesi1URL.requestCustOrVendor.replace('@type', '1').replace('@customer_id',this._q.customer_id)+"&quote_id="+this._q.quote_id+"&rev="+this._q.revision;
    this.winRef.boingNesi1(url ,'customerRequest-'+Math.random(),"500,700");
  }

  validation(): boolean {
    if (this.userform.invalid == true) {
      Object.keys(this.userform.controls).forEach(key => {
        this.userform.controls[key].markAsTouched();
      });

      console.log("NOT validated");
      return false;
    } else {
      console.log("validated");
      return true;
    }
  }

  public customerSelected(event: any, customerIdFieldName: string = '') {
    this.tryResetFormControlValueByName("parallel_bid", false);
    this.tryResetFormControlValueByName("opportunity_internal_id");
    this.tryResetFormControlValueByName('address_id');
    this.tryResetFormControlValueByName('bdm');

    super.customerSelected(event, customerIdFieldName);

    if(event.value)
    {
      this.filterOpportunities(event.value);
    }
    else
    {
      this.filterOpportunities(0);
    }
  }

  public filterOpportunities(customerId : number) {
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.quotes.opportunityFilter + customerId)
      .subscribe(
        (res: LabelValueInt[]) => {
          this.opportunities = res;
        },
        (err: any) => {
          super.PushErrorMessage(err);
        });
  }

  public opportunitySelected(event: any) {
    this.userform.get("opportunity_internal_id").setValue(event.value);
    if(!event.value)
    {
      this.userform.get("parallel_bid").setValue(false);
    }
    if (this._q) {
      this.OpportunityChanged.emit({ opportunity_internal_id: event.value});
    }
  }

  public bdmChanged(event: any): void {
    const newValue = event.value;
   // this.userform.get('acting_bdm').reset(0);
  }
}
