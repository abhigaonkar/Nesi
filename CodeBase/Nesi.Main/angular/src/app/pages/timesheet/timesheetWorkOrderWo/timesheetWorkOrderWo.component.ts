import { Component, OnInit, Output, EventEmitter, forwardRef, ViewChild, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, NgModel, ControlValueAccessor } from '@angular/forms';
import { ValueAccessorBase } from '../../../services/shared/value-accessor';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { TimesheetWorkOrderWo } from '../../../models/pages/timesheet/timesheetWorkOrderWo';
import { AdvanceValueAccessorBase } from '../../../services/shared/advanceValue-accessor';
import { Dropdown } from 'primeng/primeng';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetWorkOrderWo',
  template: `
  <p-dropdown  [editable]="editable"
   [disabled]="disabled || (!editable && !loaded && !(options && options.length>0))"
   [style]="{'width':'100%'}" [placeholder]="'&nbsp;'"
  [options]="options" [(ngModel)]="value"
  (onChange)="onchanged($event)" [required]="required"
  [filter]="filter && (options && options.length >5)">
  <ng-template let-item pTemplate="item">
    <div>{{item.label}}</div>
  </ng-template></p-dropdown>
  `,
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => TimesheetWorkOrderWoComponent), multi: true }]

})

export class TimesheetWorkOrderWoComponent extends AdvanceValueAccessorBase<any> implements OnInit {

  constructor(
    coresvs: CoreService,
  ) {
    super();
    this.cs = coresvs;
  }

  ngOnInit() {

  }

  public filterCustomer(customerId: number) {
    this.options = [];
    //  CONFIG.LOG(JSON.stringify(this.items), 'tiemsheetwork order wo filter customer');
    if (!(this.filter && this.items)) {
      return
    };
    this.items.forEach(x => {
      // CONFIG.LOG(JSON.stringify(x), 'tiemsheetwork order wo filter customer');
      //  CONFIG.LOG(customerId, 'tiemsheetwork order wo filter customer');

      if (x && x['customer_id'] === customerId) {
        this.options.push({ label: x[this.labelField], value: x[this.valueField] });
      }
    });
  }

  onchanged(event: any) {
    event.value = this.value;
    if (this.filter && this.items) {
      event.selectedCustomerId = this.items.find(x => x.value === this.value).customer_id
    }
    event.label = this.options.find(x => x.value === this.value).label;
    event.woList= this.items;
    this.onChange.emit(event);
  }
}
