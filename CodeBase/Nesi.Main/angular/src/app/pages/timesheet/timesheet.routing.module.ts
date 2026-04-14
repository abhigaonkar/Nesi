import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TimesheetListComponent } from './timesheet-list/timesheet-list.component';
import { AuthGuard } from '../../services/authentication/authGuard';
import { TimesheetBankComponent } from './timesheetBank/timesheetBank.component';
import { TimesheetVacationComponent } from './timesheetVacation/timesheetVacation.component';
import { TimesheetSafetyManualComponent } from './timesheetSafetyManual/timesheetSafetyManual.component';
import { TimesheetExpenseMainComponent } from './timesheetExpenseMain/timesheetExpenseMain.component';
import { TimesheetPelComponent } from './timesheet-pel/timesheet-pel.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: TimesheetListComponent, canActivate: [AuthGuard], data: { title: 'Timesheet' } },
      { path: 'expense', component: TimesheetExpenseMainComponent, canActivate: [AuthGuard], data: { title: 'Expense Reimbursement' } },
      { path: 'bank', component: TimesheetBankComponent, canActivate: [AuthGuard], data: { title: 'Banked Pay' } },
      { path: 'vacation', component: TimesheetVacationComponent, canActivate: [AuthGuard], data: { title: 'Vacation Requests' } },
      {
        path: 'pel', component: TimesheetPelComponent, canActivate: [AuthGuard],
        data: { title: 'Personal Emergency Leave Requests' }
      },
      { path: 'safetymanual', component: TimesheetSafetyManualComponent, canActivate: [AuthGuard], data: { title: 'Safety Manuals' } },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class TimesheetRoutingModule { }

