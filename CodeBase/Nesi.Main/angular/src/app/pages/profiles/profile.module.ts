
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LayoutProfileListComponent } from './layoutProfileList/layoutProfileList.component';
import { LayoutProfileService } from '../../services/layout/layoutProfile.services';
import { ProfileRoutingModule } from './profile.routing.module';
import { ToggleButtonModule, DialogModule, InputTextModule } from 'primeng/primeng';
import {
  InputSwitchModule,
  ButtonModule,
  FieldsetModule,
  TabViewModule,
  RadioButtonModule,
  SliderModule,
  TooltipModule
} from 'primeng/primeng';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';

@NgModule({
  imports: [
    CommonModule,
    ButtonModule,
    ProfileRoutingModule,
    TabViewModule,
    RadioButtonModule,
    InputTextModule,
    FieldsetModule,
    FormsModule,
    SliderModule,
    InputSwitchModule,
    ToggleButtonModule,
    TooltipModule,
    BreadcurmbBarModule,
    DialogModule
  ],
  declarations: [
    LayoutProfileListComponent
,
],
  providers: [
  ]
})
export class ProfileModule { }
