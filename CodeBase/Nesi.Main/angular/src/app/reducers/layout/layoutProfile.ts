import * as fromLayoutProfile from '../../actions/layout/layoutPorfile';
import { Profile } from '../../models/layout/profile';
import { LayoutProfileHelper } from '../../services/layout/layoutprofile.helper';
import { CONFIG } from '../../configuration';

export enum Status {
  init = 0,
  loading = 1,
  laoded = 2,
  changed = 3,
  saving = 4,
  saveSuccessed = 5,
  saveFailed = 6
}
export interface State {
  Profiles: Profile[];
  MenuMode: string;
  MenuStyle: string;
  LayoutColor: string;
  Theme: string;
  MessageLifeTime: number;
  ToggleNotificationWhenOtherSignedIn: string;
  ToggleNotificationWhenOtherSignedOut: string;
  ToggleMessageIcon: string;
  ToggleTasksIcon: string;
  ToggleTicketIcon: string;
  ToggleOnlineUserIcon: string;
  ToggleOnlineUserBadge: string;
  SignOutTime: number;
  status: Status;
}

const initialState: State = {
  Profiles: [],
  MenuMode: 'static',
  MenuStyle: 'light',
  LayoutColor: 'default',
  Theme: 'blue',
  MessageLifeTime: 5000,
  ToggleNotificationWhenOtherSignedIn: '1',
  ToggleNotificationWhenOtherSignedOut: '1',
  ToggleOnlineUserIcon: '1',
  ToggleOnlineUserBadge: '0',
  ToggleMessageIcon: '1',
  ToggleTasksIcon: '1',
  ToggleTicketIcon: '1',
  SignOutTime: 1 * 60 * 60,
  status: Status.init
};

