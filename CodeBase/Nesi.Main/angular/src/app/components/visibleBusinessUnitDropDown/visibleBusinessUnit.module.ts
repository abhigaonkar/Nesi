import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  DropdownModule
} from 'primeng/primeng';
import { FileUploadModule } from 'primeng/components/fileupload/fileupload';
import { FormsModule } from '@angular/forms';
import { PipesModule } from 'app/pipes/Pipes.module';
import { VisibleBusinessUnitDropDownComponent } from 'app/components/visibleBusinessUnitDropDown/visibleBusinessUnitDropDown.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    DropdownModule,
  ],
  declarations: [
    VisibleBusinessUnitDropDownComponent,
],
  exports: [VisibleBusinessUnitDropDownComponent],
})
export class VisibleBusinessUnitDropDownModule { }
