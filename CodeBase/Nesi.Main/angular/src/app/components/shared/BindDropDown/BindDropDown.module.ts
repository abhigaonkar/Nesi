import { NgModule, forwardRef } from '@angular/core';
import { DropdownModule } from 'primeng/primeng';
import { NG_VALUE_ACCESSOR, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BindDropDownComponent } from 'app/components/shared/BindDropDown/BindDropDown.component';

@NgModule({
  imports: [
    DropdownModule,
    CommonModule,
    FormsModule,
  ],
  declarations: [
    BindDropDownComponent,
  ],
  exports: [BindDropDownComponent],
})
export class BindDropDownModule { }
