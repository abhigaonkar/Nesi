import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { HomeTodoComponent } from 'app/pages/homepage/home-todo/home-todo.component';
import { HomeAutobingoComponent } from 'app/pages/homepage/home-autobingo/home-autobingo.component';
import { HomeInvoiceServiceComponent } from 'app/pages/homepage/home-invoice-service/home-invoice-service.component';
import { HomeSlowPageComponent } from 'app/pages/homepage/home-slow-page/home-slow-page.component';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { TokenService } from 'app/services/authentication/tokenService';
import { WindowRef } from 'app/services/shared/windowRef';

@Component({
  selector: 'nesi-home-list',
  templateUrl: './home-list.component.html',
  styleUrls: ['./home-list.component.css']
})
export class HomeListComponent implements OnInit{

  @ViewChild(HomeTodoComponent)
  todo: HomeTodoComponent;

  @ViewChild(HomeAutobingoComponent)
  autoBingo: HomeAutobingoComponent;

  @ViewChild(HomeInvoiceServiceComponent)
  invoiceService: HomeInvoiceServiceComponent;

  @ViewChild(HomeSlowPageComponent)
  slowPage: HomeSlowPageComponent;


 // refreshTime = 120000;
  show_branchpo = false;
  is_developer = 0;
  show_wo = false;
  show_quote = false;
  show_faq = false;
  buid = 0;
  show_pane = false;

  interv: any;

  constructor(
    public cs: CoreService,
    public ts: TokenService,
    private winRef: WindowRef,
  ) { }

  get isContact(): boolean {
    return this.ts.isContact;
  }

 // ngOnDestroy(): void {
 //   if (this.interv) {
 //     clearInterval(this.interv);
//    }
//  }

  ngOnInit() {
    if (!this.ts.isContact) {
      this.getEmpolyee();
    } else {
      this.getContact();
    }
  }

  openFaq() {
    this.winRef.boingNesi1('/sections/training/faq.aspx', 'faq');
  }

  openQuote() {
    this.winRef.boingNesi1('/sections/reports/CustView_Quotes/index.aspx?new_quote=true', 'quote');
  }

  openWo() {
    this.winRef.boingNesi1('/sections/workorder/index.aspx?woprog_id=0&business_unit_id=' + this.buid, 'wo');
  }

  getContact() {
    this.cs.getObject<any>(CONFIG.apiURL.page.homepage.contact)
      .subscribe(
      (res) => {
        this.show_faq = res.show_faq;
        this.show_wo = res.show_wo;
        this.show_quote = res.show_quote;

        this.buid = res.buid;
      }
      );
  }
  refreshPage(){
    if (this.is_developer === 1) 
    {             
          this.autoBingo.Refresh();
          this.invoiceService.Refresh();    
         // this.slowPage.Refresh();
    }
  }
  getEmpolyee() {
    this.cs.getObject<any>(CONFIG.apiURL.page.homepage.profile)
      .subscribe(
      (res) => {
        this.show_pane = res.show_pane;

        this.is_developer = res.is_developer ? 1 : this.show_pane ? 2 : 0;
        this.show_branchpo = res.show_branchpo;
        this.refreshPage;
      }
      );
  }
}
