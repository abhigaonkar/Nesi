import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { QuotesListComponent } from './quotes-list/quotes-list.component';
import { QuoteAddnewComponent } from './quote-addnew/quote-addnew.component';
import { QuoteSearchComponent } from './quote-search/quote-search.component';
import { QuoteWorksheetComponent } from './quote-worksheet/quote-worksheet.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: QuotesListComponent, canActivate: [AuthGuard], data: { title: 'Quotes' } },
      { path: 'new', component: QuoteAddnewComponent, canActivate: [AuthGuard], data: { title: 'Quotes - Start New' } },
      { path: 'search', component: QuoteSearchComponent, canActivate: [AuthGuard], data: { title: 'Quotes - Search' } },

    ])
  ],
  exports: [
    RouterModule
  ]
})
export class QuotesRouterModule { }

