import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { WorkOrderSummaryListComponent } from 'app/pages/workOrder/workOrder-summary-list/workOrder-summary-list.component';
import { PurchaseOrderSummaryListComponent } from 'app/pages/purchaseOrder/purchaseOrder-summary-list/purchaseOrder-summary-list.component';
import { PurchaseOrderCreditCardPurchasesComponent } from 'pages/purchaseOrder/purchase-order-credit-card-purchases/purchase-order-credit-card-purchases.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: PurchaseOrderSummaryListComponent, canActivate: [AuthGuard], data: { title: 'Purchase Order' } },
      { path: 'creditCard', component: PurchaseOrderCreditCardPurchasesComponent, canActivate: [AuthGuard], data: { title: 'Purchase Order' } },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class PurchaseOrderPageRouterModule { }

