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
import { MenuService } from '../../services/layout/menuService';
import * as fromNesiMenu from '../../actions/layout/nesiMenu';


@Injectable()
export class NesiMenuEffects {

  @Effect()
  loadMenus$: Observable<Action> = this.actions$
    .ofType(fromNesiMenu.LOAD_NESIMENUS)
    .switchMap(() => {
      return this.mns.getMenus()
        .map(menus => new fromNesiMenu.InitMenu(menus))
    });

  @Effect()
  loadMobileMenus$: Observable<Action> = this.actions$
    .ofType(fromNesiMenu.LOAD_MOBILE_NESIMENUS)
    .switchMap(() => {
      return this.mns.getMobileMenus()
        .map(menus => new fromNesiMenu.InitMobileMenu(menus))
    });

  constructor(private actions$: Actions, private mns: MenuService) { }
}