export function reducer(state = initialState, action: fromLayoutProfile.Actions): State {
  switch (action.type) {
    case fromLayoutProfile.LOAD_LAYOUT_PROFILES:
      return Object.assign({}, state, { status: Status.loading });
    case fromLayoutProfile.INIT_LAYOUT_PROFILES:
      const helper = new LayoutProfileHelper();
      helper.Profiles = action.payload;
      return {
        Profiles: action.payload,
        MenuMode: helper.MenuMode,
        MenuStyle: helper.MenuStyle,
        LayoutColor: helper.LayoutColor,
        Theme: helper.Theme,
        MessageLifeTime: helper.MessageLifeTime,
        ToggleOnlineUserIcon: helper.ToggleOnlineUserIcon,
        ToggleOnlineUserBadge: helper.ToggleOnlineUserBadge,
        ToggleNotificationWhenOtherSignedIn: helper.ToggleNotificationWhenOtherSignedIn,
        ToggleNotificationWhenOtherSignedOut: helper.ToggleNotificationWhenOtherSignedOut,
        ToggleMessageIcon: helper.ToggleMessageIcon,
        ToggleTasksIcon: helper.ToggleTasksIcon,
        ToggleTicketIcon: helper.ToggleTicketIcon,
        SignOutTime: helper.SignOutTime,
        status: Status.laoded
      };
    case fromLayoutProfile.SET_LAYOUT_PROFILE:
      const newProfiles: Profile[] = [];
      state.Profiles.forEach((p: Profile) => {
        const newProfile = Object.assign({}, p);
        newProfiles.push(newProfile);
      });
      const newHelper = new LayoutProfileHelper();
      newHelper.Profiles = newProfiles;
      switch (action.payload.name) {
        case 'MenuMode':
          newHelper.MenuMode = action.payload.value;
          return Object.assign({}, state, { Profiles: newHelper.Profiles, MenuMode: action.payload.value, status: Status.changed });
        case 'MenuStyle':
          newHelper.MenuStyle = action.payload.value;
          return Object.assign({}, state, { Profiles: newHelper.Profiles, MenuStyle: action.payload.value, status: Status.changed });
        case 'LayoutColor':
          newHelper.LayoutColor = action.payload.value;
          return Object.assign({}, state, { Profiles: newHelper.Profiles, LayoutColor: action.payload.value, status: Status.changed });
        case 'Theme':
          newHelper.Theme = action.payload.value;
          return Object.assign({}, state, { Profiles: newHelper.Profiles, Theme: action.payload.value, status: Status.changed });
        case 'MessageLifeTime':
          newHelper.MessageLifeTime = Number(action.payload.value);
          return Object.assign({}, state,
            { Profiles: newHelper.Profiles, MessageLifeTime: Number(action.payload.value), status: Status.changed });
        case 'SignOutTime':
          newHelper.SignOutTime = Number(action.payload.value);
          return Object.assign({}, state,
            { Profiles: newHelper.Profiles, SignOutTime: Number(action.payload.value), status: Status.changed });
        case 'ToggleOnlineUserIcon':
          newHelper.ToggleOnlineUserIcon = action.payload.value;
          return Object.assign({}, state,
            { Profiles: newHelper.Profiles, ToggleOnlineUserIcon: action.payload.value, status: Status.changed });
        case 'ToggleOnlineUserBadge':
          newHelper.ToggleOnlineUserBadge = action.payload.value;
          return Object.assign({}, state,
            { Profiles: newHelper.Profiles, ToggleOnlineUserBadge: action.payload.value, status: Status.changed });
        case 'ToggleMessageIcon':
          newHelper.ToggleMessageIcon = action.payload.value;
          return Object.assign({}, state,
            { Profiles: newHelper.Profiles, ToggleMessageIcon: action.payload.value, status: Status.changed });
        case 'ToggleTasksIcon':
          newHelper.ToggleTasksIcon = action.payload.value;
          return Object.assign({}, state,
            { Profiles: newHelper.Profiles, ToggleTasksIcon: action.payload.value, status: Status.changed });
        case 'ToggleTicketIcon':
          newHelper.ToggleTicketIcon = action.payload.value;
          return Object.assign({}, state,
            { Profiles: newHelper.Profiles, ToggleTicketIcon: action.payload.value, status: Status.changed });
        case 'ToggleNotificationWhenOtherSignedIn':
          newHelper.ToggleNotificationWhenOtherSignedIn = action.payload.value;
          return Object.assign({}, state,
            { Profiles: newHelper.Profiles, ToggleNotificationWhenOtherSignedIn: action.payload.value, status: Status.changed });
        case 'ToggleNotificationWhenOtherSignedOut':
          newHelper.ToggleNotificationWhenOtherSignedOut = action.payload.value;
          return Object.assign({}, state,
            { Profiles: newHelper.Profiles, ToggleNotificationWhenOtherSignedOut: action.payload.value, status: Status.changed });
        default:
          return Object.assign({}, state, { status: Status.changed });
      }
    case fromLayoutProfile.SAVE_LAYOUT_PROFILE:
      return Object.assign({}, state, { status: Status.saving });
    case fromLayoutProfile.SAVE_SUCCESS_LAYOUT_PROFILE:
      return Object.assign({}, state, { status: Status.saveSuccessed });
    case fromLayoutProfile.SAVE_FAIL_LAYOUT_PROFILE:
      return Object.assign({}, state, { status: Status.saveFailed });
    default:
      return state;
  }

}

export const getLayout_Profiles = (state: State) => state.Profiles;
export const getLayout_MenuMode = (state: State) => state.MenuMode;
export const getLayout_MenuStyle = (state: State) => state.MenuStyle;
export const getLayout_LayoutColor = (state: State) => state.LayoutColor;
export const getLayout_Theme = (state: State) => state.Theme;
export const getLayout_MessageLifeTime = (state: State) => state.MessageLifeTime;
export const getLayout_SignOutTime = (state: State) => state.SignOutTime;
export const getLayout_ToggleOnlineUserIcon = (state: State) => state.ToggleOnlineUserIcon;
export const getLayout_ToggleMessageIcon = (state: State) => state.ToggleMessageIcon;
export const getLayout_ToggleTasksIcon = (state: State) => state.ToggleTasksIcon;
export const getLayout_ToggleTicketsIcon = (state: State) => state.ToggleTicketIcon;
export const getLayout_ToggleNotificationWhenOtherSignedIn = (state: State) => state.ToggleNotificationWhenOtherSignedIn;
export const getLayout_ToggleNotificationWhenOtherSignedOut = (state: State) => state.ToggleNotificationWhenOtherSignedOut;
export const getLayout_ToggleOnlineUserBadge = (state: State) => state.ToggleOnlineUserBadge;

export const getLayoutProfilesStatus = (state: State) => state.status;
