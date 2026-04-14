import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { VendorListComponent } from './vendor-list/vendor-list.component';
import { VendorOpensComponent } from './vendor-opens/vendor-opens.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: '', component: VendorListComponent, canActivate: [AuthGuard], data: { title: 'Vendors - Interface' } },
            { path: 'edit/:vendor_id', component: VendorOpensComponent, canActivate: [AuthGuard], data: { title: 'Vendor' } }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class VendorsRoutingModule { }

