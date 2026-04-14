import * as fromNesiMenu from '../../actions/layout/nesiMenu';
import { NesiMenuItem } from '../../models/layout/nesiMenuItem';

export interface State {
  mainMenu: NesiMenuItem[];
  mobileMenu: NesiMenuItem[];
  mainMenuExpandAll: boolean;
}

let initialState: State = {
  mainMenu: [],
  mobileMenu: [],
  mainMenuExpandAll: false
};

export function reducer(state = initialState, action: fromNesiMenu.Actions): State {
  switch (action.type) {
    case fromNesiMenu.LOAD_NESIMENUS:
      return state;
    case fromNesiMenu.INIT_NESIMENUS:
      initialState = Object.assign({}, state, { mainMenu: action.payload });
      return initialState;
    case fromNesiMenu.LOAD_MOBILE_NESIMENUS:
      return state;
    case fromNesiMenu.INIT_MOBILE_NESIMENUS:
      initialState = Object.assign({}, state, { mobileMenu: action.payload });
      return initialState;
    case fromNesiMenu.RESET_NESIMENUS:
      return state;
    case fromNesiMenu.SEARCH_NESIMENUS:
      return  Object.assign({}, state, {
        mainMenu: searchMenus(state.mainMenu, action.payload),
        mainMenuExpandAll: action.payload !== '',
      });
    case fromNesiMenu.OPEN_MENUEXPANDALL:
      return Object.assign({}, state, {
        mainMenuExpandAll: true
      });
    case fromNesiMenu.CLOSE_MENUEXPANDALL:
      return Object.assign({}, state, {
        mainMenuExpandAll: false
      });
    default:
      return state;
  }

}


export const getMainMenus = (state: State) => state.mainMenu;

export const getMobileMenus = (state: State) => state.mobileMenu;

export const getMainMenuExpandAll = (state: State) => state.mainMenuExpandAll;

function searchMenus(state: NesiMenuItem[], payload: string): NesiMenuItem[] {
  const newMenu: NesiMenuItem[] = [];
  initialState.mainMenu.forEach(item => {
    const menu: NesiMenuItem = Object.assign({}, item);
    menu.items = [];
    if (item.items) {
      item.items.forEach(subItem => {
        if (subItem.label.toLowerCase().indexOf(payload.toLowerCase()) > -1) {
          // subItem.label = subItem.label.replace(payload, '<span style="color:red">{payload}</span>');
          menu.items.push(subItem);
        }
      });
    }
    if (menu.label.toLowerCase().indexOf(payload.toLowerCase()) > -1 || (menu.items && menu.items.length > 0)) {
      // menu.label = menu.label.replace(payload, '<span style="color:red">{payload}</span>');
      newMenu.push(menu);
    }
  });
  return newMenu;
}
