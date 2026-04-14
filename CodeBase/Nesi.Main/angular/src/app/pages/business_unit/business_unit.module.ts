import { DataTableModule, DialogModule } from 'primeng/primeng';
import { NgModule } from '@angular/core';
import { BusinessUnitListComponent } from './business_unit-list/business_unit-list.component';
import { BusinessUnitRoutingModule } from './business_unit.routing.module';
import { BusinessUnitEditComponent } from './business_unit-edit/business_unit-edit.component';
import { BusinessUnitService } from '../../services/pages/business_unit.service';

@NgModule({
  imports: [
    BusinessUnitRoutingModule,
    DataTableModule,
 ],
  declarations: [
    BusinessUnitListComponent,
    BusinessUnitEditComponent
],
  providers: [
    BusinessUnitService
   ]
})
export class BusinessUnitModule { }
