

import { NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import { IntegrationErrorComponent } from 'pages/admintools/integration-error/integration-error.component';
import { AdmintoolsRoutingModule } from 'pages/admintools/admintools.routing.module';
import { DxDataGridModule ,DxSelectBoxModule,DxAccordionModule,DxTemplateModule} from 'devextreme-angular';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import {SelectDurationComponent} from '../admintools/select-duration/select-duration.component';
@NgModule({
    imports: [
        AdmintoolsRoutingModule,
        DxDataGridModule,
        DxSelectBoxModule,
        BreadcurmbBarModule,     
        CommonModule ,
        DxAccordionModule,
        DxTemplateModule
    ],
    declarations: [
      
        SelectDurationComponent,   IntegrationErrorComponent     
    ],
    providers: [

    ],
    exports:[SelectDurationComponent]
})
export class AdminToolsModule{}


