import * as fromCurrentUser from '../../actions/layout/currentUser';
import { OnlineUser } from '../../models/authentication/onlineUser';
import { Fvr } from '../../models/layout/fvr';
import { DeviceService } from '../../services/authentication/device';
import { Ng2DeviceService } from 'ng2-device-detector';
import { ToDo } from '../../models/layout/todo';
import { Message } from '../../models/banner/message';
import { Ticket } from '../../models/layout/ticket';

export interface State {
  activeUserList: OnlineUser[];
  fvrs: Fvr[];
  todo: ToDo[];
  messages: Message[];
  tickets: Ticket[];
  currentPageId: number;
  currentPageName: string;
  rebootTime: number;
  refreshTime: number;
  isMobile: boolean;
}

const initialState: State = {
  activeUserList: [],
  fvrs: [],
  todo: [],
  messages: [],
  tickets: [],
  currentPageId: 0,
  currentPageName: '',
  rebootTime: 0,
  refreshTime: 0,
  isMobile: new DeviceService(new Ng2DeviceService()).isMobile
}


export function reducer(state = initialState, action: fromCurrentUser.Actions): State {
  switch (action.type) {
    case fromCurrentUser.LOAD_ACTIVEUSERLIST:
      return state;
    case fromCurrentUser.INIT_ACTIVEUSERLIST:
      return Object.assign({}, state, {
        activeUserList: action.payload
      });
    case fromCurrentUser.LOAD_FVRLIST:
      return state;
    case fromCurrentUser.INIT_FVRLIST:
      return Object.assign({}, state, {
        fvrs: action.payload
      });
    case fromCurrentUser.LOAD_TODOLIST:
      return state;
    case fromCurrentUser.INIT_TODOLIST:
      return Object.assign({}, state, {
        todo: action.payload
      });
    case fromCurrentUser.LOAD_MESSAGELIST:
      return state;
    case fromCurrentUser.INIT_MESSAGELIST:
      return Object.assign({}, state, {
        messages: action.payload
      });
    case fromCurrentUser.LOAD_TICKETLIST:
      return state;
    case fromCurrentUser.INIT_TICKETLIST:
      return Object.assign({}, state, {
        tickets: action.payload
      });
    case fromCurrentUser.CHANGE_TO_MOBILE:
      return Object.assign({}, state, {
        isMobile: true
      });
    case fromCurrentUser.CHANGE_TO_DESKTOP:
      return Object.assign({}, state, {
        isMobile: false
      });
    case fromCurrentUser.SET_CURRENT_PAGEID:
      return Object.assign({}, state, {
        currentPageId: action.payload
      });
    case fromCurrentUser.SET_CURRENT_PAGENAME:
      return Object.assign({}, state, {
        currentPageName: action.payload
      });
    case fromCurrentUser.SET_REBOOT_TIME:
      return Object.assign({}, state, {
        rebootTime: action.payload
      });
    case fromCurrentUser.SET_REFRESH_TIME:
      return Object.assign({}, state, {
        refreshTime: action.payload
      });
    default:
      return state;
  }
}

export const getActiveUserList = (state: State) => state.activeUserList;
export const getFVRList = (state: State) => state.fvrs;
export const getToDoList = (state: State) => state.todo;
export const getMessageList = (state: State) => state.messages;
export const getTicketList = (state: State) => state.tickets;
export const getIsMobile = (state: State) => state.isMobile;
export const getCurrentPageId = (state: State) => state.currentPageId;
export const getCurrentPageName = (state: State) => state.currentPageName;
export const getRebootTime = (state: State) => state.rebootTime;
export const getRefreshTime = (state: State) => state.refreshTime;
