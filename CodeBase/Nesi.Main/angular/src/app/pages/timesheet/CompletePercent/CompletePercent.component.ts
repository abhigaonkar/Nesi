import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { ValueAccessorBase } from '../../../services/shared/value-accessor';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'nesi-CompletePercent',
  templateUrl: './CompletePercent.component.html',
  styleUrls: ['./CompletePercent.component.css'],
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => CompletePercentComponent), multi: true }]

})
export class CompletePercentComponent extends ValueAccessorBase<number> implements OnInit {

  @Input()
  value: number;
  constructor() {
    super();
  }

  ngOnInit() {
  }

}
