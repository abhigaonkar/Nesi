
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToggleButtonModule, ButtonModule, InputTextareaModule, InputTextModule, PanelModule } from 'primeng/primeng';
import { ReleaseComponent } from './release/release.component';
import { TabViewModule } from 'primeng/components/tabview/tabview';
import { RadioButtonModule } from 'primeng/components/radiobutton/radiobutton';
import { TooltipModule } from 'primeng/components/tooltip/tooltip';
import { ReleaseSystemRoutingModule } from './release-system.routing.module';
import { ReactiveDropDownModule } from '../../components/shared/BindDropDownReactive/reactiveDropDown.module';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import { PasswordEnhanceComponent } from './passwordEnhance/passwordEnhance.component';
import { ResetPasswordComponent } from './resetpassword/resetpassword.component';
import {PasswordModule} from 'primeng/password';

@NgModule({
  imports: [
    CommonModule,
    ButtonModule,
    TabViewModule,
    InputTextareaModule,
    InputTextModule,
    RadioButtonModule,
    FormsModule,
    ReactiveFormsModule,
    ToggleButtonModule,
    TooltipModule,
    ReleaseSystemRoutingModule,
    ReactiveDropDownModule,
    PanelModule,
    BreadcurmbBarModule,
    PasswordModule
  ],
  declarations: [
    ReleaseComponent,
    PasswordEnhanceComponent,
    ResetPasswordComponent,
],
  providers: [
  ],
  exports: [
    PasswordEnhanceComponent,
    ResetPasswordComponent
  ]
})
export class ReleaseSystemModule { }
