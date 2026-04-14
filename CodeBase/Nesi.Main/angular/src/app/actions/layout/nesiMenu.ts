import { Action } from '@ngrx/store';
import { NesiMenuItem } from '../../models/layout/nesiMenuItem';

export const LOAD_NESIMENUS = '[nesiMenu] Start to Load Main Menu';
export const INIT_NESIMENUS = '[nesiMenu] Init Main Menu';
export const LOAD_MOBILE_NESIMENUS = '[nesiMenu] Start to Load Mobile Menu';
export const INIT_MOBILE_NESIMENUS = '[nesiMenu] Init Mobile Menu';
export const SEARCH_NESIMENUS = '[nesiMenu] Search Main Menu';
export const RESET_NESIMENUS = '[nesiMenu] Reset Main Menu';


export const OPEN_MENUEXPANDALL = '[ToggleState] Open Main Menu Expand All';
export const CLOSE_MENUEXPANDALL = '[ToggleState] Close Main Menu Expand All';

export class OpenMenuExpandAll implements Action {
  readonly type= OPEN_MENUEXPANDALL;
}


export class CloseMenuExpandAll implements Action {
  readonly type= CLOSE_MENUEXPANDALL;
}

export class LoadMenu implements Action {
  readonly type= LOAD_NESIMENUS;

}

export class InitMenu implements Action {
  readonly type= INIT_NESIMENUS;

  constructor(public payload: NesiMenuItem[]) {}
}
export class LoadMobileMenu implements Action {
  readonly type= LOAD_MOBILE_NESIMENUS;

}

export class InitMobileMenu implements Action {
  readonly type= INIT_MOBILE_NESIMENUS;

  constructor(public payload: NesiMenuItem[]) {}
}
export class ResetMenu implements Action {
  readonly type= RESET_NESIMENUS;
}

export class SearchMenu implements Action {
  readonly type= SEARCH_NESIMENUS;

  constructor(public payload: string) {}
}

export type Actions
= LoadMenu
| ResetMenu
| SearchMenu
| InitMenu
| OpenMenuExpandAll
| CloseMenuExpandAll
| LoadMobileMenu
| InitMobileMenu
