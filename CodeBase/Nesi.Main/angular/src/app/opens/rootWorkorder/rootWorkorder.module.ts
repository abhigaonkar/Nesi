import { NgModule } from '@angular/core';
import { PickListModule } from '../../components/shared/picklist/picklist.module';
import { VendorsModule } from '../../pages/vendors/vendors.module';
import { WorkOrderPageModule } from '../../pages/workOrder/workorder.module';
import { RootWorkorderWocommentsComponent } from './rootWorkorder-wocomments/rootWorkorder-wocomments.component';
import { RootWorkorderRouterModule } from './rootWorkorder.routing.module';


@NgModule({
  imports: [
    RootWorkorderRouterModule,
    WorkOrderPageModule,
  ],
  declarations: [
    RootWorkorderWocommentsComponent
],
  providers: [
  ]
})
export class RootWorkorderModule { }
