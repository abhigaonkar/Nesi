import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NesiSideBarComponent } from './nesi-sideBar.component';
import { SidebarModule } from 'primeng/primeng';
import { LoadingSpinnerModule } from '../loading-spinner/loading-spinner.module';

@NgModule({
  imports: [
    CommonModule,
    SidebarModule,
    LoadingSpinnerModule,
  ],
  declarations: [
    NesiSideBarComponent
  ],
  exports: [
    NesiSideBarComponent
  ],
})
export class NesiSideBarModule { }
