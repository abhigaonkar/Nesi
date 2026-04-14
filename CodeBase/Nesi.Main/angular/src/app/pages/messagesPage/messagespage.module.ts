
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FieldErrorDisplayModule } from '../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { LoadingSpinnerModule } from '../../components/shared/loading-spinner/loading-spinner.module';
import { PipesModule } from '../../pipes/Pipes.module';
import { MessagespageListComponent } from './messagespage-list/messagespage-list.component';
import { MessagesPageRouterModule } from './messagespage.routing.module';
import {
    ButtonModule, DialogModule, DataTableModule, TabViewModule,
    InputTextareaModule, MultiSelectModule, DropdownModule, CheckboxModule, InputTextModule, FileUploadModule, EditorModule
} from 'primeng/primeng';
import { MessagespageInboxComponent } from './messagespage-inbox/messagespage-inbox.component';
import { MessagespageNewComponent } from './messagespage-new/messagespage-new.component';
import { MessagespageReplyComponent } from './messagespage-reply/messagespage-reply.component';
import { MessagespageReadmessageComponent } from './messagespage-readmessage/messagespage-readmessage.component';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ButtonModule,
        ReactiveFormsModule,
        LoadingSpinnerModule,
        PipesModule,
        MessagesPageRouterModule,
        DialogModule,
        DataTableModule,
        TabViewModule,
        InputTextareaModule,
        MultiSelectModule,
        DropdownModule,
        CheckboxModule,
        InputTextModule,
        DialogModule,
        FieldErrorDisplayModule,
        BreadcurmbBarModule,
        FileUploadModule,
        EditorModule,
    ],
    declarations: [
        MessagespageListComponent,
        MessagespageInboxComponent,
        MessagespageNewComponent,
        MessagespageReplyComponent,
        MessagespageReadmessageComponent
    ],
    providers: [

    ],
    exports: [
        MessagespageListComponent,
    ]
})
export class MessagesPageModule { }
