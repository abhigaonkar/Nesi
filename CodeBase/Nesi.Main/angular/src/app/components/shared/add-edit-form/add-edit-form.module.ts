import { FileManagerComponent } from './../fileManager/fileManager.component';
import { NgModule } from '@angular/core';
import { AutoCompleteModule, CalendarModule, DropdownModule} from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
} from '@angular/forms';
import { AddEditFormComponent } from './add-edit-form.component';
import { FileManagerModule } from './../fileManager/fileManager.module';

@NgModule({
  imports: [
    ReactiveFormsModule,
    CommonModule,
    DropdownModule,
    CalendarModule,
    FileManagerModule,
    AutoCompleteModule
  ],
  declarations: [AddEditFormComponent],
  exports: [AddEditFormComponent]
})
export class AddEditFormModule {

}
