import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-rootquotesDetail',
  templateUrl: './rootquotesDetail.component.html',
  styleUrls: ['./rootquotesDetail.component.css']
})
export class RootquotesDetailComponent implements OnInit, OnDestroy {
  // quoteId: number;
  // revision: number;
  // private sub: any;
  constructor(
    // private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    // this.sub = this.route.params.subscribe(params => {
    //   this.quoteId = +params['quote_id']; // (+) converts string 'id' to a number
    //   this.revision = +params['revision'];
    //   // In a real app: dispatch action to load the details here.
    // });
  }

  ngOnDestroy() {
    // this.sub.unsubscribe();
  }
}
