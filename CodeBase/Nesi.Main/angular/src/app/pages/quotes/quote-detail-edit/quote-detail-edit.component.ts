import { Component, OnInit, OnDestroy, ChangeDetectorRef, AfterViewInit, ViewChild, HostListener, Input } from '@angular/core';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { Params, ActivatedRoute, Router } from '@angular/router';
import { MessageBase } from '../../../core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { Title } from '@angular/platform-browser';
import { WindowRef } from '../../../services/shared/windowRef';
import { ConfirmationService } from 'primeng/primeng';
import { TokenService } from '../../../services/authentication/tokenService';
import { QuoteFormBase } from '../_base/quoteFormBase';
import { QuoteDetailEditStrategyComponent } from '../quote-detail-edit-strategy/quote-detail-edit-strategy.component';
import { PicklistMainComponent } from 'app/components/shared/picklist/picklist-main/picklist-main.component';
import { QuoteEditScoreworkComponent } from 'app/pages/quotes/quote-edit-scorework/quote-edit-scorework.component';
import { QuoteEditGeneralComponent } from 'app/pages/quotes/quote-edit-general/quote-edit-general.component';
import { QuoteEditNoteadddressComponent } from 'app/pages/quotes/quote-edit-noteadddress/quote-edit-noteadddress.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-detail-edit',
  templateUrl: './quote-detail-edit.component.html',
  styleUrls: ['./quote-detail-edit.component.css']
})
export class QuoteDetailEditComponent extends QuoteFormBase implements OnInit, OnDestroy, AfterViewInit {

  @Input() quoteId: number;
  @Input() revision: number;
  active_revision = false;
  private sub: any;
  private emptyAddress = {
    active_revision: '',
    address_addr1: '',
    address_addr2: '',
    address_addr3: '',
    address_addr4: '',
    address_city: '',
    address_country: '',
    address_faxNumber: '',
    address_id: 0,
    address_is_active: false,
    address_options: [],
    address_phoneNumber: '',
    address_postal: '',
    address_prov: ''
  }
  exist_quotes: any[];
  openQuotesDisplay = false;
  newVersionDisplay = false;

  quoted_price: number;

  @ViewChild(PicklistMainComponent)
  picklistmain: PicklistMainComponent;
  @ViewChild(QuoteEditScoreworkComponent)
  scopeWork: QuoteEditScoreworkComponent;
  @ViewChild(QuoteEditNoteadddressComponent)
  noteAdders: QuoteEditNoteadddressComponent;
  @ViewChild(QuoteEditGeneralComponent)
  gernal: QuoteEditGeneralComponent

  showStage1 = false;
  errorTitle: string;
  errorMessage: string;
  hasError = false;

  // @ViewChild(QuoteDetailEditStrategyComponent)
  // strategy: QuoteDetailEditStrategyComponent;

  public _q: any;
  set q(value: any) {
    this._q = value;
  }

  get q() {
    return this._q;
  }

  get revisions(): any[] {
    if (this._q) {
      return this._q.revisions;
    } else {
      return [];
    }
  }

  public get edit_disabled(): boolean {
    if (this._q) {
      return this._q.edit_disabled;
    } else {
      return false;
    }
  }


  activeTableView = 0;
  searchDisplay = false;
  loading = true;
  msgs = [];

  constructor(
    protected winRef: WindowRef,
    public cs: CoreService,
    private route: ActivatedRoute,
    protected store: Store<fromRoot.State>,
    private titleService: Title,
    private router: Router,
    private cf: ConfirmationService,
    protected ts: TokenService,
  ) {
    super(winRef, store, cs);
  }

  createForm() {

  }

