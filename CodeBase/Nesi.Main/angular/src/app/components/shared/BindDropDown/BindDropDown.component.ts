import { Component, OnInit, Output, EventEmitter, forwardRef, ViewChild, Input } from '@angular/core';
import { VisibleBusinessUnitDropDown } from '../../../models/Shared/visibleBusinessUnit';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { NG_VALUE_ACCESSOR, NgModel, ControlValueAccessor } from '@angular/forms';
import { ValueAccessorBase } from '../../../services/shared/value-accessor';
import { AdvanceValueAccessorBase } from '../../../services/shared/advanceValue-accessor';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-BindDropDown',
  template: `
  <p-dropdown [disabled]="disabled || !(options && options.length >0)" [style]="{'width':'100%'}"
  [options]="options" [(ngModel)]="value"
  (onChange)="onchanged($event)"
  [filter]="filter && (options && options.length >5)">
  <ng-template let-item pTemplate="item">
    <div>{{item.label}}</div>
  </ng-template></p-dropdown>
  `,
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => BindDropDownComponent), multi: true }]

})
export class BindDropDownComponent extends  AdvanceValueAccessorBase<any> implements OnInit {

  constructor(
    coresvs: CoreService,
  ) {
    super();
    this.cs = coresvs;
  }
  ngOnInit() {

  }
}
