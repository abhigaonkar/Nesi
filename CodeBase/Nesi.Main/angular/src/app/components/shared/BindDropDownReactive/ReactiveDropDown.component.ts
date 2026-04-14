import { Component, OnInit, Output, EventEmitter, forwardRef, ViewChild, Input } from '@angular/core';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { NG_VALUE_ACCESSOR, NgModel, ControlValueAccessor } from '@angular/forms';
import { ValueAccessorBase } from '../../../services/shared/value-accessor';
import { AdvanceValueAccessorBase } from '../../../services/shared/advanceValue-accessor';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-ReactiveDropDown',
  template: `
  <p-dropdown  [style]="{'width':'100%'}" [placeholder]="placeholder || '&nbsp;'"
  [options]="options" [(ngModel)]="value"
  (onChange)="onchanged($event)"
  [filter]="filter && (options && options.length >5)"
  [required]="required"
  >
  <ng-template let-item pTemplate="item">
    <div>{{item.label}}</div>
  </ng-template>
  </p-dropdown>
  `,
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => ReactiveDropDownComponent), multi: true }]

})
export class ReactiveDropDownComponent extends AdvanceValueAccessorBase<any> implements OnInit {
  @Input()
  placeholder: string;

  constructor(
    coresvs: CoreService,
  ) {
    super();
    this.cs = coresvs;
  }
  ngOnInit() {

  }
}
