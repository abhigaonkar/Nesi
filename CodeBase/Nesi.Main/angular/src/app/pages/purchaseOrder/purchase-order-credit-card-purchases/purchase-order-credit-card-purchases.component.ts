import { Component, OnInit, ViewChild } from '@angular/core';
import { TimeSheetProfile } from '../../../models/pages/timesheet/timesheetProfile';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { Validators, FormControl, FormGroup, FormBuilder, Validator } from '@angular/forms';
import { ExpenseCurrentProfile } from '../../../models/pages/timesheet/expenseCurrentUserProfile';
import { ExpenseSelectedUserProfile } from '../../../models/pages/timesheet/expenseSelectedUserProfile';
import { ExpenseReimbursement } from '../../../models/pages/timesheet/expenseReimbursement';
import { TokenService } from '../../../services/authentication/tokenService';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { LabelValueString } from '../../../models/Shared/labelValueInt';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { TimesheetExpenseBase } from '../../timesheet/interface/timesheetExpenseBase';
import * as DATE from '../../../services/helper/datetime';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-creditCardPurchases',
  templateUrl: './purchase-order-credit-card-purchases.component.html',
  styleUrls: ['./purchase-order-credit-card-purchases.component.css']
})
export class PurchaseOrderCreditCardPurchasesComponent extends TimesheetExpenseBase implements OnInit {

  public userform: FormGroup;
  public submitted: boolean;

  public currentUserProfile: ExpenseCurrentProfile;
  public selectedUserProfile: ExpenseSelectedUserProfile;

  public expenseReimbursement: ExpenseReimbursement;
  public CalendedrMaxDate: Date;
  public CalendedrMinDate: Date;
  public sellerList: string[];
  public receiptRequried = true;
  public allow_unlinked_timesheet: boolean;
  public cards: LabelValueString[];

  get categories(): any[] {
    if (this.selectedUserProfile) {
      console.log(this.selectedUserProfile.creditCardCategories);
      return this.selectedUserProfile.creditCardCategories;

    } else {
      return [];
    }
  }


  currencies = [
    {
      label: 'USA',
      value: 0
    },
    {
      label: 'CDN',
      value: 1
    },
    {
      label: 'EUR',
      value: 2
    },
  ];

