import * as fromMessage from '../../actions/layout/growlMessage';
import { GrowlMessage } from '../../models/layout/growlMessage';
import { CONFIG } from '../../configuration';
export interface State {
  messages: GrowlMessage[];
}

const initialState: State = {
  messages: []
}

function PushNewMessage(state: State, msg: GrowlMessage): State {
  const newState = Object.assign({}, state, {
    messages: [msg]
  });

  // setTimeout(() => {
  //   const index = newState.messages.indexOf(msg);
  //   if (index > -1) {
  //     newState.messages.slice(index, 1);
  //   }
  // }, CONFIG.messageLifeTime);
  return newState;
}

export function reducer(state = initialState, action: fromMessage.Actions): State {
  switch (action.type) {
    case fromMessage.PUSH_MESSAGE:
      return Object.assign({}, state, {
        messages: [action.payload]
      });
    case fromMessage.PUSH_ERROR_MESSAGE:
      return PushNewMessage(state, {
        severity: 'error',
        summary: 'Error Message',
        detail: action.payload
      });
    case fromMessage.PUSH_INFO_MESSAGE:
      return PushNewMessage(state, {
        severity: 'info',
        summary: 'Info Message',
        detail: action.payload
      });
    case fromMessage.PUSH_SUCCESS_MESSAGE:
      return PushNewMessage(state, {
        severity: 'success',
        summary: 'Success Message',
        detail: action.payload
      });
    case fromMessage.PUSH_WARN_MESSAGE:
      return PushNewMessage(state, {
        severity: 'warn',
        summary: 'Warning Message',
        detail: action.payload
      });
    case fromMessage.PUSH_MULTIPLE_MESSAGE:
      return Object.assign({}, state, {
        messages: [...action.payload]
      });
    case fromMessage.CLEAR_MESSAGE:
      return Object.assign({}, state, {
        messages: []
      });
    case fromMessage.SLICE_MESSAGE:
      const index = state.messages.indexOf(action.payload);
      if (index > -1) {
        return Object.assign({}, state, {
          messages: state.messages.slice(index, 1)
        });
      } else {
        return state;
      }
    default:
      return state;
  }
}

export const getMessages = (state: State) => state.messages;
