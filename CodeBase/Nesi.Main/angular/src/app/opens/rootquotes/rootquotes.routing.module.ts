import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { RootquotesDetailComponent } from './rootquotesDetail/rootquotesDetail.component';
import { RootquotesWorkSheetComponent } from './rootquotesWorkSheet/rootquotesWorkSheet.component';
import { RootquotesDetail2Component } from './rootquotesDetail2/rootquotesDetail2.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: 'worksheet/:quote_id/:revision', component: RootquotesWorkSheetComponent,
        data: { title: 'Quotes - Work Sheet' }
      },

      {
        path: ':quote_id/:revision', component: RootquotesDetailComponent,
        canActivate: [AuthGuard], data: { title: 'Quote' }
      },
      {
        path: ':quote_id', component: RootquotesDetail2Component,
        canActivate: [AuthGuard], data: { title: 'Quote' }
      },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class RootQuotesRouterModule { }

