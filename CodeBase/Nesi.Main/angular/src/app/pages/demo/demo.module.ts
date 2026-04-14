import { NgModule } from '@angular/core';
import { BusinessUnitService } from '../../services/pages/business_unit.service';
import { DemoRoutingModule } from './demo.routing.module';
import { UsernamePasswordComponent } from './usernamePassword/usernamePassword.component';
import { FieldErrorDisplayModule } from '../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { InputTextModule, ButtonModule } from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    DemoRoutingModule,
    FieldErrorDisplayModule,
    InputTextModule,
    ButtonModule,
  ],
  declarations: [
    UsernamePasswordComponent,
  ],
  providers: [

  ]
})
export class DemoModule { }
