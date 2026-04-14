import { NgModule, forwardRef } from '@angular/core';
import { DropdownModule, InputTextModule, ButtonModule,
  InputTextareaModule, CheckboxModule, DataTableModule } from 'primeng/primeng';
import { NG_VALUE_ACCESSOR, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FlyoutComponent } from './flyout.component';
import { FieldErrorDisplayModule } from '../shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { ReactiveDropDownModule } from '../shared/BindDropDownReactive/reactiveDropDown.module';
import { ResponsiveModule, ResponsiveConfig } from 'ng2-responsive';
import { FileUploadModule } from 'primeng/components/fileupload/fileupload';
import { ResponsiveDefinition } from '../../configuration';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DataTableModule,
    InputTextModule,
    ButtonModule,
    DropdownModule,
    FieldErrorDisplayModule,
    ReactiveDropDownModule,
    ResponsiveModule,
    InputTextareaModule,
    FileUploadModule,
    CheckboxModule,
  ],
  declarations: [
    FlyoutComponent
  ],
  providers: [

  ],
  exports: [FlyoutComponent],
})
export class FlyoutModule { }
