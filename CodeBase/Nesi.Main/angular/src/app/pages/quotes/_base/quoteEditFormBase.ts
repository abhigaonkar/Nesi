import { WindowRef } from '../../../services/shared/windowRef';
import { CONFIG } from '../../../configuration';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { QuoteFormBase } from './quoteFormBase';
import { Input, ViewChild, EventEmitter, Output } from '@angular/core';
import { TokenService } from '../../../services/authentication/tokenService';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { NewCustomerContactComponent } from '../../../components/shared/new-customer-contact/new-customer-contact.component';
import { PostResult } from '../../../models/core/postResult';
import { DataExtra } from '../../../models/core/dataExtra';
import { FormArray, FormGroup } from '@angular/forms';
import { ResponsiveModule } from 'ng2-responsive';
import { AbstractControl } from '@angular/forms/src/model';
import * as DATE from '../../../services/helper/datetime';
import { window } from 'rxjs/operators/window';

declare var jQuery: any;

export abstract class QuoteEditFormBase extends QuoteFormBase {
  @Output()
  CustomerChanged = new EventEmitter();
  @Output()
  ContactChanged = new EventEmitter();
  @Output()
  RevisionChanged = new EventEmitter();
  @Output() rowUpdated = new EventEmitter();
  @Output() reOrder = new EventEmitter();


  // tslint:disable-next-line:member-ordering
  public _q: any;
  public tooltip: String = '';

  @Input()
  public set q(value: any) {
    if (!this._q) {
      this._q = value;
      this.initQ();
    }

    if (this._q) {
      if (!this._q.is_Qced) {
        this.tooltip = 'Cannot cut work order, customer has not been QC\'ed.';
      } else {
        this.tooltip = '';
      }
    }

  };

  public get q() {
    if (this._q) {
      return this._q;
    } else {
      return null;
    }
  }

  public get edit_disabled(): boolean {
    if (this._q) {
      return this._q.edit_disabled;
    } else {
      return false;
    }
  }

  // tslint:disable-next-line:member-ordering
  public customers: LabelValueInt[];
  public customerAddresses: LabelValueInt[];
  public customerContacts: LabelValueInt[];
  public customerQuotes: any[];
  public customerBDM: number;
  public customerName: string;
  public selectedCustomerId: number;
  public selectedAddressId: number;
  public selectBussinessUnitId: number;

  public addNewContactDisplay = false;
  @ViewChild(NewCustomerContactComponent)
  public newcustomer: NewCustomerContactComponent;
  businessUnits: LabelValueInt[];
  // revenueLines:LabelValueInt[];
  users: LabelValueInt[];
  except_fields = ['customer'];


  constructor(
    protected winRef: WindowRef,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    protected ts: TokenService,
  ) {
    super(winRef, store, cs);
  }


  afterRowEdit(row_id, value) {

  }
  public initQ() {

  }


  public afterCustomerSelected() {

  }
  public filterCustomer(event: any) {
    let customerId = 0;
    let buid=this.ts.currentUser.businessUnitId;
    let quoteId=0;
    let version=1;
    if(this._q && this._q.quote_id)
    {
      quoteId=this._q.quote_id;
    }
    if (this._q && this._q.quoted_business_unit_id)
    {
      buid = this._q.quoted_business_unit_id;
    }
    if(this.selectBussinessUnitId)
    {
      buid=this.selectBussinessUnitId;
    }
    if (this._q && this._q.customer_id && this.selectBussinessUnitId == this._q.quoted_business_unit_id)
    {
      customerId = this._q.customer_id;
    }

    if (this._q && this._q.revision)
    {
      version = this._q.revision;
    }

    this.cs.postList<LabelValueInt>(CONFIG.apiURL.page.quotes.newCustomerFilter, { data: event.query, data2: customerId,data3:buid,data4:quoteId,data5:version})
      .subscribe(
        (res: LabelValueInt[]) => {
          this.customers = res;
          this.customerAddresses = null;
          this.customerContacts = null;
          this.customerQuotes = null;
          this.customerName = '';
        },
        (err:any)=>{
          super.PushErrorMessage(err);
        });
  }

