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
import { BannerService } from '../../services/layout/banner.service';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import { OnlineUser } from '../../models/authentication/onlineUser';
import { Fvr } from '../../models/layout/fvr';
import { ToDo } from '../../models/layout/todo';
import { Message } from '../../models/banner/message';
import { Ticket } from '../../models/layout/ticket';
import { defer } from 'rxjs/observable/defer';


@Injectable()
export class CurrentuserEffects {

  @Effect()
  loadActiveList$: Observable<Action> = this.actions$
    .ofType<fromCurrentUser.LoadActiveUserList>(fromCurrentUser.LOAD_ACTIVEUSERLIST)
    .switchMap(() => {
      return this.sv.loadActiveUserList()
        .map((res: OnlineUser[]) => new fromCurrentUser.InitActiveUserList(res))
    });

  @Effect()
  loadFVRList$: Observable<Action> = this.actions$
    .ofType<fromCurrentUser.LoadFVRList>(fromCurrentUser.LOAD_FVRLIST)
    .switchMap(() => {
      return this.sv.loadFVRList()
        .map((res: Fvr[]) => new fromCurrentUser.InitFVRList(res))
    });

  @Effect()
  loadTODOList$: Observable<Action> = this.actions$
    .ofType<fromCurrentUser.LoadTODOList>(fromCurrentUser.LOAD_TODOLIST)
    .switchMap(() => {
      return this.sv.loadTODOList()
        .map((res: ToDo[]) => new fromCurrentUser.InitTODOList(res))
    });

  @Effect()
  loadMessageList$: Observable<Action> = this.actions$
    .ofType<fromCurrentUser.LoadMESSAGEList>(fromCurrentUser.LOAD_MESSAGELIST)
    .switchMap(() => {
      return this.sv.loadMESSAGEList()
        .map((res: Message[]) => new fromCurrentUser.InitMESSAGEList(res))
    });
  @Effect()
  loadTicketList$: Observable<Action> = this.actions$
    .ofType<fromCurrentUser.LoadTicketList>(fromCurrentUser.LOAD_TICKETLIST)
    .switchMap(() => {
      return this.sv.loadTicketList()
        .map((res: Ticket[]) => new fromCurrentUser.InitTicketList(res))
    });
  constructor(private actions$: Actions, private sv: BannerService) { }
}
