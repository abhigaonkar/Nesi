import { Component, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { QuoteFormBase } from '../_base/quoteFormBase';
import { WindowRef } from '../../../services/shared/windowRef';
import { FormBuilder, Validators } from '@angular/forms';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { NewCustomerContactComponent } from '../../../components/shared/new-customer-contact/new-customer-contact.component';
import { PostResult } from '../../../models/core/postResult';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-addnew-pane',
  templateUrl: './quote-addnew-pane.component.html',
  styleUrls: ['./quote-addnew-pane.component.css']
})
export class QuoteAddnewPaneComponent extends QuoteEditFormBase implements OnInit {

  qualifierList: any[];
  privilege61 = false;
  bdmList: any[] = [];
  isLoadingBDM = false;

  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,

  ) {
    super(winRef, store, cs, ts);
    super.Init(CONFIG.apiURL.page.quotes.new);
  }

  createForm() {
    this.userform = this.fb.group({
      'customer': ['', Validators.required],
      'customer_id': ['', Validators.required],
      'customer_address': ['', Validators.required],
      'customer_contact': ['', Validators.required],
      'expected_value': ['', [Validators.required, Validators.min(0.01)]],
      'chance_winning': ['', Validators.required],
      'date_due': ['', Validators.required],
      'date_expected_start': ['', Validators.required],
      'date_invoiced': ['', Validators.required],
      'job_description': ['', Validators.required],
      'quote_businessUnit': ['', Validators.required],
      'bdm': ['', Validators.required],
      'managed_by': ['', Validators.required],
      'qualifiers': '',
      'business_unit_name': '',
      'customer_name': '',
      'customer_address_name': '',
      'customer_contact_name': '',
      'managed_by_name': '',
      'bdm_name': '', 
      'quoter_locked': '',
    });

    this.initFormvalue = {
      'customer': '',
      'customer_id': '',
      'customer_address': '',
      'customer_contact': '',
      'expected_value': '',
      'chance_winning': 30,
      'date_due': '',
      'date_expected_start': '',
      'date_invoiced': '',
      'job_description': '',
      'quote_businessUnit': this.ts.currentUser.businessUnitId,
      'managed_by': this.ts.currentUser.id,
      'bdm': '', 
      'qualifiers': null,
      'business_unit_name': '',
      'customer_name': '',
      'customer_address_name': '',
      'customer_contact_name': '',
      'managed_by_name': '',
      'bdm_name': '', 
      'quoter_locked': false,
    }
  }

  ngOnInit() {
    this.cs.getObject(CONFIG.apiURL.page.quotes.newProfile)
      .subscribe(
      (res: any) => {
        this.businessUnits = res.businessUnitList;
        this.users = res.userListInBusinessUnit;
        this.privilege61 = res.privilege61;
        this.qualifierList = res.qualifiers;
        // Note: Don't set bdmList here as it depends on customer selection
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }

customerSelected(event: any, fieldName: string) {
  // SAVE BDM value
  const savedBDM = this.userform.get('bdm').value;
  /* console.log('customerSelected called with:', event, fieldName);*/
  super.customerSelected(event, fieldName);
  
  setTimeout(() => {
    this.resetBDMField();
    this.loadBDMOptionsIfReady();
    
    // PUT BDM VALUE BACK OR CLEAR IT
    setTimeout(() => {
      // Get the current value after loading
      const currentBDMValue = this.userform.get('bdm').value;
      
      if (currentBDMValue === null || currentBDMValue === undefined) {
        // If current value is null/undefined, keep it null (no BDM options available)
        this.userform.get('bdm').setValue(null);
      } else if (savedBDM) {
        // If there are options available and we had a saved value, restore it
        this.userform.get('bdm').setValue(savedBDM);
      } else {
        // If there are options but no saved value, set to null
        this.userform.get('bdm').setValue(null);
      }
    }, 300);
    
  }, 800);
}

  BusinessUnitSelected(event: any) {
  /*  console.log('BusinessUnitSelected called with:', event);*/
    if (super.BusinessUnitSelected) {
      super.BusinessUnitSelected(event);
    }
    
    // Use setTimeout to ensure form control is updated
    setTimeout(() => {
      this.resetBDMField();
      this.loadBDMOptionsIfReady();
    }, 300);
  }

  private resetBDMField() {
    // Reset form control safely - Angular 6 compatible
    var bdmControl = this.userform.get('bdm');
    if (bdmControl) {
      bdmControl.setValue(null);
      bdmControl.markAsUntouched();
      bdmControl.updateValueAndValidity();
    }
    
    // Reset component state
    this.bdmList = [];
    this.isLoadingBDM = false;
  }

  private loadBDMOptionsIfReady() {
    var customerControl = this.userform.get('customer_id');
    var businessUnitControl = this.userform.get('quote_businessUnit');
    
    var customerId = customerControl ? customerControl.value : null;
    var businessUnitId = businessUnitControl ? businessUnitControl.value : null;
    
    if (customerId && businessUnitId) {
      /*console.log('Both values present, calling loadBDMOptions');*/
      this.loadBDMOptions(customerId, businessUnitId);
    } else {
     /* console.log('Missing values - resetting BDM field');*/
      this.resetBDMField();
    }
  }

 private loadBDMOptions(customerId: number, businessUnitId: number) {
  /*console.log('Loading BDM options for customer:', customerId, 'business unit:', businessUnitId);*/
  
  this.isLoadingBDM = true;
  this.bdmList = [];

  // Reset BDM field first
  var bdmControl = this.userform.get('bdm');
  if (bdmControl) {
    bdmControl.setValue(null);
  }

  // Build the API URL
  var apiUrl = CONFIG.apiURL.page.quotes.newCustomerBDM + customerId + '/' + businessUnitId;
  /*console.log('BDM API URL:', apiUrl);*/
  
  this.cs.getObject(apiUrl).subscribe(
    (res: any) => {
      /*console.log('BDM API Response:', res);*/
      this.isLoadingBDM = false;

      // Handle the response structure
      if (res && res.bdmList && Array.isArray(res.bdmList)) {
        /*console.log('Raw first item from API:', res.bdmList[0]);*/
        
        // Map the BDM list with proper data types
        this.bdmList = res.bdmList.map((item: any) => {
          /*console.log('Mapping item:', item);*/
          return {
            label: item.label,
            value: item.value // Keep original type from API
          };
        });

        //console.log('Mapped BDM List:', this.bdmList);
        //console.log('Selected BDM from API:', res.selectedBdm, 'Type:', typeof res.selectedBdm);

        // Set selected value with proper Angular 6 approach
        if (res.selectedBdm && res.selectedBdm > 0) {
          // Find matching BDM in the list
          var selectedValue = res.selectedBdm;
          var matchingBdm = this.bdmList.find(function(bdm) { 
            return bdm.value == selectedValue; // Use == for loose comparison
          });
          
          //console.log('Looking for BDM with value:', selectedValue);
          //console.log('Matching BDM found:', matchingBdm);
          
          if (matchingBdm) {
            // Use setTimeout to ensure dropdown options are rendered
            setTimeout(() => {
              var bdmControl = this.userform.get('bdm');
              if (bdmControl) {
                // Set the value that matches exactly what's in the options
                bdmControl.setValue(matchingBdm.value);
                bdmControl.markAsDirty();
                bdmControl.markAsTouched();
                bdmControl.updateValueAndValidity();
                
                //console.log('Set selected BDM:', matchingBdm.value);
                //console.log('Form control value after setting:', bdmControl.value);
                
                // Additional debug - check if dropdown recognizes the value
                setTimeout(() => {
                  //console.log('Final form control value:', bdmControl.value);
                  //console.log('BDM list at time of selection:', this.bdmList);
                }, 100);
              }
            }, 300); // Increased delay for Angular 6
          } else {
            console.warn('Selected BDM value not found in options list');
            console.warn('Available values:', this.bdmList.map(function(bdm) { return bdm.value; }));
          }
        } else {
         /* console.log('No selected BDM (selectedBdm is null or 0)');*/
        }
      } else {
        /*console.log('No BDM data in response');*/
        this.bdmList = [];
      }
    },
    (err: any) => {
      console.error('BDM API Error:', err);
      this.isLoadingBDM = false;
      this.bdmList = [];
      
      var bdmControl = this.userform.get('bdm');
      if (bdmControl) {
        bdmControl.setValue(null);
      }
      
      // Only show error for non-404 responses
      if (err.status !== 404) {
        this.PushErrorMessage('Failed to load BDM information');
      }
    }
  );
}

  getBDMPlaceholder(): string {
    var customerControl = this.userform.get('customer_id');
    var businessUnitControl = this.userform.get('quote_businessUnit');
    
    var hasCustomer = customerControl ? customerControl.value : null;
    var hasBusinessUnit = businessUnitControl ? businessUnitControl.value : null;
    
    if (!hasCustomer || !hasBusinessUnit) {
      return 'Select Customer & Business Unit First';
    }
    if (this.isLoadingBDM) {
      return 'Loading BDM...';
    }
    if (!this.bdmList || this.bdmList.length === 0) {
      return 'No BDM Available';
    }
    return 'Select BDM...';
  }

  // Helper methods for BDM field template conditions - Angular 6 compatible
  getBDMDisabledState(): boolean {
    var customerControl = this.userform.get('customer_id');
    var businessUnitControl = this.userform.get('quote_businessUnit');
    
    var hasCustomer = customerControl ? customerControl.value : null;
    var hasBusinessUnit = businessUnitControl ? businessUnitControl.value : null;
    
    return !hasCustomer || !hasBusinessUnit || this.isLoadingBDM;
  }

  shouldShowPrerequisiteError(): boolean {
    var customerControl = this.userform.get('customer_id');
    var businessUnitControl = this.userform.get('quote_businessUnit');
    
    var hasCustomer = customerControl ? customerControl.value : null;
    var hasBusinessUnit = businessUnitControl ? businessUnitControl.value : null;
    
    return !hasCustomer || !hasBusinessUnit;
  }

  shouldShowNoBDMError(): boolean {
    var customerControl = this.userform.get('customer_id');
    var businessUnitControl = this.userform.get('quote_businessUnit');
    
    var hasCustomer = customerControl ? customerControl.value : null;
    var hasBusinessUnit = businessUnitControl ? businessUnitControl.value : null;
    
    return hasCustomer && hasBusinessUnit && this.bdmList && this.bdmList.length === 0 && !this.isLoadingBDM;
  }

  shouldShowValidationError(): boolean {
    var customerControl = this.userform.get('customer_id');
    var businessUnitControl = this.userform.get('quote_businessUnit');
    
    var hasCustomer = customerControl ? customerControl.value : null;
    var hasBusinessUnit = businessUnitControl ? businessUnitControl.value : null;
    
    return hasCustomer && hasBusinessUnit && this.bdmList && this.bdmList.length > 0;
  }

  formValidateBefore() {
    this.userform.get('managed_by').enable();
    var selectedCustomer = this.userform.get('customer').value;
    if(selectedCustomer)
    {
      this.userform.get('customer_name').setValue(selectedCustomer.label);
    }
    this.userform.get('business_unit_name').setValue(
      this.GetLabelByValueFromLabelValueInt(this.businessUnits, this.userform.get('quote_businessUnit').value)
    );
    this.userform.get('customer_address_name').setValue(
      this.GetLabelByValueFromLabelValueInt(this.customerAddresses, this.userform.get('customer_address').value)
    );
    this.userform.get('customer_contact_name').setValue(
      this.GetLabelByValueFromLabelValueInt(this.customerContacts, this.userform.get('customer_contact').value)
    );
    this.userform.get('managed_by_name').setValue(
      this.GetLabelByValueFromLabelValueInt(this.users, this.userform.get('managed_by').value)
    );
    this.userform.get('bdm_name').setValue(
      this.GetLabelByValueFromLabelValueInt(this.bdmList, this.userform.get('bdm').value)
    );
  }

  lockedChange() {
    CONFIG.LOG(this.userform.get('quoter_locked').value, ' quoter locked value');
    if (this.userform.get('quoter_locked').value) {
      this.userform.get('managed_by').disable();
    } else {
      this.userform.get('managed_by').enable();
    }
  }
  
  public handle_customer_request(){
    /*console.log("handle");*/
    var url = CONFIG.Nesi1URL.requestCustOrVendor.replace('@type', '1').replace('@customer_id',this.userform.get('customer_id').value);
    this.winRef.boingNesi1(url ,'customerRequest-'+Math.random(),"500,700");
  }
}
