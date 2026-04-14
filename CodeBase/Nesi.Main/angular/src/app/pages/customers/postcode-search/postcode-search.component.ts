import { Component, OnInit, Input } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import * as DATE from '../../../services/helper/datetime';
import { DataTableBase } from 'app/components/shared/Bases/DataTableBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-postcode-search',
  templateUrl: './postcode-search.component.html',
  styleUrls: ['./postcode-search.component.css']
})
export class PostcodeSearchComponent extends DataTableBase implements OnInit {

  constructor(
    protected store: Store<fromRoot.State>,
    protected cs: CoreService,
  ) {
    super(store, cs);

  }

  init() {
    this.sortField = 'postal_definer';
  }

  get url(): string {
    return CONFIG.apiURL.page.customers.postcodeSearch;
  }

  get postData(): any {
    return {};
  }

}