  ngAfterViewInit() {
  }
  ngOnInit() {
    if (!this.quoteId) {
      this.sub = this.route.params.subscribe(params => {
        this.quoteId = +params['quote_id']; // (+) converts string 'id' to a number
        this.revision = +params['revision'];
        this.errorTitle = 'Quote loading error';

        this.extractQuoteIdAndRevision();

        // In a real app: dispatch action to load the details here.
        if (!this.quoteId || this.quoteId === NaN || this.quoteId === 0) {
          // tslint:disable-next-line:max-line-length
          this.hasError = true;
          // tslint:disable-next-line:max-line-length
          this.errorMessage = ('Quote # not supplied - If you are cutting a quote please remember each step you just did and cut a ticket with each step detailed.');
          this.msgs.push({ severity: 'error', summary: this.errorTitle, detail: this.errorMessage });
        }
        if (!this.revision || this.revision === NaN || this.revision === 0) {
          // tslint:disable-next-line:max-line-length
          this.hasError = true;
          // tslint:disable-next-line:max-line-length
          this.errorMessage = ('Revision # not supplied - If you are cutting a quote please remember each step you just did and cut a ticket with each step detailed.');
          this.msgs.push({ severity: 'error', summary: this.errorTitle, detail: this.errorMessage });
        }
        if (this.quoteId < 100000) {
          // tslint:disable-next-line:max-line-length
          this.hasError = true;
          // tslint:disable-next-line:max-line-length
          this.errorMessage = ('This quote was cut using the ACCESS based quote program, not the Spark Ops quote program... all Spark Ops quotes have a quote number greater than 100000.');
          this.msgs.push({ severity: 'error', summary: this.errorTitle, detail: this.errorMessage });
        }
        if (!this.hasError) {
          this.loadQuote();
        } else {
          this.loading = false;
        }
      });
    } else {
      this.loadQuote();
    }
  }

  loadQuote() {
    this.loading = true;
    this.initializeQuote();
    this.titleService.setTitle('#' + this.quoteId + ' - Quote');
  }

  initializeQuote(selectQuote = false) {
    CONFIG.LOG(this.loading, 'loading value intializequote');
    this.cs.getObject<any>(CONFIG.apiURL.page.quotes.editInit + this.quoteId + '/' + this.revision)
      .subscribe(
        (res: any) => {
          if (res.hasError) {
            this.hasError = true;
            this.errorTitle = res.errorTitle;
            this.errorMessage = res.errorMessage;
            this.msgs.push({ severity: 'error', summary: this.errorTitle, detail: this.errorMessage });
          } else {
            this.hasError = false;
            this.showStage1 = (res.status_id === '10');
            this.q = res;
            CONFIG.LOG(res.quote_id, 'quote is loaded intializequote');
            this.active_revision = res.active_revision === '1';
            if (selectQuote) {
              this.exist_quotes = res.exist_quotes;
              CONFIG.LOG(this.exist_quotes, 'exist quotes after customer changed');
              if (this.exist_quotes && this.exist_quotes.length > 0) {
                this.openQuotesDisplay = true;
              }
            }
          }
          this.loading = false;
        },
        (err:any)=>{
          this.PushErrorMessage(err);
          this.loading = false;
        }
      )
  }

  OnCustomerChanged(event: any) {
    this.quoteId = event.quote_id;
    this.revision = event.revision;
    this._q.address_options = event.addresses;
    this._q.customer_id = event.customer_id;
    this._q.bdm = event.bdm;    
    this._q.customer_name = event.customer_name;
    this.exist_quotes = event.customerQuotes;
    this.SetObjectValueFromObject(this.emptyAddress, this._q);
    if (this._q.customer_id) {
      this.cs.getObject<any>(CONFIG.apiURL.page.quotes.editAddress + event.customer_id + '/' + event.address_id)
        .subscribe(
          (res: any) => {
            this._q.address_options = res.address_options;
          },
          (err:any)=>{
            this.PushErrorMessage(err);
          }
        );
    }
    if (this.exist_quotes && this.exist_quotes.length > 0) {
      this.openQuotesDisplay = true;
    }
  }

  update_quoted_price(event) {
    this.q.quoted_price = event;
    this.quoted_price = event;
  }

  public openRowQuoteSelf(event: any) {
    const id = event.data.quoteid;
    const rev = event.data.rev;
    super.LOG(event, 'open row quote self');
    // tslint:disable-next-line:max-line-length
    if (window.confirm(`If the current quote ( ${this.q.quote_id + ' v' + this.q.revision} ) is yours and you click this link it will clear it and redirect you to quote #  ${id + ' v' + rev} \n
       If the current quote isn't yours it will just redirect you to the selected quote.\n\nPlease confirm this is what you want done.`)) {
      this.q = null;
      this.scopeWork.rowsLoaded = false;
      this.router.navigate(['/opens/65/quotes/' + id + '/' + rev]);
      this.openQuotesDisplay = false;
    }

  }

  @HostListener('window:beforeunload', ['$event'])
  public unloadHandler($event) {
    if (this.gernal && this.gernal.dirty) {
      $event.returnValue = true;
    }
  }


  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  tabviewOnChange(event: any) {
    const currentIndex = this.activeTableView;
    this.activeTableView = event.index;
    if (currentIndex === 0 && this.gernal.dirty && event.index !== 0) {
      this.cf.confirm({
        message: 'Changes you made may not be saved, do you really want to leave this tab?',
        accept: () => {

        },
        reject: () => {
          this.activeTableView = 0;
        }
      });
    }
  }


