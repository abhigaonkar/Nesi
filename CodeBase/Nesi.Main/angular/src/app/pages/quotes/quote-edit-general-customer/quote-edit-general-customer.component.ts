import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from 'services/authentication/tokenService';
import { CoreService } from 'services/shared/core.service';
import { WindowRef } from 'services/shared/windowRef';
import { ActivatedRoute, Router } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';
import { DomSanitizer } from '@angular/platform-browser';
import { ConfirmationService } from 'primeng/primeng';
import { DataExtra } from 'app/models/core/dataExtra';
import { baseDirectiveCreate } from '@angular/core/src/render3/instructions';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-edit-general-customer',
  templateUrl: './quote-edit-general-customer.component.html',
  styleUrls: ['./quote-edit-general-customer.component.css']
})
export class QuoteEditGeneralCustomerComponent extends QuoteEditFormBase implements OnInit {

  killQuoteDisplay = false;
  printPaneDisplay = false;
  trustedUrl: any;
  @Input() is_ready_for_print = true;

  @Output() printing = new EventEmitter();

  @Output() address_Changed = new EventEmitter();

  @Output() triggerValidation = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    private route: ActivatedRoute,
    private router: Router,
    private sanitizer: DomSanitizer,
    private cf: ConfirmationService,
  ) {
    super(winRef, store, cs, ts);
    super.Init(CONFIG.apiURL.page.quotes.editUpdate);
  }
  createForm() {
    this.userform = this.fb.group({
      'address_id': ['', Validators.required],
      'include_title': ''
    });
  }

  initQ() {
    this.selectedCustomerId = this._q.customer_id;

    this.initFormvalue = {
      'address_id': this._q && this._q.address_id,
      'include_title': this._q && this._q.include_title,
    };

    this.submitReset();
    //   this.loadChangeEvent();
    if (this.edit_disabled && this._q.status_id !== 4) {
      this.userform.disable();
    }
  }

  openEditCustomer() {
    this.winRef.boingNesi1(CONFIG.Nesi1URL.customer.editCustomer + this.selectedCustomerId.toString()
      , 'customer_' + this.selectedCustomerId.toString());
  }

  onAdderssChanged(event: any) {
    CONFIG.LOG(event, 'on address changed quote edit general custoemr');
    this.address_Changed.emit({
      address_id: event.value
    });
    this.cs.getObject<any>(CONFIG.apiURL.page.quotes.editAddress + this.selectedCustomerId + '/' + event.value)
      .subscribe(
        (res: any) => {
          this.SetObjectValueFromObject(res, this._q);
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
      );
  }

  ngOnInit() {
  }

  killQuote() {
    this.killQuoteDisplay = false;
    setTimeout(() => {
      window.location.reload();
    }, 1000);
    window.close();
  }


  reviveQuote() {
    this.cf.confirm({
      message: 'Would you like to revive this quote?',
      accept: () => {
        // tslint:disable-next-line:max-line-length
        this.cs.postString(CONFIG.apiURL.page.quotes.updateStatus, { quote_id: this._q.quote_id, revision: this._q.revision, status_id: 1, is_revive: true })
          .subscribe(
            (res) => {
              if (this.PushResponseMessage(res)) {
                setTimeout(() => {
                  window.location.reload();
                }, 1000);
              }
            },
            (err:any)=>{
              this.PushErrorMessage(err);
            }
          );
      }
    });
  }

  print_quote(type) {
    if (!this.is_ready_for_print) {
      this.printing.emit();
      this.PushWarnMessage('You have to fill all required fields and save the quote before printing!');
      return;
    }
    const quote_id = this._q.quote_id;
    const revision = this._q.revision;
    const status_id = this._q.status_id;
    if (type === 'w' || type === 'wo') {
      this.cf.confirm({
        message: "Please confirm that the quoted price for this quote should be $" + this._q.quoted_price,
        accept: () => { 
          if ((type === 'w') && ((status_id === '1') || (status_id === '2') || (status_id === '3') || (status_id === '10') || (status_id === '12')))
          {
           // this.cf.confirm({
           //   message: 'Would you like to update the last printed date?',
           //   accept: () => {
                this.cs.postDataExtra(CONFIG.apiURL.page.quotes.updateLastPrintDate + quote_id + '/' + revision, null)
                  .subscribe(
                    (res) => {
                      this._q.last_print_date = res.extra;
                     this.print_quote_url(quote_id, revision, this._q.wo, type);
                  }
                  );
            //  },
            //  reject: () => {
             //   this.print_quote_url(quote_id, revision, this._q.wo, type);
           //   }
          //  }
          } else {
           this.print_quote_url(quote_id, revision, this._q.wo, type);
          }       
        }
      });
    }
    else
    {
      this.print_quote_url(quote_id, revision, this._q.wo, type);
    }
  }

  print_quote_url(quote_id, revision, wo, type) {
    let url = '';
    if (type !== 'service_report') {
      url = '/sections/reports/print_quote/index.aspx?quoteid=' + quote_id + '' + revision + '&type=' + type;
    } else {
      url = '/sections/reports/service_report/index.aspx?wo=' + wo;
    }
    this.setTrustedUrl(url);
  }

  print_worksheet(type) {
    const quote_id = this.q.quote_id;
    const revision = this.q.revision;
    const url = '/sections/member/quote/index.aspx?a=print_worksheet&quote_id=' + quote_id + '&revision=' + revision + '&type=' + type;
    this.setTrustedUrl(url);
  }

  setTrustedUrl(url) {
    this.trustedUrl = this.sanitizer.bypassSecurityTrustResourceUrl(decodeURIComponent(CONFIG.Nesi1URL.host() + '/'
      + url + '&is_n1=true'));
    this.printPaneDisplay = true;
  }


  workOrder_click() {
    this.cs.getObject<any>(CONFIG.apiURL.page.quotes.editAddress + this.selectedCustomerId + '/' + this.q.address_id)
    .subscribe(
      (res: any) => {
        if(res.address_is_active)
        {
          this.winRef.boingNesi1('/sections/workorder/index.aspx?woprog_id=0&business_unit_id='
          + this._q.business_unit_id + '&fromquoteid=' + this.q.quote_id + this.q.revision, 'workorder');
        }
        else {
          this.PushErrorMessage(
            "The address against this quote is inactive. To create a work order from this, you will first need to revise the quote and select the correct address.");
        }
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
    );
  }

  duplicate() {
    this.cf.confirm({
      message: 'Would you like to duplicate this quote?',
      accept: () => {
        this.submitting = true;
        this.cs.postDataExtra(CONFIG.apiURL.page.quotes.duplicate
          + this._q.quote_id + '/' + this._q.revision, {})
          .subscribe(
            (res: DataExtra) => {
              if (this.PushResponseExtraMessage(res.data,res.extra)) {
                const newId = res.extra;
                this.submitting = false;
                this.q = null;
                setTimeout(() => {
                  window.location.href = '/#/opens/65/quotes/' + newId + '/1';
                  window.location.reload(true);
                  // this.router.navigate(['/opens/65/quotes/', newId, 1]);
                }, 1000);
              }
              else{
              setTimeout(() => {
              window.location.href = '/#/opens/65/quotes/' +this._q.quote_id + '/' + this._q.revision;
              window.location.reload(true);
            }, 3000);}
            },
            (err:any)=>{

              this.PushErrorMessage(err);
            }
          );
      }
    });
  }

  openwo(link) {
    if (link) {
      this.winRef.boingNesi1(link, 'wo');
    }
  }
  refresh() {
    this.winRef.refreshPage();
  }

  update_checkValidation(type: any) {
    const promise = new Promise<boolean>((res, rej) => {
      this.triggerValidation.emit({ resolve: res, reject: rej });
    })
      .then((r) => {
        console.log("pass checking");
        if(!this.is_ready_for_print)
        {
          this.PushWarnMessage('You have to save the quote first!');
          return;
        }
        if (type === "last_print_date") {
          this.cf.confirm({
            message:
              "Please confirm that the quoted price for this quote should be $" +
              this._q.quoted_price,
            accept: () => {
              super.update_date(type);
            }
          });
        } else {
          super.update_date(type);
        }
      })
      .catch((r) => console.log("failed"));
  }
}