  public customerSelected(event: any, customerIdFieldName: string = '') {
    super.LOG(event, 'event object of customer selected in quote-addnew-pane');
    this.selectedCustomerId = event.value;
    super.LOG(customerIdFieldName, 'customerID field name of customer selected in quote-addnew-pane');

    if (customerIdFieldName !== '') {
      this.userform.get(customerIdFieldName).setValue(this.selectedCustomerId);
      // tslint:disable-next-line:max-line-length
      super.LOG(this.userform.get(customerIdFieldName).value, 'set value of customerID field object of customer selected in quote-addnew-pane');
    }
    this.selectedAddressId = 0;
    this.tryResetFormControlValueByName('customer_address');
    this.tryResetFormControlValueByName('customer_contact');

    if (this.selectedCustomerId) {
      this.cs.getList<any>(CONFIG.apiURL.page.quotes.newCustomerProfile + this.selectedCustomerId)
        .subscribe(
          (res: any) => {
            this.customerAddresses = res.address;
            this.customerContacts = res.contact;
            this.customerQuotes = res.quotes;
            this.customerBDM = res.bdm;          
            this.customerName = res.name;
            
            this.tryResetFormControlValueByName('bdm', this.customerBDM);

            if (this._q) {
              this.CustomerChanged.emit({
                quote_id: this._q.quote_id,
                revision: this._q.revision,
                customer_id: this.selectedCustomerId,
                address_id: this.selectedAddressId,
                addresses: this.customerAddresses,
                bdm: this.customerBDM,
                customerQuotes: this.customerQuotes,
                customer_name : this.customerName
              });
            }
            // res.quote_id = this._q.quote_id;
            // res.revision = this._q.revision;
          },
          (err:any)=>{
            super.PushErrorMessage(err);
          }
        );
    } else {      
      this.customerAddresses = null;
      this.customerContacts = null;
      this.customerBDM = 0;
      this.tryResetFormControlValueByName('customer');
      if (this._q) {
        this.CustomerChanged.emit({
          quote_id: this._q.quote_id,
          revision: this._q.revision,
          customer_id: 0,
          address_id: this.selectedAddressId,
          addresses: this.customerAddresses,
          bdm: this.customerBDM,
          customerQuotes: [],
          customer_name : ''
        });
      }
    }
    this.ContactChanged.emit(
    {
      contact_id:  0,
    });    
  }


  public addContact() {
    if (this._q)
    {
       this.selectedAddressId=this._q.address_id;
    }
    if (this.selectedAddressId > 0) {
      this.newcustomer.addressId = this.selectedAddressId;
      this.newcustomer.reInit(this.selectedCustomerId)
      this.addNewContactDisplay = true;
    }
    else {
      super.PushErrorMessage("You must select a valid Address.");
    }
  }

  public BusinessUnitSelected(event)
  {
    this.selectBussinessUnitId=event.value;
    this.customerSelected({value:0}, "customer_id");
  }

  public addressSelected(event) {
    this.selectedAddressId = event.value;
    // this.userform.get('customer_contact').setValue(null);
    // this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.quotes.newCustomerAddressContact + this.selectedAddressId.toString())
    //   .subscribe(
    //   (res) => {
    //     this.customerContacts = res;
    //   }
    //   );
  }

  public contactSaved(event: PostResult) {
    super.LOG(event, 'event on contact saved');
    this.addNewContactDisplay = false;
    this.cs.getList(CONFIG.apiURL.page.quotes.newCustomerAddressContact + this.selectedCustomerId.toString())
      .subscribe(
        (res: LabelValueInt[]) => {
          this.customerContacts = res;
          if (this.userform.get('customer_contact')) {
            this.userform.get('customer_contact').setValue(event.result);
          }
        }
      ),
      (err:any)=>{
        super.PushErrorMessage(err);
      };
  }

  public contactCancel() {
    this.addNewContactDisplay = false;
  }

