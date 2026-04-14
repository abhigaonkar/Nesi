import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BreadcurmbComponent } from 'app/components/shared/breadcrumb/breadcurmb/breadcurmb.component';
import { BreadcrumbModule } from 'primeng/primeng';

@NgModule({
  imports: [
    CommonModule,
    BreadcrumbModule,
  ],
  declarations: [
    BreadcurmbComponent,
  ],
  exports: [
    BreadcurmbComponent
  ],
})
export class BreadcurmbBarModule { }
