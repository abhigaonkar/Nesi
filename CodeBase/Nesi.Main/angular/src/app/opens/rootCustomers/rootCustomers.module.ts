import { NgModule } from '@angular/core';
import { PickListModule } from '../../components/shared/picklist/picklist.module';
import { RootCustomersDetailComponent } from './RootCustomersDetail/RootCustomersDetail.component';
import { RootCustomersRouterModule } from 'app/opens/rootCustomers/rootCustomers.routing.module';
import { CustomersModule } from 'app/pages/customers/customers.module';


@NgModule({
  imports: [
    RootCustomersRouterModule,
    CustomersModule,
   ],
  declarations: [
    RootCustomersDetailComponent
],
  providers: [
  ]
})
export class RootCustomersModule { }
