
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FieldErrorDisplayModule } from '../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { LoadingSpinnerModule } from '../../components/shared/loading-spinner/loading-spinner.module';
import { HomeListComponent } from './home-list/home-list.component';
import { HomePageRouterModule } from 'app/pages/homepage/homepage.routing.module';
import { HomeTodoComponent } from './home-todo/home-todo.component';
import { DataTableModule, PanelModule, ProgressBarModule,ButtonModule} from 'primeng/primeng';
import { HomeAutobingoComponent } from './home-autobingo/home-autobingo.component';
import { HomeInvoiceServiceComponent } from './home-invoice-service/home-invoice-service.component';
import { HomeSlowPageComponent } from './home-slow-page/home-slow-page.component';
import { HomePaneComponent } from './home-pane/home-pane.component';
import { HomePageQtyBarComponent } from './home-page-qtyBar/home-page-qtyBar.component';
import { PipesModule } from 'app/pipes/Pipes.module';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import { NesiSideBarModule } from '../../components/shared/nesi-sideBar/nesi-sideBar.module';
import { HomeModule } from '../../home/home.module';
import { EmployeesPageModule } from '../employees/employees.module';
import { QuotesModule } from '../quotes/quotes.module';
import { CustomersModule } from '../customers/customers.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        LoadingSpinnerModule,
        HomePageRouterModule,
        DataTableModule,
        PanelModule,
        ProgressBarModule,
        PipesModule,
        BreadcurmbBarModule,
        NesiSideBarModule,
        HomeModule,
        EmployeesPageModule,
        QuotesModule,
        CustomersModule,
        ButtonModule
    ],
    declarations: [
        HomeListComponent,
        HomeTodoComponent,
        HomeAutobingoComponent,
        HomeInvoiceServiceComponent,
        HomeSlowPageComponent,
        HomePaneComponent,
        HomePageQtyBarComponent,
    ],
    providers: [

    ],
    exports: [
        HomeListComponent,
    ]
})
export class HomePageModule { }
