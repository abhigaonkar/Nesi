import {NgModule} from '@angular/core';
import {RouterModule} from '@angular/router';
import {BusinessUnitListComponent } from './business_unit-list/business_unit-list.component';
import { BusinessUnitEditComponent } from './business_unit-edit/business_unit-edit.component';
import { AuthGuard } from '../../services/authentication/authGuard';

@NgModule({
  imports: [
    RouterModule.forChild([
      {path: '', component: BusinessUnitListComponent, canActivate: [AuthGuard]},
      {path: 'edit/:id', component: BusinessUnitEditComponent, canActivate: [AuthGuard]}

    ])
  ],
  exports: [
    RouterModule
  ]
})
export class BusinessUnitRoutingModule {}

