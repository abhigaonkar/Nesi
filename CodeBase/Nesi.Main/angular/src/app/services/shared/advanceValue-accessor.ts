import { ValueAccessorBase } from './value-accessor';
import { Output, EventEmitter, ViewChild, Input } from '@angular/core';
import { NgModel } from '@angular/forms';
import { CONFIG } from '../../configuration';
import { CoreService } from './core.service';
import { Dropdown, SelectItem } from 'primeng/primeng';

export abstract class AdvanceValueAccessorBase<T> extends ValueAccessorBase<T> {

  public apiURL: string;

  @Input()
  public options: any[];

  public items: any[];

  public cs: CoreService;


  @Output() onChange = new EventEmitter();

  @ViewChild(NgModel) model: NgModel;

  @ViewChild(Dropdown)
  dropdown: Dropdown;

  @Input()
  required = false;
  @Input()
  editable = false;

  @Input()
  filter = true && !this.editable;

  @Input()
  disabled = false;
  @Input()
  valueField = 'value';
  @Input()
  labelField = 'label';
  @Input()
  set URL(value: string) {
    if (value) {
      CONFIG.LOG(value, 'binddropdown url');
      this.apiURL = value;
      this.getList();
    }
  }

  @Output()
  OnAfterLoadUrl = new EventEmitter();


  public get loaded() {
    return this.options && this.options.length > 0;
  }
  public selectedItem: any;

  public getList(url?: string) {
    if (url) {
      this.apiURL = url;
    }
    CONFIG.LOG(this.apiURL, 'apiurl getlist dropdown list');
    this.dropdown.filterValue = '';
   // this.dropdown.resetFilter();

    this.cs.getList(this.apiURL)
      .subscribe((res: any[]) => {
        this.items = res;
        this.loadDropDown();
        this.OnAfterLoadUrl.emit();
      });
  }

  public getList_WO(url?: string) {
    if (url) {
      this.apiURL = url;
    }
    CONFIG.LOG(this.apiURL, 'apiurl getlist dropdown list');
    this.dropdown.filterValue = '';
   // this.dropdown.resetFilter();

    this.cs.getList(this.apiURL)
      .subscribe((res: any[]) => {
        this.items = res;
        this.loadDropDown();
        this.OnAfterLoadUrl.emit();    
      });
  }

  public loadDropDown() {
    if (!this.items && this.options && this.options.length > 0) {
      this.items = this.options;
    }
    this.options = [];
    if (!this.items) { return; }
    this.items.forEach(x => {
      this.options.push({ label: x[this.labelField], value: x[this.valueField] });
    });
    CONFIG.LOG(this.required, 'required in dropdown');
    if (this.required) {
      if(this.options.length == 0) {
        //
        // Found this bug when checking issue on Release Preview build.
        // No good to assume the list of 'options' always has items. So when there are no items, we should do nothing.
        // By doing so, we can avoid errors arising from here which froze the timesheet page if no hours type returned from server (for nesi 100).
        // So this checking will apply to all the code using this control, and make it work well.
        //
      } else {
        this.value = this.options[0].value;
      }
    } else {
      this.value = null;
    }
    this.dropdown.filterValue = '';
  //  this.dropdown.resetFilter();
  }

  public showOne(label: any, value: any) {
    this.options = [];
    this.options.push({ label: label, value: value });
    this.value = value;
    this.disabled = true;
  }

  public clear() {
    this.options = [];
    this.value = null;
  }

  onchanged(event: any) {
    event.value = this.value;
    if (!this.items && this.options && this.options.length > 0) {
      this.items = this.options;
    }
    this.selectedItem = this.items.find(x => x.value === this.value);
    if (this.selectedItem) {
      event.item = this.selectedItem;
      event.label = this.selectedItem.label;
    }
    this.onChange.emit(event);
  }

  public getSelectedItemProp(propName: string) {
    CONFIG.LOG(this.selectedItem, 'selected item getSelectedItemProp');
    return this.selectedItem && this.selectedItem[propName];
  }
  public getLabelNameByValue(value: string) {
    if (!this.options) {
      return null;
    }
    const item = this.options.find(x => x.value.toString() === value);

    return item && item.label;
  }



}
