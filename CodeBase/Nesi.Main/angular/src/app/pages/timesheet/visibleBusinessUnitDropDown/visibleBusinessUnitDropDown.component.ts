import { Component, OnInit, Output, EventEmitter, forwardRef, ViewChild, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, NgModel, ControlValueAccessor } from '@angular/forms';
import { ValueAccessorBase } from '../../../services/shared/value-accessor';
import { VisibleBusinessUnitDropDown } from '../../../models/Shared/visibleBusinessUnit';
import { CoreService } from '../../../services/shared/core.service';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { Dropdown } from 'primeng/primeng';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import * as DATE from '../../../services/helper/datetime';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromMessage from '../../../actions/layout/growlMessage';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timeSheetvisibleBusinessUnitDropDown',
  templateUrl: './visibleBusinessUnitDropDown.component.html',
  styleUrls: ['./visibleBusinessUnitDropDown.component.css'],
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => TimeSheetVisibleBusinessUnitDropDownComponent), multi: true }]
})
export class TimeSheetVisibleBusinessUnitDropDownComponent extends ValueAccessorBase<number> implements OnInit {

  visibleBusinessUnitList: VisibleBusinessUnitDropDown[];
  selectBusinessUnit: VisibleBusinessUnitDropDown;

  value: number;

  @Output() onChange = new EventEmitter();

  @ViewChild(NgModel) model: NgModel;

  @ViewChild(Dropdown)
  dropdown: Dropdown;
  @Input()
  disabled = true;

  options: any[];
  constructor(
    private tss: TimesheetService,
    protected store: Store<fromRoot.State>,
  ) {
    super();
  }

  public resetFilter() {
    this.dropdown.resetFilter();
  }
  ngOnInit() {

    this.tss.TimesheetVisibleBusinessUnitDropDownList()
      .subscribe((res: VisibleBusinessUnitDropDown[]) => {
        this.initDropDownList(res);
        this.tss.visibleBusinessUnitList = this.visibleBusinessUnitList;
      },(err:any)=>{
        this.store.dispatch(new fromMessage.PushErrorMessage(err));
      });

  }

  private initDropDownList(res: VisibleBusinessUnitDropDown[]) {
    this.visibleBusinessUnitList = res;
    this.converToDropDown();
    this.disabled = false;
  }

  private converToDropDown() {
    this.options = [];
    this.visibleBusinessUnitList.forEach(x => {
      this.options.push({ label: x.ddl_name, value: x.id });
    });
  }

  public getBusinessUnitNameById(id: number) {
    return this.visibleBusinessUnitList.find(x => x.id === id).ddl_name;
  }

  onchanged(event: any) {
    // CONFIG.LOG(this.value, 'select businessUnit Id');
    this.selectBusinessUnit = this.visibleBusinessUnitList.find(x => x.id === this.value);
    event.value = this.value;
    event.selectBusinessUnit = this.selectBusinessUnit;
    this.onChange.emit(event);
  }
}
