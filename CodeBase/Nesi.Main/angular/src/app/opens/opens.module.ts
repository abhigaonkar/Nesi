import { NgModule } from '@angular/core';
import { OpensRoutingModule } from './open-routing.module';
import { OpensComponent } from './opens.component';
import { ConfirmationService, ConfirmDialogModule } from 'primeng/primeng';
import { BannerModule } from '../components/banner/banner.module';


@NgModule({
  imports: [
    OpensRoutingModule,
    ConfirmDialogModule,
  ],
  declarations: [
    OpensComponent,
  ],
  providers: [
  ]
})
export class OpensModule { }
