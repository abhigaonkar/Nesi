import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileManagerComponent } from './fileManager.component';
import {
  InputTextModule, TreeModule, DataTableModule, TreeNode, ContextMenuModule,
  DialogModule, ButtonModule
} from 'primeng/primeng';
import { FileUploadModule } from 'primeng/components/fileupload/fileupload';
import { FormsModule } from '@angular/forms';
import { PipesModule } from 'app/pipes/Pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    DataTableModule,
    TreeModule,
    ContextMenuModule,
    FileUploadModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    PipesModule,
  ],
  declarations: [
    FileManagerComponent,
  ],
  exports: [FileManagerComponent],
})
export class FileManagerModule { }
