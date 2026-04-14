import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { CustomerListComponent } from 'app/pages/customers/customer-list/customer-list.component';
import { CustomerOpensComponent } from './customer-opens/customer-opens.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: '', component: CustomerListComponent, canActivate: [AuthGuard], data: { title: 'Customer - Interface' } },
            { path: 'edit/:customer_id', component: CustomerOpensComponent, canActivate: [AuthGuard], data: { title: 'Customer' } }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class CustomersRoutingModule { }

