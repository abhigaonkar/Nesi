import { Component, OnInit, Output, EventEmitter, forwardRef, ViewChild, Input } from '@angular/core';
import { VisibleBusinessUnitDropDown } from '../../models/Shared/visibleBusinessUnit';
import { CoreService } from '../../services/shared/core.service';
import { CONFIG } from '../../configuration';
import { NG_VALUE_ACCESSOR, NgModel, ControlValueAccessor } from '@angular/forms';
import { ValueAccessorBase } from '../../services/shared/value-accessor';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-visibleBusinessUnitDropDown',
  templateUrl: './visibleBusinessUnitDropDown.component.html',
  styleUrls: ['./visibleBusinessUnitDropDown.component.css'],
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => VisibleBusinessUnitDropDownComponent), multi: true }]
})
export class VisibleBusinessUnitDropDownComponent extends ValueAccessorBase<number> implements OnInit {
  @Input() required = true;
  visibleBusinessUnitList: VisibleBusinessUnitDropDown[];
  selectBusinessUnit: VisibleBusinessUnitDropDown;

  value: number;

  @Output() onChange = new EventEmitter();

  @ViewChild(NgModel) model: NgModel;


  options: any[];
  constructor(
    public cs: CoreService,
  ) {
    super();
  }

  ngOnInit() {
    this.cs.VisibleBusinessUnitDropDownList()
      .subscribe((res: VisibleBusinessUnitDropDown[]) => {
        this.initDropDownList(res);
        this.cs.visibleBusinessUnitList = this.visibleBusinessUnitList;
      });
  }

  private initDropDownList(res: VisibleBusinessUnitDropDown[]) {
    this.visibleBusinessUnitList = res;
    this.converToDropDown();
  }

  private converToDropDown() {
    this.options = [];
    this.visibleBusinessUnitList.forEach(x => {
      this.options.push({ label: x.ddl_name, value: x.id });
    });
  }

  onchanged(event: any) {
    // CONFIG.LOG(this.value, 'select businessUnit Id');
    this.selectBusinessUnit = this.visibleBusinessUnitList.find(x => x.id === this.value);
    event.value = this.value;
    event.selectBusinessUnit = this.selectBusinessUnit;
    this.onChange.emit(event);
  }
}
