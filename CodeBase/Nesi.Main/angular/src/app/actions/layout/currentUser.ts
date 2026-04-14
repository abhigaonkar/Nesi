import { Action } from '@ngrx/store';
import { OnlineUser } from '../../models/authentication/onlineUser';
import { Fvr } from '../../models/layout/fvr';
import { ToDo } from '../../models/layout/todo';
import { Message } from '../../models/banner/message';
import { Ticket } from '../../models/layout/ticket';

export const INIT_ACTIVEUSERLIST = '[CurrentUser] Init acitve user list';
export const LOAD_ACTIVEUSERLIST = '[CurrentUser] Load acitve user list';
export const INIT_FVRLIST = '[CurrentUser] Init FVR list';
export const LOAD_FVRLIST = '[CurrentUser] Load FVR list';
export const INIT_TODOLIST = '[CurrentUser] Init TODO list';
export const LOAD_TODOLIST = '[CurrentUser] Load TODO list';
export const INIT_MESSAGELIST = '[CurrentUser] Init MESSAGE list';
export const LOAD_MESSAGELIST = '[CurrentUser] Load MESSAGE list';
export const INIT_TICKETLIST = '[CurrentUser] Init TICKET list';
export const LOAD_TICKETLIST = '[CurrentUser] Load TICKET list';
export const CHANGE_TO_MOBILE = '[CurrentUser] CHANGE Layout to Mobile';
export const CHANGE_TO_DESKTOP = '[CurrentUser] CHANGE Layout to Desktop';
export const SET_CURRENT_PAGEID = '[CurrentUser] SET Current Page Id';
export const SET_CURRENT_PAGENAME = '[CurrentUser] SET Current Page Name';
export const SET_REBOOT_TIME = '[CurrentUser] SET Reboot Time';
export const SET_REFRESH_TIME = '[CurrentUser] SET Refresh Time';


export class LoadActiveUserList implements Action {
  readonly type = LOAD_ACTIVEUSERLIST;
}

export class InitActiveUserList implements Action {
  readonly type = INIT_ACTIVEUSERLIST;

  constructor(public payload: OnlineUser[]) { }
}

export class LoadFVRList implements Action {
  readonly type = LOAD_FVRLIST;

}
export class InitFVRList implements Action {
  readonly type = INIT_FVRLIST;
  constructor(public payload: Fvr[]) { }
}
export class LoadTODOList implements Action {
  readonly type = LOAD_TODOLIST;

}
export class InitTODOList implements Action {
  readonly type = INIT_TODOLIST;
  constructor(public payload: ToDo[]) { }
}
export class LoadMESSAGEList implements Action {
  readonly type = LOAD_MESSAGELIST;

}
export class InitMESSAGEList implements Action {
  readonly type = INIT_MESSAGELIST;
  constructor(public payload: Message[]) { }
}
export class LoadTicketList implements Action {
  readonly type = LOAD_TICKETLIST;

}
export class InitTicketList implements Action {
  readonly type = INIT_TICKETLIST;
  constructor(public payload: Ticket[]) { }
}
export class ChangeToMobile implements Action {
  readonly type = CHANGE_TO_MOBILE;
}

export class ChangeToDesktop implements Action {
  readonly type = CHANGE_TO_DESKTOP;
}
export class SetCurrentPageId implements Action {
  readonly type = SET_CURRENT_PAGEID;
  constructor(public payload: number) { }
}

export class SetCurrentPageName implements Action {
  readonly type = SET_CURRENT_PAGENAME;
  constructor(public payload: string) { }
}
export class SetRebootTime implements Action {
  readonly type = SET_REBOOT_TIME;
  constructor(public payload: number) { }
}
export class SetRefreshTime implements Action {
  readonly type = SET_REFRESH_TIME;
  constructor(public payload: number) { }
}

export type Actions
  = LoadActiveUserList
  | InitActiveUserList
  | LoadFVRList
  | InitFVRList
  | LoadTODOList
  | InitTODOList
  | LoadMESSAGEList
  | InitMESSAGEList
  | LoadTicketList
  | InitTicketList
  | ChangeToMobile
  | ChangeToDesktop
  | SetCurrentPageId
  | SetCurrentPageName
  | SetRebootTime
  | SetRefreshTime
