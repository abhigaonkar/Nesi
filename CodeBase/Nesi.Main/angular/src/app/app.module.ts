import {
  BlockUIModule, CheckboxModule,
  ButtonModule, InputTextareaModule, DropdownModule
} from 'primeng/primeng';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SignInComponent } from './pages/signin/signin.component';
import { HttpService } from './core/http.service';
import { CoreModule } from './core/core.modules';
import { SignOutComponent } from './pages/signout/signout.component';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { MenuService } from './services/layout/menuService';
import { BannerService } from './services/layout/banner.service';
import { reducer } from './reducers';
import { AuthGuard } from './services/authentication/authGuard';
import { TokenService } from './services/authentication/tokenService';
import { NesiMenuEffects } from './effects/layout/nesiMenu';
import { EffectsModule } from '@ngrx/effects';
import { LayoutProfileEffects } from './effects/layout/layoutProfile';
import { LayoutProfileService } from './services/layout/layoutProfile.services';
import { CurrentuserEffects } from './effects/layout/currentUser';
import { FvrComponent } from './pages/fvr/fvr.component';
import { WindowRef } from './services/shared/windowRef';
import { CONFIG, ResponsiveDefinition } from './configuration';
import { ResponsiveModule, ResponsiveConfig } from 'ng2-responsive'
import { Ng2DeviceDetectorModule } from 'ng2-device-detector';
import { DeviceService } from './services/authentication/device';
import { LayoutProfileHelper } from './services/layout/layoutprofile.helper';
import { ProfileModule } from './pages/profiles/profile.module';
import { SignalRService } from './services/authentication/signalR.service';
import { AuthorizeService } from './services/authentication/authorize.Service';
import { PasswordService } from './services/authentication/password.service';
import { ValidateService } from './services/shared/validateService';
import { CoreService } from './services/shared/core.service';
import { TimesheetService } from './services/pages/timesheet.service';
import { CookieService } from 'ng2-cookies';
import { TimerFormatPipe } from './pipes/timerFormat';
import { AppTimerComponent } from './components/appTimer/appTimer.component';
import { environment } from '../environments/environment'; // Angular CLI environment

import {
  InputTextModule,
  SplitButtonModule,
  PanelMenuModule,
  PanelModule,
  MegaMenuModule,
  DialogModule,
  GrowlModule,
  OverlayPanelModule,
  InputSwitchModule,
  ConfirmDialogModule,
  ConfirmationService,
} from 'primeng/primeng';
import { PipesModule } from 'app/pipes/Pipes.module';
import { FvrTableModule } from 'app/components/shared/fvrTable/fvrTable.module';
import { RouterExtService } from './services/layout/routerExtService';
import { PurchaseOrderCreditCardPurchasesComponent } from './pages/purchaseOrder/purchase-order-credit-card-purchases/purchase-order-credit-card-purchases.component';


@NgModule({
  declarations: [
    AppComponent,
    SignInComponent,
    SignOutComponent,
    FvrComponent,
    AppTimerComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    PipesModule,
    PanelModule,
    BlockUIModule,
    GrowlModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpModule,
    CoreModule,
    InputTextModule,
    BrowserAnimationsModule,
    DialogModule,
    OverlayPanelModule,
    FvrTableModule,
    Ng2DeviceDetectorModule.forRoot(),
    StoreModule.forRoot(reducer),
    // StoreDevtoolsModule.instrumentOnlyWithExtension(),
    !environment.production ? StoreDevtoolsModule.instrument({ maxAge: 50 }) : [],
    EffectsModule.forRoot([
      NesiMenuEffects,
      LayoutProfileEffects,
      CurrentuserEffects,
    ]),
    ConfirmDialogModule,
    ButtonModule,
  ],
  providers: [
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    {
      provide: ResponsiveConfig,
      useFactory: ResponsiveDefinition
    },
    TokenService,
    AuthGuard,
    SignalRService,
    MenuService,
    BannerService,
    AuthorizeService,
    LayoutProfileService,
    WindowRef,
    DeviceService,
    LayoutProfileHelper,
    ConfirmationService,
    PasswordService,
    ValidateService,
    CoreService,
    CookieService,
    // RouterExtService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
