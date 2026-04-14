import { Component, OnInit, Output, EventEmitter, forwardRef, ViewChild, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, NgModel, ControlValueAccessor } from '@angular/forms';
import { ValueAccessorBase } from '../../../services/shared/value-accessor';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { AdvanceValueAccessorBase } from '../../../services/shared/advanceValue-accessor';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetWorkOrderCustomer',
  template: `
  <p-dropdown [editable]="editable"
   [disabled]="disabled  || (!editable && !loaded && !(options && options.length >0))"
    [style]="{'width':'100%'}" [placeholder]="'&nbsp;'"
  [options]="options" [(ngModel)]="value"
  (onChange)="onchanged($event)" [required]="required"
  [filter]="filter  && (options && options.length >5)">
  <ng-template let-item pTemplate="item">
    <div>{{item.label}}</div>
  </ng-template></p-dropdown>
  `,
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => TimesheetWorkOrderCustomerComponent), multi: true }]

})

export class TimesheetWorkOrderCustomerComponent extends AdvanceValueAccessorBase<any> implements OnInit {

  constructor(
    coresvs: CoreService,
  ) {
    super();
    this.cs = coresvs;
  }
  ngOnInit() {

  }

}
