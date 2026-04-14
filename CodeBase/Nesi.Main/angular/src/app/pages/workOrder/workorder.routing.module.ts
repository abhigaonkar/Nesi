import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { WorkOrderSummaryListComponent } from 'app/pages/workOrder/workOrder-summary-list/workOrder-summary-list.component';
import { WorkOrderBucketsComponent } from './workOrder-buckets/workOrder-buckets.component';
import { WorkorderSplitScannedFileComponent } from './workorder-split-scanned-file/workorder-split-scanned-file.component';
import { WorkorderMainComponent } from './workorder-main/workorder-main.component';
import { WorkorderMainWithFileComponent } from './workorder-main-with-file/workorder-main-with-file.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: WorkOrderSummaryListComponent, canActivate: [AuthGuard], data: { title: 'Work Order' } },
      {
        path: 'buckets/:business_unit_id', component: WorkOrderBucketsComponent,
        canActivate: [AuthGuard], data: { title: 'Work Order Buckets' }
      },
      {
        path: 'scanned_file/:business_unit_id/:file_name', component: WorkorderSplitScannedFileComponent,
        canActivate: [AuthGuard], data: { title: 'Work Order Scanned File' }
      },
      {
        path: 'rename/:business_unit_id/:woprog_id', component: WorkorderSplitScannedFileComponent,
        canActivate: [AuthGuard], data: { title: 'Work Order File Rename' }
      }, {
        path: 'edit/:business_unit_id/:str_wo_id', component: WorkorderMainComponent,
        canActivate: [AuthGuard], data: { title: 'Work Order' }
      },
      {
        path: 'edit_withfile/:business_unit_id/:str_wo_id', component: WorkorderMainWithFileComponent,
        canActivate: [AuthGuard], data: { title: 'Work Order' }
      },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class WorkOrderPageRouterModule { }

