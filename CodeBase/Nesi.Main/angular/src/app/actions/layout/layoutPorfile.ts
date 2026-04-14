import { Action } from '@ngrx/store';
import { Profile } from '../../models/layout/profile';
import { LayoutProfileHelper } from '../../services/layout/layoutprofile.helper';

export const LOAD_LAYOUT_PROFILES = '[Layout Profile] Start to Load layout profiles from webapi';
export const INIT_LAYOUT_PROFILES = '[Layout Profile] Init layout profiles from webapi';
export const SET_LAYOUT_PROFILE = '[Layout Profile] Set a layout profiles value';
export const SAVE_LAYOUT_PROFILE = '[Layout Profile] Start to save layout profiles to webapi';
export const SAVE_SUCCESS_LAYOUT_PROFILE = '[Layout Profile] Save successfully layout profiles to webapi';
export const SAVE_FAIL_LAYOUT_PROFILE = '[Layout Profile] Save failed layout profiles to webapi';

export class LoadLayoutProfiles implements Action {
  readonly type = LOAD_LAYOUT_PROFILES;

}

export class InitLayoutProfiles implements Action {
  readonly type = INIT_LAYOUT_PROFILES;

  constructor(public payload: Profile[]) { }
}


export class SetLayoutProfile implements Action {
  readonly type = SET_LAYOUT_PROFILE;

  constructor(public payload: { name: string, value: string }) { }
}
export class SaveLayoutProfile implements Action {
  readonly type = SAVE_LAYOUT_PROFILE;
  constructor(public payload: Profile[]) {}
}
export class SaveSuccessLayoutProfile implements Action {
  readonly type = SAVE_SUCCESS_LAYOUT_PROFILE;

}
export class SaveFailLayoutProfile implements Action {
  readonly type = SAVE_FAIL_LAYOUT_PROFILE;

}

export type Actions
  = InitLayoutProfiles
  | LoadLayoutProfiles
  | SetLayoutProfile
  | SaveLayoutProfile
  | SaveSuccessLayoutProfile
  | SaveFailLayoutProfile
