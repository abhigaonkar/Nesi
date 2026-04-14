import { NgModule } from '@angular/core';
import { PickListModule } from '../../components/shared/picklist/picklist.module';
import { VendorsModule } from '../../pages/vendors/vendors.module';
import { RootVendorsDetailComponent } from './RootVendorsDetail/RootVendorsDetail.component';
import { RootVendorsRouterModule } from './rootVendors.routing.module';


@NgModule({
  imports: [
    VendorsModule,
    RootVendorsRouterModule,
  ],
  declarations: [
    RootVendorsDetailComponent,
  ],
  providers: [
  ]
})
export class RootVendorsModule { }
