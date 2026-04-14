import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { AuthGuard } from '../services/authentication/authGuard';
import { OpensComponent } from '../opens/opens.component';

@NgModule({
  imports: [
    RouterModule.forChild([{
      path: '', component: OpensComponent,
      children: [
        { path: '65/quotes', loadChildren: './rootquotes/rootquotes.module#RootQuotesModule' },
        { path: '10/customers', loadChildren: './rootCustomers/rootCustomers.module#RootCustomersModule' },
        { path: '11/vendors', loadChildren: './rootVendors/rootVendors.module#RootVendorsModule' },
        { path: '12/workorder', loadChildren: './rootWorkorder/rootWorkorder.module#RootWorkorderModule' },
        { path: '127/employees', loadChildren: './rootEmployees/rootEmployees.module#RootEmployeesModule' },
        { path: '138/applicants', loadChildren: './rootApplicants/rootApplicants.module#RootApplicantsModule' },
        { path: '166/password', loadChildren: './rootPasswordEnhance/rootPasswordEnhance.module#RootPasswordEnhanceModule' },
      ]
    }
    ]),
  ],
  exports: [RouterModule]
})

export class OpensRoutingModule { }
