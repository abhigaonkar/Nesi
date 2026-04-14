import { Input, Output, EventEmitter } from '@angular/core';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { FormMessageBase } from 'app/core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../../reducers';
import { CONFIG } from 'app/configuration';
import { FormArray } from '@angular/forms';
import { FormBuilder } from '@angular/forms/src/form_builder';
import { PickListGroup } from 'app/models/picklist/picklistGroup';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs';
import { map, filter, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import './pickList-constains';
import { cost_level_tips } from './pickList-constains';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';

export class PickListSearchbase extends FormMessageBase implements OnInit {
  @Input()
  buId: number;
  allitems: LabelValueInt[];
  @Input()
  quote_id: number;
  @Input()
  revision: number;
  @Input()
  section_id: number;
  @Output() close = new EventEmitter();
  @Input()
  isDropdown = true;
  selected_sections = [];

  get rows(): FormArray {
    return <FormArray>this.userform.get('rows');
  }

  get filter_rows(): FormArray {
    return <FormArray>this.userform.get('filter_rows');
  }

  searchString = new Subject<string>();
  searchString$ = this.searchString.asObservable();
  queryAction: Observable<LabelValueInt[]>;

  value: number;
  label: string;

  text: any;
  dialogDisplay = false;
  placeholder = '';
  getCustomerUrl: string;
  getAllUrl: string;
  getListUrl: string;
  searchUrl: string;

  selectItem: any;
  customers: LabelValueInt[];
  selectCustomer = 0;
  checkedAll = true;
  sourceName: string;
  submitEnabled = true;
  cost_count = 0;
  public search_results = [];

  constructor(
    protected fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);

    this.queryAction = this.searchString$
    .filter(text => text.length >= 3)
    .debounceTime(20)
    .distinctUntilChanged()
    .switchMap(v => {
      return this.getSearch2(v);
    });

    this.queryAction.subscribe(res =>  {
      this.cs.hideLoading();
      CONFIG.LOG(res, 'search result => ');
      this.allitems = this.removeEmptyValue(res);
    });
  }

  ngOnInit() {
    if (this.getCustomerUrl) {
      this.cs.getList<LabelValueInt>(this.getCustomerUrl + this.buId)
        .subscribe(
          (res: LabelValueInt[]) => {
            this.customers = res;
            if (this.sourceName) {
              this.customers.unshift({ label: `-Search ${this.sourceName} in all customers-`, value: 0 });
            }
          }
        );
    } else if (this.isDropdown) {
      this.getSearch();
    }
  }



  createForm() {
    this.userform = this.fb.group({
      'rows': this.fb.array([]),
      'filter_rows': this.fb.array([]),
    });
  }

  submitBefore() {
    if (this.sourceName !== 'Quote') {
      if (this.userform.get('rows')) {
        this.submitedValue = this.userform.get('rows').value;
        if (this.submitedValue) {
          this.submitedValue = this.submitedValue.filter(x => x.is_checked && x.qty > 0 && x.master_id);
        }
      } else {
        this.submitedValue = this.userform.value;
      }
    } else {
      if (this.userform.get('filter_rows')) {
        this.submitedValue = this.userform.get('filter_rows').value;
        if (this.submitedValue) {
          this.submitedValue = this.submitedValue.filter(x => x.is_checked && x.qty > 0 && x.master_id);
        }
      } else {
        this.submitedValue = this.userform.value;
      }
    }
    this.postUrl = this.replaceURL(this.postUrl);
  }

  submitValidate(): boolean {
    if (this.submitedValue) {
      let total = 0;
      this.submitedValue.forEach(element => {
        if (element.is_checked && element.qty && element.qty > 0) {
          total += element.qty;
        }
      });
      if (total === 0) {
        this.submitting = false;
        super.PushWarnMessage('You have to input the Qty of the selections.');
        return false;
      }
    }
    return true;
  }

  submitSuccess() {
    this.dialogDisplay = false;
  }


  select(event: any) {
    this.ClearMessage();
    const value = event.value;
    CONFIG.LOG(value, 'select on pick-search-base');
    this.getList(value);
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
    this.cs.getList<PickListGroup>(url)
      .subscribe(
        (res: PickListGroup[]) => {
          this.submitting = false;
          CONFIG.LOG(res, 'list of select items in search base');
          if (res && res.length > 0) {
            this.dialogDisplay = true;
            res = res.filter(x => !!x.master_id);
            this.setRows(res);
            this.setFilterRows(true);
          } else {
            this.dialogDisplay = false;
            this.PushWarnMessage('There are no usable parts.');
          }
        }
      );
  }

  customerSelect(event: any) {
    this.ClearMessage();
    this.selectItem = null;
    CONFIG.LOG(this.selectCustomer, 'select customer in customer select');
    if (this.selectCustomer !== 0) {
      this.isDropdown = true;
      this.getSearch();
    } else {
      this.text = '';
      this.isDropdown = false;
    }
  }

  search(event: any) {
    const query = event.query;
    CONFIG.LOG(query, 'search on pick-search-base');
    if (!this.searchValidate(query)) {
      return;
    }

    // this.getSearch(query);
    this.searchString.next(query);
  }

  searchValidate(query: string): boolean {
    return true;
  }

  getSearch(query: string = null) {

    let url = ''
    let ob: Observable<LabelValueInt[]>;
    this.cs.showLoading();
    CONFIG.LOG(this.isDropdown, 'is dropdwon in get all picklist search base');
    if (this.isDropdown) {
      url = this.replaceURL(this.getAllUrl, query);
      ob = this.cs.getList<LabelValueInt>(url)
    } else {
      url = this.replaceURL(this.searchUrl);
      ob = this.cs.postList<LabelValueInt>(url, { data: query });
    }
    ob.subscribe(
      (res) => {
        this.cs.hideLoading();
        this.allitems = this.removeEmptyValue(res);
      }
    );
  }

  getSearch2(query: string = null) {

    let url = ''
    let ob: Observable<LabelValueInt[]>;
    this.cs.showLoading();
    CONFIG.LOG(this.isDropdown, 'is dropdwon in get all picklist search base');
    if (this.isDropdown) {
      url = this.replaceURL(this.getAllUrl, query);
      ob = this.cs.getList<LabelValueInt>(url)
    } else {
      url = this.replaceURL(this.searchUrl);
      ob = this.cs.postList<LabelValueInt>(url, { data: query });
    }

    return ob;
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
    if (this.selectCustomer && this.selectCustomer > 0) {
      url = url.replace('$custId', this.selectCustomer.toString());
    }
    if (value) {
      url = url.replace('$value', String(value));
    }
    return url;
  }

  setRows(res: any[]) {
    res.forEach(item => {
      item.section_id = this.section_id;
    });
    this.search_results = res;
    const rs = res.map(item => this.fb.group(item));
    const rowsFormArray = this.fb.array(rs);
    this.userform.setControl('rows', rowsFormArray);

  }

  setFilterRows(reset_selected = false) {
    let fs = this.search_results;
    if (reset_selected) {
      this.selected_sections = this.sectionList.map(x => x.value);
    }
    if (this.selected_sections && this.selected_sections.length > 0) {
      fs = this.search_results.filter(x =>
        this.selected_sections.includes(x.section_name)
      );
    } else {
      fs = [];
    }

    const rs = fs.map(item => this.fb.group(item));
    const rowsFormArray = this.fb.array(rs);
    this.userform.setControl('filter_rows', rowsFormArray);
  }

  get sectionList(): any[] {
    const o = Array.from(new Set(this.search_results.map(x => x.section_name)));
    // o = [{ label: 'All sections', value: null }, ...o];
    return o.map(x => ({ label: x, value: x }));
  }

  checkAll(event: any) {
    const chk = event;
    CONFIG.LOG(chk, 'is_checked in checkall quote event searc base');
    for (let i = 0; i < this.rows.length; i++) {
      this.filter_rows.controls[i].get('is_checked').setValue(chk);
    }
    for (let i = 0; i < this.rows.length; i++) {
      this.rows.controls[i].get('is_checked').setValue(chk);
    }
  }

  getCost_ToolTip(level: number) {
    return cost_level_tips[level];
  }

  costStart() {
    this.cost_count++;
    CONFIG.LOG(this.cost_count, 'coust count value in cost start load');
  }

  costLoaded(event: any, i: number) {
    this.cost_count--;
    CONFIG.LOG(this.cost_count, 'coust count value in cost start load');
    if (event.item.master_id) {
      const qty = this.rows.controls[i].get('qty').value;
      if (qty) {
        event.item.qty = qty;
      }
      this.rows.controls[i].patchValue(event.item);
    }
    this.rows.controls[i].get('section_id').setValue(this.section_id);
  }


  getCostURL(master_id: number, qty: number) {
    return CONFIG.apiURL.page.shared.pickList.quoteInventory
      + this.quote_id + '/' + this.revision + '/' + master_id.toString() + '/' + qty.toString() + '/0';
  }

  textFocus(event: any) {
    event.target.select();
  }

}
