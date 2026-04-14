import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { HomeComponent } from '../pages/home/home.component';
import { SignInComponent } from '../pages/signin/signin.component';
import { Nesi1Component } from '../pages/Nesi1/Nesi1.component';
import { SignOutComponent } from '../pages/signout/signout.component';
import { AuthGuard } from '../services/authentication/authGuard';
import { FvrComponent } from '../pages/fvr/fvr.component';
import { OpensComponent } from '../opens/opens.component';
import { NesiRComponent } from '../pages/Nesi1/NesiR.component';
import { NesiR2Component } from '../pages/Nesi1/NesiR2.component.';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: '', component: HomeComponent, canActivate: [AuthGuard],
        children: [
          { path: '', redirectTo: '1/0'},
          { path: '1', redirectTo: '1/0'},
          { path: '1/1/homepage/default', loadChildren: '../pages/homepage/homepage.module#HomePageModule'},
          { path: '1/1/profile/default', loadChildren: '../pages/profiles/profile.module#ProfileModule' },
          { path: '1/:id', component: NesiRComponent, canActivate: [AuthGuard] },
          { path: '1/:id/default', component: Nesi1Component, canActivate: [AuthGuard] },
          { path: '1/:id/n2', component: NesiR2Component, canActivate: [AuthGuard] },
          { path: '0/:url', component: Nesi1Component},
          { path: '2/business_unit', loadChildren: '../pages/business_unit/business_unit.module#BusinessUnitModule' },
          { path: '10/customers', loadChildren: '../pages/customers/customers.module#CustomersModule' },
          { path: '11/vendors', loadChildren: '../pages/vendors/vendors.module#VendorsModule' },
          { path: '12/workorder', loadChildren: '../pages/workOrder/workorder.module#WorkOrderPageModule' },
          { path: '92/purchaseorder', loadChildren: '../pages/purchaseOrder/purchaseorder.module#PurchaseOrderPageModule' },
          { path: '28/timesheet', loadChildren: '../pages/timesheet/timesheet.module#TimesheetModule' },
          { path: '29/messages', loadChildren: '../pages/messagesPage/messagespage.module#MessagesPageModule' },
          { path: '65/quotes', loadChildren: '../pages/quotes/quotes.module#QuotesModule' },
          { path: '138/applicants', loadChildren: '../pages/applicant/applicant.module#ApplicantPageModule' },
          { path: '127/employees', loadChildren: '../pages/employees/employees.module#EmployeesPageModule' },
          { path: '166/release', loadChildren: '../pages/releaseSystem/release-system.module#ReleaseSystemModule' },
          { path: '51/reports', loadChildren: '../pages/reports/reports.module#ReportsModule' },
          { path: '260/integerrors', loadChildren: '../pages/admintools/admintools.module#AdminToolsModule'  },
 { path: '267/customerrequestadmins', loadChildren: '../pages/customerrequestadmin/customerrequestadmin.module#CustomerRequestAdminPageModule' },
        //  { path: '246/integErrors', loadChildren: ''  },
        ]
      },
    ]),
  ],
  exports: [RouterModule]
})

export class HomeRoutingModule { }
