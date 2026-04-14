import { WorkOrderFormBase } from '../_base/workorderFormBase';
import { Component, OnInit ,Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';
import { CoreService } from 'services/shared/core.service';
import { CONFIG } from 'app/configuration';


@Component({
  selector: 'nesi-workorder-customer',
  templateUrl: './workorder-customer.component.html',
  styleUrls: ['./workorder-customer.component.css']
})

export class WorkorderCustomerComponent implements OnInit {

 
  @Input() Business_unit_id:number;
  @Input() Woprog_id:number;
  @Input() Customer_id:number;
  sub: Subscription;
  link: string = '/#/opens/10/customers/14';
  profile: any;
  Customer_ID: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private sanitizer: DomSanitizer,
    private cs: CoreService
  ) { }

  trust(url) {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
}

  ngOnInit() {
      
   // this.cs.getObject<any>('/api/Page/WorkOrder/Edit/Main/' + this.Business_unit_id + '/' + this.Woprog_id)
     // .subscribe(
       // (res) => {
        //  this.profile = res;
       // }
     // );

       this.Customer_ID=this.Business_unit_id ? this.Business_unit_id:11;
       //this.Customer_ID=this.Customer_id ? this.Customer_id:11;
      // this.Customer_ID=this.profile.customerId;
       //  alert(this.Customer_id);
       this.link='/#/opens/10/customers/'+this.Customer_id;
  }

}
