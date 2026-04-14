import { Component, OnInit, forwardRef, ViewChild } from '@angular/core';
import { NG_VALUE_ACCESSOR, NgModel } from '@angular/forms';
import { ValueAccessorBase } from '../../../../services/shared/value-accessor';
import { LabelValueInt } from '../../../../models/Shared/labelValueString';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-source',
  templateUrl: './picklist-source.component.html',
  styleUrls: ['./picklist-source.component.css'],
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => PicklistSourceComponent), multi: true }]
})
export class PicklistSourceComponent  extends ValueAccessorBase<LabelValueInt>  implements OnInit {

  @ViewChild(NgModel) model: NgModel;

  items: LabelValueInt[];

  constructor() { super()}

  ngOnInit() {

  }

}