  changeRevision(event) {
    super.LOG(event.value, 'change revision value');
    this.q = null;
    this.quoted_price = null;
    this.revision = event.value;
    window.location.href = '/#/opens/65/quotes/' + this.quoteId + '/' + event.value;
  }

  do_revsion(event) {
    super.LOG(event, 'do revision in quote detail edit');
    this.q = null;
    this.quoted_price = null;
    this.quoteId = event.quote_id;
    this.revision = event.revision;
    if (Number(event.revision)) 
    {
      window.location.href = '/#/opens/65/quotes/' + event.quote_id + '/' + event.revision;
    }
    else 
    {
      // the extradata is a string of errors occured
      this.PushResponseExtraMessage("Failed", event.revision);
      setTimeout(() => {
        window.location.reload();
      }, 3000)
    }
  }

  activeRevision(obj, event) {
    if (!this.active_revision) { return; }
    this.cs.postString(CONFIG.apiURL.page.quotes.setActiveRevision + this._q.quote_id + '/' + this._q.revision, null)
      .subscribe(
        (res) => {
          this.PushShortResponseMessage(res);
          setTimeout(() => {
            location.reload();
          }, 500)
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
      );
  }


  priceChange(event: any) {
    this.q.tm_pricing_total = event.totalTM;
    this.q.worksheet_total = event.totalQuote;
    this.q.total_cost = event.totalCost;
    this.q.margin_TM = this.q.tm_pricing_total !== 0 ? (this.q.tm_pricing_total - this.q.total_cost) / this.q.tm_pricing_total : 0;
    this.q.margin_total = this.q.worksheet_total !== 0 ? (this.q.worksheet_total - this.q.total_cost) / this.q.worksheet_total : 0;
  }

  quoteEditChanged(event: any) {
    this._q.quoted_price = event.post.quoted_price;
    this.quoted_price = event.post.quoted_price;
    super.LOG(this._q.quoted_price, 'quote_price after save quote');
  }


  openSection(event: any) {
    super.LOG(this.activeTableView, 'activeTableView in open section detail edit');
    this.activeTableView = 3;
    this.picklistmain.openSection(event);
  }


  rowUpdated(event: any) {
    super.LOG(event, 'row updated in open section detail edit');
    this.picklistmain.updateSection(event);
  }

  priceUpdated() {
    this.cs.getList<any>(CONFIG.apiURL.page.quotes.detailDiv + this.quoteId + '/' + this.revision + '/1')
      .subscribe(
        (res) => {
          this._q.divs1 = res;
          this.scopeWork.q.divs1 = res;
          this.scopeWork.rowsLoaded = false;
          this.scopeWork.initQ();
          super.LOG(this.scopeWork.q.div1, 'div1 value in price updated in worksheet');
        },
        (err:any)=>{
          this.PushErrorMessage(err);
          this.scopeWork.rowsLoaded = false;
        }
      );

    this.cs.getList<any>(CONFIG.apiURL.page.quotes.detailDiv + this.quoteId + '/' + this.revision + '/2')
      .subscribe(
        (res) => {
          this._q.divs2 = res;
          this.noteAdders.q.divs2 = res;
          this.noteAdders.rowsLoaded = false;
          this.noteAdders.initQ();
          super.LOG(this.noteAdders.q.div2, 'div2 value in price updated in worksheet');
        },
        (err:any)=>{
          this.PushErrorMessage(err);
          this.noteAdders.rowsLoaded = false;
        }
      );
  }

  quoteKilled() {
    this.showStage1 = false;
    window.close();
  }

  quoteApproved() {
    this.showStage1 = false;
    window.setTimeout(() => {
      window.location.reload();
    }, 1000);
  }

  reOrderScopeWork(event: any) {
    this.picklistmain.updateSection(event);
  }

  sectionsChanged(event) {

  }

  private extractQuoteIdAndRevision() {
    //
    // The method to handle Quote and Revision is suggested by Jordan.
    //

    /*
    min(quote_id) 	max(quote_id)
          100000	  147202

    revision
          1
          2
    */

    if (this.revision !== 0) {
      return;
    }

    if (this.quoteId >= 1000000) {
      // The first 6 digitals are quoteId,
      // The left part is the revision.

      const quote = this.quoteId.toString();
      const quoteId = quote.substr(0, 6);
      const revision = quote.substr(6, quote.length - 6);

      // tslint:disable-next-line:radix
      this.quoteId = Number(quoteId);
      this.revision = Number(revision);
    }

  }
}