  public loadUserList(event: any, userControlName: string) {
    if (this.userform.get(userControlName) && this.userform.get(userControlName).disabled) {
      return;
    }
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.quotes.newUserList + event.value)
      .subscribe(
        (res: LabelValueInt[]) => {
          this.users = res;
          if (Number(event.value) === this.ts.currentUser.businessUnitId) {
            this.userform.get(userControlName).setValue(this.ts.currentUser.id);
          } else {
            this.userform.get(userControlName).setValue('');
          }
        }
      ),
      (err:any)=>{
        super.PushErrorMessage(err);
      };
  }

  // public loadRevenueLine(event: any, userControlName: string) {
  //   if (this.userform.get("revenueLine") && this.userform.get("revenueLine").disabled) {
  //     return;
  //   }
  //   this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.quotes.NewReveneLine + event.value)
  //     .subscribe(
  //       (res: LabelValueInt[]) => {
  //         this.revenueLines = res;
  //         if (Number(event.value) === this.ts.currentUser.businessUnitId) {
  //           this.userform.get("revenueLine").setValue(9);
  //         } else {
  //           this.userform.get("revenueLine").setValue(9);
  //         }
  //       }
  //     );
  // }

  public loadChangeEvent() {
    for (const name in this.userform.controls) {
      if (this.except_fields.indexOf(name) > -1) {
        continue;
      }
      const control = this.userform.get(name);
      // super.LOG(control.get(0), 'control in loadchangeevent');
      this.loadControlChangeEvent(control, name);

    }
  }

  public loadFormArrayChangeEvent(arrayName: string, type = 1) {
    const array = <FormArray>this.userform.get(arrayName);
    // tslint:disable-next-line:forin
    for (let i = 0; i < array.controls.length; i++) {
      const group = array.controls[i] as FormGroup;
      // tslint:disable-next-line:forin
      for (const name in group.controls) {
        const control = group.get(name);
        const line_id = group.get('row_id').value;
        if (control) {
          this.loadControlChangeEvent(control, name, line_id, type);
        }
      }
    }
  }

  public loadControlChangeEvent(control, name: string, line_id = 0, type = 1) {
    control.valueChanges
      .debounceTime(1000)
      .distinctUntilChanged()
      .subscribe(
        (value: any) => {
          control.updateValueAndValidity();
          if (!control.valid && !control.dirty) { return; }
          if (value !== this._q[name]) {
            this.updateField(name, value, line_id, type, control);
          }
        }
      ),
      (err:any)=>{
        super.PushErrorMessage(err);
      };
  }


  updateField(fieldName: string, value: any, line_id = 0, type = 1, control = null) {

    this._q[fieldName] = value;

    if (fieldName === 'line_text') {
      if (type === 1) {
        fieldName = 'detailrow';
      } else {
        fieldName = 'notesrow';
      }
    }
    if (fieldName === 'section_id') {
      return;
    }
    const postData = {
      quoteId: this._q.quote_id,
      revision: this._q.revision,
      fieldName: fieldName,
      fieldValue:
        value instanceof Array
          ? value && value.length > 0 ? value[0] : 0
          : value || 0,
      ticks: this._q.ts_ticks || 0,
      line_id: line_id || 0
    };
    super.LOG(postData, 'postData in updateField quote-edit-general-details');
    const url = CONFIG.apiURL.page.quotes.editUpdate;
    this.submitting = true;
    this.cs.postDataExtra(url, postData).subscribe(
      (data: DataExtra) => {
        const res = String(data.data);

        if (this.CheckResponseMessage(res)) {
          if (fieldName === 'detail_row_is_checked') {
            // this._q.div1 = data.extra.div;
            this.reOrder.emit(data.extra.sections);
          }
          if (fieldName === 'notes_row_is_checked') {
            // this._q.div2 = data.extra.div;
            this.reOrder.emit(data.extra.sections);
          }
          if (fieldName === 'detailrow' || fieldName === 'notesrow') {
            // update div1 or div2 value
            this.afterRowEdit(line_id, value);
            this.rowUpdated.emit({ not_update_price: true });
            // setTimeout(() => {
            //   try {
            //     jQuery('#linetext' + line_id).focus();
            //   } catch { }
            // }, 700);
            // return;
          }
          if (fieldName === 'customer_id') {
            this.CustomerChanged.emit(
              {
                quote_id: this._q.quote_id,
                revision: this._q.revision
              }
            );
          }

          if (fieldName === 'customer_contact') {
            this.ContactChanged.emit(
              {
                contact_id: this.userform.get('customer_contact').value,
              }
            );
          }
          if (fieldName === 'do_revision') {
            this.RevisionChanged.emit(
              {
                quote_id: this._q.quote_id,
                revision: data.extra,
              }
            );
          }
          if (fieldName === 'verified_date' || fieldName === 'last_fax_date'||fieldName ==='last_print_date') {
            setTimeout(() => {
              location.reload();
            }, 500);
          }
        } else {
          super.PushErrorMessage(res);
        }
        this.submitting = false;
      }
    ),
    (err:any)=>{
      super.PushErrorMessage(err);
    };
  }


  update_date(type) {
    const value = DATE.ToyyyyMMddHHmmss(new Date());
    this.updateField(type, value);
    this._q[type] = value;
    if (type === 'last_fax_date' && this._q.status_id === 2) {
      this._q.status_id = 3;
    }
    if (type === 'verified_date' && this._q.status_id === 3) {
      this._q.status_id = 4;
    }
    if (type === 'last_print_date' && this._q.status_id === 1) {
      this._q.status_id = 2;
    }
  }


  protected tryResetFormControlValueByName(formControlName: string, value?: any) {
    const formControl = this.userform.get(formControlName);
    if (formControl) {
      formControl.reset(value);
    }
  }
}
