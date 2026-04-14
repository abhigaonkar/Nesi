import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FvrTableComponent } from './fvrTable.component';

@NgModule({
  imports: [
    CommonModule,
  ],
  declarations: [
    FvrTableComponent
],
  exports: [FvrTableComponent],
})
export class FvrTableModule { }
