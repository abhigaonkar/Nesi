import { Component,OnInit,ViewChildren, QueryList, ElementRef,Input,Output, EventEmitter,ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormControl,Validators,NgModel } from '@angular/forms';
import { SignatureFieldComponent } from '../signature-field/signature-field.component';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from './../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { WindowRef } from 'app/services/shared/windowRef';
import { TokenService } from '../../../services/authentication/tokenService';
import { CONFIG } from '../../../configuration';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { TimesheetWorkOrderWo } from '../../../models/pages/timesheet/timesheetWorkOrderWo';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { PostResult } from '../../../models/core/postResult';
import { NewCustomerContactComponent } from '../../../components/shared/new-customer-contact/new-customer-contact.component';
import { e } from '@angular/core/src/render3';
import { DomSanitizer, SafeResourceUrl, SafeUrl,SafeHtml} from '@angular/platform-browser';



@Component({
  selector: 'nesi-timesheet-signoff',
  templateUrl: './timesheet-signoff.component.html',
  styleUrls: ['./timesheet-signoff.component.css']
})
export class TimesheetSignoffComponent extends FormMessageBase implements OnInit {

  @Input()
  WoId: number;
  @Input()
  SelectedDate: Date;

  @Input()
  visible:boolean;
  addNewContactDisplay=false;
 // PDFDisplay=false;
  public innerHtml:SafeHtml;
  CutPO:number;
  public title = 'timesheet-signoff';
  pdfSrc="";
  IsPrintName=false;
  IsEmail=false;
  FileName="";
  public pdfsource:string;
  public customerContacts: LabelValueInt[];
  public multiContacts: LabelValueInt[];
  public selectedAddressId:number;
  public selectedCustomerId:number;
  public customer_contact:number;
  public send_to_contact_ids:string[];
  IsMultiContactsVisible=false;
 
  @Output()
  close = new EventEmitter();
  //public signoffform:FormGroup;
  // for convenience as we don't have a QueryList.index
  //public secondSig: SignatureFieldComponent;

  @ViewChildren(SignatureFieldComponent) public sigs: QueryList<SignatureFieldComponent>;
  @ViewChildren('sigContainer1') public sigContainer1: QueryList<ElementRef>;
  @ViewChild(NewCustomerContactComponent)
  public newcustomer: NewCustomerContactComponent;

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    private tss: TimesheetService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,  
    private sanitizer: DomSanitizer
  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.timesheet.Signature);
  }

  createForm() {
    this.userform = this.fb.group({
      'cutpo':['', [Validators.required, Validators.minLength(3)]],
      'signatureField': ['', Validators.required],
      'Ischeckeddiffsignee':false,
      'printname':'',
      'customer_contact': '',
      'send_to_contact_ids':''
    });

    this.initFormvalue = {
      'cutpo': '',
      'signatureField': '',
      'Ischeckeddiffsignee':false,
      'printname':'',
      'customer_contact':'',
      'send_to_contact_ids':''
    };
    
  }

  onResize(event) {
    this.resize(this.sigContainer1.first, this.sigs.first);
    this.clear(); 
}

