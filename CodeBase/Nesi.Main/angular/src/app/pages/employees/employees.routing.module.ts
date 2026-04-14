import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { EmployeesListComponent } from './EmployeesList/EmployeesList.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: EmployeesListComponent, canActivate: [AuthGuard], data: { title: 'Employees' } },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class EmployeesPageRouterModule { }

