import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/skip';
import 'rxjs/add/operator/takeUntil';
import { Injectable } from '@angular/core';
import { Effect, Actions } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import { empty } from 'rxjs/observable/empty';
import { of } from 'rxjs/observable/of';
import * as fromLayoutProfile from '../../actions/layout/layoutPorfile';
import { LayoutProfileService } from '../../services/layout/layoutProfile.services';
import { State, Status } from '../../reducers/layout/layoutProfile';
import { Profile } from '../../models/layout/profile';
import { SaveFailLayoutProfile } from '../../actions/layout/layoutPorfile';
import { LayoutProfileHelper } from '../../services/layout/layoutprofile.helper';

@Injectable()
export class LayoutProfileEffects {

  @Effect()
  loadLayoutProfiles$: Observable<Action> = this.actions$
    .ofType<fromLayoutProfile.LoadLayoutProfiles>(fromLayoutProfile.LOAD_LAYOUT_PROFILES)
    .switchMap(() => {
      return this.lps.getLayoutProfiles()
        .map(profiles => new fromLayoutProfile.InitLayoutProfiles(profiles))
    });

  @Effect()
  saveLayoutProfiles$: Observable<Action> = this.actions$
    .ofType<fromLayoutProfile.SaveLayoutProfile>(fromLayoutProfile.SAVE_LAYOUT_PROFILE)
    .map(action => action.payload)
    .switchMap((payload: Profile[]) => {
    //   console.log(payload);
      return this.lps.saveLayoutProfiles(payload).
        map(res => {
         // console.log(res);
          const body = res.json();
          return <boolean>body.data || false
        })
      .map(
        (res: boolean) => {
          if (res) {
            return new fromLayoutProfile.SaveSuccessLayoutProfile()
          } else {
            return new fromLayoutProfile.SaveFailLayoutProfile();
          }
        })
        .catch((err) => of(new fromLayoutProfile.SaveFailLayoutProfile()));
    });

  constructor(private actions$: Actions, private lps: LayoutProfileService) { }
}
