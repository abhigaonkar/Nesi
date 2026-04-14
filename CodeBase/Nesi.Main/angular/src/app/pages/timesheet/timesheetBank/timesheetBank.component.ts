import { Component, OnInit } from '@angular/core';
import { TimeSheetProfile } from '../../../models/pages/timesheet/timesheetProfile';
import { TimesheetPageBase } from '../interface/timesheetPageBase';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { BankSummary } from '../../../models/pages/timesheet/bankSummary';
import { CONFIG } from '../../../configuration';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ConfirmationService } from 'primeng/primeng';
import { TokenService } from '../../../services/authentication/tokenService';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { CoreService } from '../../../services/shared/core.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetBank',
  templateUrl: './timesheetBank.component.html',
  styleUrls: ['./timesheetBank.component.css']
})
export class TimesheetBankComponent extends TimesheetPageBase implements OnInit {

  public summary: BankSummary;
  public withdrawForm: FormGroup;
  public depositForm: FormGroup;
  public selectedPayPeriod = 0;

  constructor(
    protected tss: TimesheetService,
    protected store: Store<fromRoot.State>,
    private fb: FormBuilder,
    private cfs: ConfirmationService,
    private ts: TokenService,
    private cs: CoreService
  ) {
    super(tss, store);
  }

  ngOnInit() {
    super.Init();
    this.loadSummary();
    this.createDepositForm();
    this.createWithDrawForm();
  }

  createDepositForm() {
    this.depositForm = this.fb.group({
      'data': ['', Validators.required],
    });
  }
  createWithDrawForm() {
    this.withdrawForm = this.fb.group({
      'data': ['', Validators.required],
    });
  }

  loadSummary() {
    this.tss.getObject<BankSummary>(CONFIG.apiURL.page.timesheet.bankpay.summary)
      .subscribe(
        (res: BankSummary) => {
          this.summary = res;
          this.loadLedger();
        },
        (err:any)=>
        {
          super.PushErrorMessage(err);
        }
      );
  }

  loadLedger() {
    this.tss.getObject<any>(CONFIG.apiURL.page.timesheet.bankpay.ledger + '/' + (this.selectedPayPeriod || 0).toString())
      .subscribe(
        (res2) => {
          this.summary.ledger = res2.ledger;
        },
        (err:any)=>
        {
          super.PushErrorMessage(err);
        }
      );
  }

  afterProfileLoad(): void {

  }


  disabledLedgerButton(item: any) {
    if (item.note) { return true; }
    if (item.added_by !== this.ts.currentUser.id) { return true; }
    if (item.type === 'R' || item.type === 'E' || item.type === 'P') { return true; }
    if (this.selectedPayPeriod) { return true; }
    return false;
  }

  actionLedgerButton(item: any) {

    this.cfs.confirm({
      message: `Are you sure you want to retract this transaction?`,
      accept: () => {
        this.tss.deleteString(CONFIG.apiURL.page.timesheet.bankpay.retractHours + item['id'])
          .subscribe(
            (res: string) => {
              super.PushResponseMessage(res);
              this.loadSummary();
            },
            (err:any)=>
            {
              super.PushErrorMessage(err);
            }
          );
      }
    });
  }

  getTypeName(type: string): string {
    switch (type) {
      case 'W':
        return 'Withdrawal';
      case 'D':
        return 'Deposit';
      case 'R':
        return 'Retracted';
      case 'E':
        return 'Deducted';
      case 'P':
        return 'Paid Out';

      default:
        return '';
    }
  }


  onHistoryChanged(event: any) {
    this.loadLedger();
  }

  onWithDrawSubmit(value: any) {
    super.LOG(value, 'withdraw submit');
    if (!this.checkHours(value.data, this.summary.withdrawableHours)) {
      return;
    }
    this.cfs.confirm({
      message: `Are you sure you want to withdraw ${value.data} hours?`,
      accept: () => {
        this.tss.postString(CONFIG.apiURL.page.timesheet.bankpay.withdrawHours, value)
          .subscribe(
            (res: string) => {
              super.PushResponseMessage(res);
              this.loadSummary();
              this.withdrawForm.reset();
            },
            (err:any)=>
            {
              super.PushErrorMessage(err);
            }
          );
      }
    });

  }
  onDepositSubmit(value: any) {
    super.LOG(value, 'deposit submit');
    if (!this.checkHours(value.data, this.summary.availableHours)) {
      return;
    }
    this.cfs.confirm({
      message: `Are you sure you want to bank ${value.data} hours?`,
      accept: () => {
        this.tss.postString(CONFIG.apiURL.page.timesheet.bankpay.bankHours, value)
          .subscribe(
            (res: string) => {
              super.PushResponseMessage(res);
              this.loadSummary();
              this.depositForm.reset();
            },
            (err:any)=>
            {
              super.PushErrorMessage(err);
            }
          );
      }
    });
  }

  checkHours(value: number, compare: number): boolean {
    if (!value) {
      super.PushWarnMessage('Only numbers may be requested.');
      return false;
    }
    if (value > compare) {
      super.PushWarnMessage('The number of hours requested is greater than the number of hours available.');
      return false;
    }
    if (value <= 0) {
      super.PushWarnMessage('You can only request numbers greater than zero.');
      return false;
    }
    if (this.cs.checkQuarterNumber(value)) {
      super.PushWarnMessage('Hours must be a whole number, or in quarter hour increments.');
      return false;
    }
    return true;
  }
}
