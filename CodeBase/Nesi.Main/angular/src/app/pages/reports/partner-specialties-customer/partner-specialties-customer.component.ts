import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-partner-specialties-customer',
  templateUrl: './partner-specialties-customer.component.html',
  styleUrls: ['./partner-specialties-customer.component.css']
})
export class PartnerSpecialtiesCustomerComponent implements OnInit {
  sub: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      let type: string = params['type'];
      const id = +params['id'];
      if (type.toLowerCase() === 'vendor' ) {
        this.router.navigate(['/opens/11/vendors/' + id]);
      } else {
        this.router.navigate(['/opens/10/customers/' + id]);
      }
    });

  }
}
