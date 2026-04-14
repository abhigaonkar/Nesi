import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule, ButtonModule, DropdownModule, DialogModule } from 'primeng/primeng';
import { FieldErrorDisplayModule } from '../fieldErrorDisplay/fieldErrorDisplay.module';
import { ReactiveDropDownModule } from '../BindDropDownReactive/reactiveDropDown.module';
import { NewCustomerContactComponent } from './new-customer-contact.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    DropdownModule,
    FieldErrorDisplayModule,
    ReactiveDropDownModule
  ],
  declarations: [
    NewCustomerContactComponent
],
  exports: [NewCustomerContactComponent],
})
export class NewCustomerContactModule { }
