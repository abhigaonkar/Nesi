import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { MessagespageListComponent } from './messagespage-list/messagespage-list.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: MessagespageListComponent, canActivate: [AuthGuard], data: { title: 'Messages' } },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class MessagesPageRouterModule { }