public setInnerHtml(pdfurl: string) {
  this.innerHtml = this.sanitizer.bypassSecurityTrustHtml(
      "<object data='" + pdfurl + "' type='application/pdf' class='embed-responsive-item' width='100%'>" +
      "Object " + pdfurl + " failed" +
      "</object>");

}

  public ngAfterViewInit() {
     this.beResponsive();
  }

  // set the dimensions of the signature pad canvas
  public beResponsive() {
    console.log('Resizing signature pad canvas to suit container size');
    this.resize(this.sigContainer1.first, this.sigs.first);
  }

  public resize(container: ElementRef, sig: SignatureFieldComponent) { 
    sig.signaturePad.set('canvasWidth', container.nativeElement.offsetWidth);
    sig.signaturePad.set('canvasHeight', container.nativeElement.offsetHeight);  
  }

  public setOptions() {
   // this.sigs.first.signaturePad.set('penColor', 'rgb(255, 0, 0)');
   // this.secondSig.signaturePad.set('penColor', 'rgb(255, 255, 0)');
    //this.secondSig.signaturePad.set('minWidth', '0.5');
    //this.secondSig.signaturePad.clear(); // clearing is needed to set the background colour
  }

  public submit() {
    if(this.userform.valid)
    {
      console.log('CAPTURED SIGS:');
      console.log(this.sigs.first.signature);
    }
  }

  public clear() {
    this.sigs.first.clear();
  }
    public ReviewPDf()
   {
    console.log(this.SelectedDate,"this.SelectedDate");
    this.tss.GetPdfPreview(this.WoId.toString(),true,this.SelectedDate).subscribe(
      (res:any)=>{
            var data="data:application/pdf;base64,"+res.data
            this.pdfsource=data;
             var w = window.open('', '_blank',"toolbar=yes,location=yes,menubar=yes");
             w.document.title =this.WoId.toString()+"_Daily Sign_off.pdf";     
             w.document.body.innerHTML = "<object data='" + this.pdfsource + "' type='application/pdf' class='embed-responsive-item' width='100%' height='100%'>" + "Object " + this.pdfsource + " failed" + "</object>";   
      }
    )
   }

  ngOnInit() {
    this.postUrl=CONFIG.apiURL.page.timesheet.Signature+this.WoId.toString()+'/'+true+'/'+this.SelectedDate.toDateString();  // it's daily signoff
    this.tss.WorkOrderWoById(this.WoId).subscribe(
      (res: TimesheetWorkOrderWo) => {
        if (res) {
          this.userform.get('cutpo').setValue(res.cutPO);
          this.selectedCustomerId=res.customer_id;
          this.selectedAddressId=res.address_id;
          this.loadContacts();
        }
        });
      
  }
  
  loadContacts() {
    this.customerContacts = null;
    this.userform.get('customer_contact').setValue(null);
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.timesheet.newCustomerAddressContact + this.selectedCustomerId)
      .subscribe(
        (res) => {
          this.customerContacts = res;
          this.multiContacts=res.filter(i=>i.value>1);
          CONFIG.LOG(this.multiContacts, 'this.multiContacts');
          this.IsMultiContactsVisible=false;
          this.multiContacts.forEach(eachobj=>{ 
            this.userform.addControl(
              eachobj.value.toString(), new FormControl([false, Validators.required])
            );
          });
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
      );
  }

  public addContact() {    
   
    if (this.selectedAddressId>0)
    {
       this.newcustomer.addressId = this.selectedAddressId;
       this.newcustomer.reInit(this.selectedCustomerId)
       this.addNewContactDisplay = true;
    }
    else{
       super.PushErrorMessage("You must select a valid Address.");
    }
  }

  public editContact()
  {
    if(this.customer_contact>1)
    {
      this.newcustomer.addressId = this.selectedAddressId;
      this.newcustomer.reLoad(this.customer_contact);
      this.addNewContactDisplay = true;
    }
  }
  
  
  public toggledifferentsignee()
  {
    this.IsPrintName=!this.IsPrintName;
    if(!this.IsPrintName)
    {
      this.userform.get('printname').setValue('');
    }
  }
  public closeClick(event: any)
  {
    CONFIG.LOG(this.visible, 'this.visible');
    this.visible=false;
    this.close.emit(event);
  }

  public contactCancel() {
    this.addNewContactDisplay = false;
  }

  public contactSaved(event: PostResult) {
    super.LOG(event, 'event on contact saved');
    this.addNewContactDisplay = false;
    this.cs.getList(CONFIG.apiURL.page.timesheet.newCustomerAddressContact + this.selectedCustomerId)
      .subscribe(
        (res: LabelValueInt[]) => {
          this.customerContacts = res;
          this.loadContacts();
          if (this.userform.get('customer_contact')) {
              this.userform.get('customer_contact').setValue(event.result);
              if(!this.send_to_contact_ids.find(event.result))
              {
                   this.send_to_contact_ids.push(event.result);
                   this.userform.get('send_to_contact_ids').setValue(this.send_to_contact_ids.toString()); 
              }      
          }
        }
      ),
      (err:any)=>{
        super.PushErrorMessage(err);
      };
  }

  onChecked(event:any)
  {
     this.userform.get('send_to_contact_ids').setValue(this.send_to_contact_ids.toString()); 
  }

  onchanged(event: any) {
    CONFIG.LOG(event.value, 'event.value');
    if(event.value==0 || event.value==1)
    {
         this.send_to_contact_ids=[];
         this.IsPrintName=true;
         this.IsEmail=false;
         this.userform.get('Ischeckeddiffsignee').setValue(false);
        if(event.value==1)
        {
          this.IsMultiContactsVisible=true;     
        }
        else
        {
          this.IsMultiContactsVisible=false;
        }
    }
    else
    {
      this.IsPrintName=true;
      this.IsEmail=false;
      this.IsMultiContactsVisible=false;   
      this.send_to_contact_ids=event.value; 
      this.userform.get('send_to_contact_ids').setValue(this.send_to_contact_ids.toString()); 
    }
    this.customer_contact=event.value;
    this.onChange.emit(event);
  }

  //
  // Below for sending email
  //
  public sendingFlag = false;
  public sendingMsg = "";
  email() {
    //
    // Basic check.
    //
    let woid = this.WoId;
    let date = this.SelectedDate;
    if(woid == 0 || date == null) {
      return;
    }

    //
    // check email recipients
    //
    if(this.send_to_contact_ids == null || this.send_to_contact_ids.length == 0) {
      // No contacts are selected.
      super.PushInfoMessage("Please select at least one contact.");
      return;
    }

    let contacts = "";

    if(this.IsMultiContactsVisible) {
      contacts = this.send_to_contact_ids.join();
    } else {
      contacts = this.send_to_contact_ids.toString()
    }

    // Setup info
    this.sendingFlag = true;
    this.sendingMsg = "Email is being sent, please wait.";

    this.tss.SendingEmails(woid, date, contacts).subscribe(
      data => {
        this.email_sending_completed(data.data);
      },
      error => {
        this.sendingFlag = false;
        this.sendingMsg = "";
        super.PushWarnMessage("We have a trouble to sending emails, try later.");
        console.log(error);
      }
    );

  }

  email_sending_completed (data) {
    if(data.okay) {
      super.PushSuccessMessage(data.msg);
    } else {
      super.PushErrorMessage(data.msg);
    }

    this.sendingFlag = true;
    this.sendingMsg = data.msg;

    setTimeout(() => {
      this.sendingFlag = false;
      this.sendingMsg = "";
    }, 1000);
  }

}


