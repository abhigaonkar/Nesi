import { WindowRef } from '../../../services/shared/windowRef';
import { CONFIG } from '../../../configuration';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
export abstract class QuoteFormBase extends FormMessageBase {


  constructor(
    protected winRef: WindowRef,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
  ) {
    super(store, cs);
  }

  public openQuote(quoteId: string, rev: string) {
    // CONFIG.LOG(line, 'open quote window');
    const url = CONFIG.opens.quoteEdit.replace('@id', quoteId).replace('@rev', rev);
    this.winRef.boing(url, 'quote_' + quoteId, 1600, 960);
  }

  public openRowQuote(event: any) {
    super.LOG(event, 'open row quote');
    let id = event.data.quote_id;
    let rev = event.data.revision;
    if (event.data.quoteid) { id = event.data.quoteid; }
    if (event.data.rev) { rev = event.data.rev; }
    this.openQuote(id, rev);
  }
}
