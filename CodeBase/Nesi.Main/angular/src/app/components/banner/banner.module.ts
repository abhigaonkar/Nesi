import {AutoCompleteModule, OverlayPanelModule,  DialogModule,  ButtonModule, BlockUIModule} from 'primeng/primeng';
import { NgModule, forwardRef } from '@angular/core';
import {
  DropdownModule, InputTextModule,
  InputTextareaModule, CheckboxModule
} from 'primeng/primeng';
import { NG_VALUE_ACCESSOR, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FieldErrorDisplayModule } from '../shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { ReactiveDropDownModule } from '../shared/BindDropDownReactive/reactiveDropDown.module';
import { ResponsiveModule, ResponsiveConfig } from 'ng2-responsive';
import { FileUploadModule } from 'primeng/components/fileupload/fileupload';
import { ResponsiveDefinition } from '../../configuration';
import { HoverIconComponent } from './hoverIcon/hoverIcon.component';
import { IconOnlineUserComponent } from './icon.onlineUser/icon.onlineUser.component';
import { IconGetSupportComponent } from './icon.getSupport/icon.getSupport.component';
import { IconTodoComponent } from './icon.todo/icon.todo.component';
import { IconMessagesComponent } from './icon.messages/icon.messages.component';
import { BannerComponent } from './banner.component';
import { AcitveUserTableComponent } from './table.acitveUser/acitveUserTable.component';
import { TicketsWaitingForMeTableComponent } from './table.ticketsWaitingForMe/ticketsWaitingForMeTable.component';
import { TodoTableTableComponent } from './table.todo/todoTable.component';
import { TableMessagesComponent } from './table.messages/table.messages.component';
import { IconFvrComponent } from './icon.fvr/icon.fvr.component';
import { SwtichUserComponent } from './swtichUser/swtichUser.component';
import { QuickExtensionTableComponent } from './table.quickExtension/quickExtensionTable.component';
import { WhoDoIAskTableComponent } from './table.whoDoIAsk/whoDoIAskTable.component';
import { UpcomingVacationsTableComponent } from './table.upcomingVacations/upcomingVacations.component';
import { RouterModule } from '@angular/router';
//import { Change_password_andyComponent } from './change_password_andy/change_password_andy.component';
import { TimerComponent } from './Timer/Timer.component';
import { GlobalSearchComponent } from './globalSearch/globalSearch.component';
import { TimerFormatPipe } from 'app/pipes/timerFormat';
import { PipesModule } from 'app/pipes/Pipes.module';
import { FvrTableModule } from 'app/components/shared/fvrTable/fvrTable.module';
import { ChangePasswordBannerComponent } from './change-password-banner/change-password-banner.component';


@NgModule({
  imports: [
    RouterModule,
    CommonModule,
    FieldErrorDisplayModule,
    ResponsiveModule,
    OverlayPanelModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    PipesModule,
    ReactiveFormsModule,
    AutoCompleteModule,
    DropdownModule,
    PipesModule,
    FvrTableModule,
    CheckboxModule,
    BlockUIModule,
  ],
  declarations: [
    HoverIconComponent,
    IconOnlineUserComponent,
    IconGetSupportComponent,
    IconTodoComponent,
    IconMessagesComponent,
    BannerComponent,
    AcitveUserTableComponent,
    TicketsWaitingForMeTableComponent,
    TodoTableTableComponent,
    TableMessagesComponent,
    IconFvrComponent,
    SwtichUserComponent,
    QuickExtensionTableComponent,
    WhoDoIAskTableComponent,
    UpcomingVacationsTableComponent,
   
    TimerComponent,
    GlobalSearchComponent,
    ChangePasswordBannerComponent
],
  exports: [
    BannerComponent,
    TimerComponent,
  ],
})
export class BannerModule { }
