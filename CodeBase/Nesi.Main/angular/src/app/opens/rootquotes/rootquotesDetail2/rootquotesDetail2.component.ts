import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-rootquotesDetail2',
  templateUrl: './rootquotesDetail2.component.html',
  styleUrls: ['./rootquotesDetail2.component.css']
})
export class RootquotesDetail2Component implements OnInit {

  sub: Subscription;
  quoteId: String;
  revision: String;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cs: CoreService,
  ) { }

  ngOnInit() {

    this.sub = this.route.params.subscribe(params => {
      const p = String(params['quote_id']); // (+) converts string 'id' to a number
      if (p.length >= 7) {
        this.quoteId = p.substr(0, 6);
        this.revision = p.substr(6);
        this.router.navigate(['/opens/65/quotes/' + this.quoteId + '/' + this.revision]);
      } else {
        this.cs.getString(CONFIG.apiURL.page.quotes.getActiveRevision + p)
          .subscribe((res) => {
            this.quoteId = p;
            this.revision = res;
            this.router.navigate(['/opens/65/quotes/' + this.quoteId + '/' + this.revision]);
          })
      }
    });

  }

}
