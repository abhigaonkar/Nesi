import * as fromMessage from '../../actions/layout/growlMessage';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';

export function pushResponseMessage(store: Store<fromRoot.State>, msg: string) {
  if(!msg) {
    return;
  }

  if (msg.indexOf('success') > -1) {
    store.dispatch(new fromMessage.PushSuccessMessage(msg));
  } else {
    store.dispatch(new fromMessage.PushErrorMessage(msg));
  }
}



