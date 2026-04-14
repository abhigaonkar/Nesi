import { Component, OnInit, Input } from '@angular/core';
import { FormMessageBase } from 'app/core/formMessageBaseComponent';
import * as fromRoot from '../../../reducers';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';
import { Store } from '@ngrx/store';
import { CONFIG } from 'app/configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'nesi-quote-add-section-from-quote',
  templateUrl: './quote-add-section-from-quote.component.html',
  styleUrls: ['./quote-add-section-from-quote.component.css']
})
export class QuoteAddSectionFromQuoteComponent extends FormMessageBase implements OnInit {
  selectCustomer: number;
  customers: LabelValueInt[];
  isDropdown = true;
  text: any;
  buId: number;
  getCustomerUrl: string;
  allitems: any[];
  selectItem: any;
  label: string;
  getAllUrl: string;
  searchUrl: string;
  getListUrl: string;
  dialogDisplay = false;
  @Input() quote_id: number;
  @Input() revision: number;
  @Input() type: number;
  checkedAll = false;

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,

  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.quotes.saveNewIds + '$quote_id/$revision/$type');
    this.getCustomerUrl = CONFIG.apiURL.page.shared.pickList.customers;
    this.searchUrl = CONFIG.apiURL.page.shared.pickList.queryQuotes;
    this.getAllUrl = CONFIG.apiURL.page.shared.pickList.customersQuotes + '$buId/$custId';
    // todo:
    this.getListUrl = CONFIG.apiURL.page.shared.pickList.quoteSections + '$value';
    this.buId = this.ts.currentUser.businessUnitId;
  }


  get rows(): FormArray {
    return <FormArray>this.userform.get('rows');
  }

  ngOnInit() {
    if (this.getCustomerUrl) {
      this.cs.getList<LabelValueInt>(this.getCustomerUrl + this.buId)
        .subscribe(
        (res: LabelValueInt[]) => {
          this.customers = res;
          this.customers.unshift({ label: `-Search Quote in all customers-`, value: 0 });
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
        );
    } else if (this.isDropdown) {
      this.getSearch();
    }
  }


  createForm() {
    this.userform = this.fb.group({
      'rows': this.fb.array([]),
    });
  }

  submitBefore() {
    if (this.userform.get('rows')) {
      this.submitedValue = this.userform.get('rows').value;
      if (this.submitedValue) {
        this.submitedValue = this.submitedValue.filter(x => x.is_checked);
      }
    } else {
      this.submitedValue = this.userform.value;
    }
    this.postUrl = this.replaceURL(this.postUrl);
  }

  submitValidate(): boolean {
    if (this.userform.get('rows')) {
      if (this.submitedValue.length === 0) {
        this.submitting = false;
        super.PushWarnMessage('No section has been selected.');
        return false;
      }
    }
    return true;
  }


  submitSuccess() {
    this.dialogDisplay = false;
    this.submitting = false;
  }

  customerSelect(event: any) {
    this.ClearMessage();
    if (this.selectCustomer !== 0) {
      this.isDropdown = true;
      this.getSearch();
    } else {
      this.text = '';
      this.isDropdown = false;
    }
  }

  select(event: any) {
    this.ClearMessage();
    const value = event.value;
    this.getList(value);
  }

  search(event: any) {
    const query = event.query;
    if (!this.searchValidate(query)) {
      return;
    }
    this.getSearch(query);
  }


  searchValidate(query: string): boolean {
    return true;
  }

  getSearch(query: string = null) {

    let url = ''
    let ob: Observable<LabelValueInt[]>;
    if (this.isDropdown) {
      url = this.replaceURL(this.getAllUrl, query);
      ob = this.cs.getList<LabelValueInt>(url)
    } else {
      url = this.replaceURL(this.searchUrl);
      ob = this.cs.postList<LabelValueInt>(url, { data: query });
    }
    ob.subscribe(
      (res) => {
        this.allitems = this.removeEmptyValue(res);
      },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
    );
  }

  replaceURL(url: string, value: any = null): string {
    if (this.quote_id) {
      url = url.replace('$quote_id', this.quote_id.toString());
    }
    if (this.revision) {
      url = url.replace('$revision', this.revision.toString());
    }
    if (this.buId) {
      url = url.replace('$buId', this.buId.toString());
    }
    if (this.type) {
      url = url.replace('$type', this.type.toString());
    }
    if (this.selectCustomer && this.selectCustomer > 0) {
      url = url.replace('$custId', this.selectCustomer.toString());
    }
    if (value) {
      let v: string = String(value);
      const q = v.substr(0, 6);
      const rev = v.substr(6);
      v = q + '/' + rev;
      url = url.replace('$value', v);
    }
    return url;
  }

  getList(value: string) {
    if (value && !isNaN(+value)) {
      this.label = this.GetLabelByValueFromLabelValueInt(this.allitems, +value);
    }
    if (!this.getListUrl) {
      return;
    }
    const url = this.replaceURL(this.getListUrl, value);
    this.submitting = true;
    this.cs.getList<any>(url)
      .subscribe(
      (res: any[]) => {
        this.submitting = false;
        CONFIG.LOG(res, 'list of select items in search base');
        if (res && res.length > 1) {
          this.dialogDisplay = true;
          res = res.slice(1);
          this.setRows(res);
        } else {
          this.dialogDisplay = false;
          this.PushWarnMessage('There are no sections in this quote.');
        }
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }

  setRows(res: any[]) {
    res.forEach(item => {
      item.is_checked = false;
      item.label = decodeURIComponent(item.label)
    });
    const rs = res.map(item => this.fb.group(item));
    const rowsFormArray = this.fb.array(rs);
    this.userform.setControl('rows', rowsFormArray);

  }

  checkAll(event: any) {
    const chk = event;
    for (let i = 0; i < this.rows.length; i++) {
      this.rows.controls[i].get('is_checked').setValue(chk);
    }
  }


}
