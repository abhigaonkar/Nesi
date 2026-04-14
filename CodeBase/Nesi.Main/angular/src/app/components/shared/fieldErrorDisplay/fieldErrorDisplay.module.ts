import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FieldErrorDisplayComponent } from './fieldErrorDisplay.component';
import { AsyncFieldErrorDisplayComponent } from '../asyncFieldErrorDisplay/asyncFieldErrorDisplay.component';

@NgModule({
  imports: [
    CommonModule,
  ],
  declarations: [
    FieldErrorDisplayComponent,
    AsyncFieldErrorDisplayComponent,
  ],
  exports: [FieldErrorDisplayComponent,
    AsyncFieldErrorDisplayComponent,
  ],
})
export class FieldErrorDisplayModule { }
