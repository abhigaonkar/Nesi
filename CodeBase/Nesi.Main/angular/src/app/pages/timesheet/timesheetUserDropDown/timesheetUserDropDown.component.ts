import { Component, OnInit, forwardRef, EventEmitter, Output, ViewChild, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, NgModel, ControlValueAccessor } from '@angular/forms';
import { ValueAccessorBase } from '../../../services/shared/value-accessor';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { Dropdown } from 'primeng/primeng';
import { CONFIG } from '../../../configuration';

@Component({
  selector: 'nesi-timesheetUserDropDown',
  templateUrl: './timesheetUserDropDown.component.html',
  styleUrls: ['./timesheetUserDropDown.component.css'],
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => TimesheetUserDropDownComponent), multi: true }]

})
export class TimesheetUserDropDownComponent extends ValueAccessorBase<number> implements OnInit {

  @Output() onChange = new EventEmitter();

  @ViewChild(NgModel) model: NgModel;
  @Input()
  disabled = true;

  @ViewChild(Dropdown)
  dropdown: Dropdown;
  value: number;

  options: any[];

  @Input()
  set Options(value: any[]) {
    CONFIG.LOG('reset option value', 'userdrop down timesheet input options');
    this.dropdown.filterValue = '';
    this.options = value;
    this.disabled = false;
  }

  constructor(
  ) {
    super();
  }

  ngOnInit() {
  }

  onchanged(event: any) {
    event.value = this.value;
    this.onChange.emit(event);
  }

}