  constructor(
    protected tss: TimesheetService,
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.purchaseorder.creditCardSave);
  }

  ngOnInit() {
    this.tss.GetExpenseCurrentUserProfile()
      .subscribe(
        (res: ExpenseCurrentProfile) => {
          this.currentUserProfile = res;
          // this.CalendedrMaxDate = new Date(this.currentUserProfile.maxDate);
          // this.CalendedrMinDate = new Date(this.currentUserProfile.minDate);
          // CONFIG.LOG(this.CalendedrMinDate, 'min date');
          // this.allow_unlinked_timesheet = res.allow_unlinked_timesheet;
          this.loadSelectedUserProfile();
          this.fullPath = this.currentUserProfile.uploadFullPath + '\\' + this.ts.currentAuthData.guid;
        });
    // get the list of credit cards
    this.cs.getList<any>(CONFIG.apiURL.page.purchaseorder.creditCard).subscribe(
      (res: any) => {
        this.cards = res;
      }
    );

    this.onChange.subscribe(
      res => {
        console.dir(res);
      },

      error => {

      }
    );
  }

  onUploaded(event: any) {
    super.onUploaded(event);
    if (this.uploadedFiles && this.uploadedFiles.length > 0) {
      this.userform.get('file_name').setValue(this.uploadedFiles[0].name);
    }
    super.LOG(this.userform.get('file_name').value, 'file uploaded timesheet expense');

  }

  createForm() {
    this.userform = this.fb.group({
      'member_id': '',
      'date_purchased': '',
      'date_start': '',
      'date_end': '',
      'seller_id': ['', Validators.required],
      'wo_number': '',
      'isShop': false,
      'amount': ['', [Validators.required, Validators.min(0.01)]],
      'currency': 1,
      'master_id': [0, Validators.required],
      'credit_card_id': [0, Validators.required],
      'master_name': '',
      'receipt_number': '',
      'has_file': false,
      'file_name': '',
      'item_text': ['', Validators.required],
      'file_path': '',
      'distance': 0,
      'distance_unit': 'KM',
      'attendees': '',
    }, {
        validator: this.checkWorkOrderAndShop()
      });

    this.initFormvalue = {
      'member_id': this.ts.currentUser.id,
      'date_purchased': DATE.Today(),
      'date_start': '',
      'date_end': '',
      'seller_id': '',
      'wo_number': '',
      'isShop': false,
      'amount': '',
      'currency': 1,
      'master_id': 0,
      'credit_card_id': 0,
      'master_name': '',
      'receipt_number': '',
      'has_file': false,
      'file_name': '',
      'item_text': '',
      'file_path': '',
      'distance': 0,
      'distance_unit': 'KM',
      'attendees': '',
    }


    this.userform.get('distance').valueChanges.subscribe(
      (value: string) => {
        if (!this.receiptRequried) {
          this.userform.get('amount').setValue(Math.round(Number(value) * 35) / 100);
        }
      }
    );

  }



  loadSelectedUserProfile() {
    this.tss.GetExpenseSelectedUserProfile(this.userform.get('member_id').value)
      .subscribe((res: ExpenseSelectedUserProfile) => {
        this.selectedUserProfile = res;
        this.userform.get('master_id').setValue('');
        this.receiptRequried = true;
        this.userform.get('currency').setValue(this.selectedUserProfile.currency);
        this.userform.get('distance_unit').setValue(this.selectedUserProfile.currency === 1 ? 'KM' : 'M');

        // CONFIG.LOG(this.selectedUserProfile, 'load selected userPfrofile');
      });
  }


  filterSeller(event: any) {
    this.tss.GetExpenseSeller(event.query)
      .subscribe(
        (res: string[]) => {
          this.sellerList = res;
          console.log(this.sellerList);
        });
  }
  categoryChanged(event) {
    const selectUserId = this.userform.get('member_id').value;
    const categoryId = this.userform.get('master_id').value;
    if (categoryId === '73105') {
      this.userform.get('attendees').setValidators([Validators.required]);
    } else {
      this.userform.get('attendees').clearValidators();
    }
    this.userform.get('attendees').updateValueAndValidity();
    this.cs.getBoolean(CONFIG.apiURL.page.timesheet.expense.receiptRequired + selectUserId + '/' + categoryId)
      .subscribe(
        (res: boolean) => {
          super.LOG(res, 'categoryChanged response');
          this.receiptRequried = res;
          if (res) {
            this.userform.get('file_name').setValidators([Validators.required]);
          } else {
            this.userform.get('file_name').clearValidators();
          }
          this.userform.get('file_name').updateValueAndValidity();
        }
      );
  }

  creditCardChanged(event) {

  }


  shopChanged(event) {
    if (this.userform.get('isShop').value) {
      this.userform.get('wo_number').setValue('');
      this.userform.get('wo_number').disable();
    } else {
      this.userform.get('wo_number').enable();
    }
  }

  reloadGrid() {
    document.getElementsByTagName('iframe')[0].src = document.getElementsByTagName('iframe')[0].src;

  }

  formValidateBefore() {
    const categoryId = this.userform.get('master_id').value;
    this.userform.get('file_path').setValue(this.fullPath);
    this.userform.get('has_file').setValue(super.isFieldHasValue('file_name'));
    const pdate = this.userform.get('date_purchased').value;
    this.userform.get('date_start').setValue(pdate);
    this.userform.get('date_end').setValue(pdate);

    if (!(this.selectedUserProfile && this.selectedUserProfile.creditCardCategories)) {
      return;
    }
    const selected = super.selectedItem(this.selectedUserProfile.creditCardCategories, 'value', categoryId);
    if (selected) {
      this.userform.get('master_name').setValue(selected.label);
    }
  }

}

