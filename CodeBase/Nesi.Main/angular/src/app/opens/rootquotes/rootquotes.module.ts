import {RootQuotesRouterModule} from './rootquotes.routing.module';
import { NgModule } from '@angular/core';
import { RootquotesDetailComponent } from './rootquotesDetail/rootquotesDetail.component';
import { QuotesModule } from '../../pages/quotes/quotes.module';
import { RootquotesWorkSheetComponent } from './rootquotesWorkSheet/rootquotesWorkSheet.component';
import { PickListModule } from '../../components/shared/picklist/picklist.module';
import { RootquotesDetail2Component } from './rootquotesDetail2/rootquotesDetail2.component';


@NgModule({
  imports: [
    RootQuotesRouterModule,
    QuotesModule,
    PickListModule,
   ],
  declarations: [
    RootquotesDetailComponent,
    RootquotesWorkSheetComponent,
    RootquotesDetail2Component
],
  providers: [
  ]
})
export class RootQuotesModule { }
