import {CheckboxModule, ButtonModule,   InputTextareaModule,   DropdownModule} from 'primeng/primeng';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { HomeComponent } from '../pages/home/home.component';
import { MainMenuComponent } from '../components/menu/menu.component';
import { SignInComponent } from '../pages/signin/signin.component';
import { FooterComponent } from '../components/footer/footer.component';
import { Nesi1Component } from '../pages/Nesi1/Nesi1.component';
import { AppSubMenuComponent } from '../components/menu/appsubmenu.component';
import { HttpService } from '../core/http.service';
import { CoreModule } from '../core/core.modules';
import { SignOutComponent } from '../pages/signout/signout.component';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { ProfileModule } from '../pages/profiles/profile.module';
import { DashMessageComponent } from '../components/dashMessage/dashMessage.component';
import { FlyoutComponent } from '../components/flyout/flyout.component';


import {
  PanelMenuModule,
  PanelModule,
  MegaMenuModule,
  DialogModule,
  GrowlModule,
  OverlayPanelModule,
  ConfirmDialogModule,
  ConfirmationService,
} from 'primeng/primeng';
import { ReactiveDropDownModule } from '../components/shared/BindDropDownReactive/reactiveDropDown.module';
import { FieldErrorDisplayModule } from '../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { HomeRoutingModule } from './home-routing.module';
import { EqualValidatorDirective } from '../directives/equalvalidator.directive';
import { FlyoutModule } from '../components/flyout/flyout.module';
import { BannerModule } from '../components/banner/banner.module';
import { NesiRComponent } from '../pages/Nesi1/NesiR.component';
import { VisibleBusinessUnitDropDownModule } from 'app/components/visibleBusinessUnitDropDown/visibleBusinessUnit.module';
import { NesiDatatableModule } from 'app/components/nesi-datatable/nesi-datatable.module';
import { ReportsModule } from '../pages/reports/reports.module';
import { FvrTableModule } from 'app/components/shared/fvrTable/fvrTable.module';
import { NesiR2Component } from '../pages/Nesi1/NesiR2.component.';


@NgModule({
  declarations: [
    HomeComponent,
    MainMenuComponent,
    AppSubMenuComponent,
    FooterComponent,
    Nesi1Component,
    NesiR2Component,
    EqualValidatorDirective,
    DashMessageComponent,
    NesiRComponent,
],
  imports: [
    ReactiveDropDownModule,
    CommonModule,
    PanelMenuModule,
    PanelModule,
    GrowlModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    CoreModule,
    MegaMenuModule,
    DialogModule,
    OverlayPanelModule,
    ProfileModule,
    ConfirmDialogModule,
    DropdownModule,
    InputTextareaModule,
    ButtonModule,
    HomeRoutingModule,
    FlyoutModule,
    BannerModule,
    VisibleBusinessUnitDropDownModule,
    NesiDatatableModule,
    ReportsModule,
    FvrTableModule
  ],
  providers: [

  ],
  exports: [
    Nesi1Component,
  ]
})
export class HomeModule { }
