import { createSelector } from 'reselect';
import { ActionReducer, ActionReducerMap } from '@ngrx/store';
import * as fromRouter from '@ngrx/router-store';
import { environment } from '../../environments/environment';
// import { storeFreeze } from 'ngrx-store-freeze';
import { combineReducers } from '@ngrx/store';

import * as fromNesiMenu from './layout/nesiMenus';
import * as fromCurrentUser from './layout/currentUser'
import * as fromMessage from './layout/growlMessage';
import * as fromLayoutProfile from './layout/layoutProfile';
import { getCurrentPageName, getRefreshTime } from './layout/currentUser';

export interface State {
  nesiMenu: fromNesiMenu.State,
  activeUserList: fromCurrentUser.State,
  messages: fromMessage.State,
  layoutProfiles: fromLayoutProfile.State,
}

/**
 * Because metareducers take a reducer function and return a new reducer,
 * we can use our compose helper to chain them together. Here we are
 * using combineReducers to make our top level reducer, and then
 * wrapping that in storeLogger. Remember that compose applies
 * the result from right to left.
 */
// const reducers = {
//   nesiMenu: fromNesiMenu.reducer,
//   activeUserList: fromCurrentUser.reducer,
//   messages: fromMessage.reducer,
//   layoutProfiles: fromLayoutProfile.reducer,
// };

// const developmentReducer: ActionReducer<State> = compose(storeFreeze, combineReducers)(reducers);
export const reducer: ActionReducerMap<State> = {
  nesiMenu: fromNesiMenu.reducer,
  activeUserList: fromCurrentUser.reducer,
  messages: fromMessage.reducer,
  layoutProfiles: fromLayoutProfile.reducer,
};

// export function reducer(state: any, action: any) {
//   if (environment.production) {
//     return productionReducer(state, action);
//   } else {
//     return productionReducer(state, action);
//   }
// }

export const getNesiMenuStateReducer = (state: State) => state.nesiMenu;
export const getCurrentUserReducer = (state: State) => state.activeUserList;
export const getMessageReducer = (state: State) => state.messages;
export const getLayoutProfileReducer = (state: State) => state.layoutProfiles;

export const getMainMenuExpandAll = createSelector(getNesiMenuStateReducer, fromNesiMenu.getMainMenuExpandAll);
export const getMainMenus = createSelector(getNesiMenuStateReducer, fromNesiMenu.getMainMenus);
export const getMobileMenus = createSelector(getNesiMenuStateReducer, fromNesiMenu.getMobileMenus);
export const getMessages = createSelector(getMessageReducer, fromMessage.getMessages);

export const getLayoutProfilesStatus = createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayoutProfilesStatus);

export const getCurrentuser = {
  getActiveUserList: createSelector(getCurrentUserReducer, fromCurrentUser.getActiveUserList),
  getFVRList: createSelector(getCurrentUserReducer, fromCurrentUser.getFVRList),
  getIsMobile: createSelector(getCurrentUserReducer, fromCurrentUser.getIsMobile),
  getCurrentPageId: createSelector(getCurrentUserReducer, fromCurrentUser.getCurrentPageId),
  getCurrentPageName: createSelector(getCurrentUserReducer, fromCurrentUser.getCurrentPageName),
  getRefreshTime: createSelector(getCurrentUserReducer, fromCurrentUser.getRefreshTime),
  getRebootTime: createSelector(getCurrentUserReducer, fromCurrentUser.getRebootTime),
  getTODOList: createSelector(getCurrentUserReducer, fromCurrentUser.getToDoList),
  getMessageList: createSelector(getCurrentUserReducer, fromCurrentUser.getMessageList),
  getTicketList: createSelector(getCurrentUserReducer, fromCurrentUser.getTicketList)
}

export const getLayoutProfiles = {
  Profiles: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_Profiles),
  MenuMode: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_MenuMode),
  MenuStyle: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_MenuStyle),
  LayoutColor: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_LayoutColor),
  Theme: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_Theme),
  MessageLifeTime: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_MessageLifeTime),
  SignOutTime: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_SignOutTime),
  ToggleTasksIcon: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_ToggleTasksIcon),
  ToggleMessageIcon: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_ToggleMessageIcon),
  ToggleTicketIcon: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_ToggleTicketsIcon),
  ToggleNotificationWhenOtherSignedIn:
    createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_ToggleNotificationWhenOtherSignedIn),
  ToggleNotificationWhenOtherSignedOut:
    createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_ToggleNotificationWhenOtherSignedOut),
  ToggleOnlineUserBadge: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_ToggleOnlineUserBadge),
  ToggleOnlineUserIcon: createSelector(getLayoutProfileReducer, fromLayoutProfile.getLayout_ToggleOnlineUserIcon),
}
