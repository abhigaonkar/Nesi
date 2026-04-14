import { ReactiveDropDownComponent } from './ReactiveDropDown.component';
import { NgModule, forwardRef } from '@angular/core';
import { DropdownModule } from 'primeng/primeng';
import { NG_VALUE_ACCESSOR, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
    DropdownModule,
    CommonModule,
    FormsModule,
  ],
  declarations: [
    ReactiveDropDownComponent
  ],
  exports: [ReactiveDropDownComponent],
})
export class ReactiveDropDownModule { }
