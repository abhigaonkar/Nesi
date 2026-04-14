import { Action } from '@ngrx/store';
import { GrowlMessage } from '../../models/layout/growlMessage';


export const PUSH_MESSAGE = '[MESSAGE] Show message';
export const PUSH_SUCCESS_MESSAGE = '[MESSAGE] Show Success message';
export const PUSH_INFO_MESSAGE = '[MESSAGE] Show Info message';
export const PUSH_WARN_MESSAGE = '[MESSAGE] Show Warn message';
export const PUSH_ERROR_MESSAGE = '[MESSAGE] Show Error message';
export const PUSH_MULTIPLE_MESSAGE = '[MESSAGE] Show Multiple messages';
export const SLICE_MESSAGE = '[MESSAGE] Remove a message';
export const CLEAR_MESSAGE = '[MESSAGE] Clear all messages';


export class SliceMessage implements Action {
  readonly type = SLICE_MESSAGE;

  constructor(public payload: GrowlMessage) { }
}

export class ClearMessage implements Action {
  readonly type = CLEAR_MESSAGE;

  constructor() { }
}
export class PushMessage implements Action {
  readonly type = PUSH_MESSAGE;

  constructor(public payload: GrowlMessage) { }
}

export class PushSuccessMessage implements Action {
  readonly type = PUSH_SUCCESS_MESSAGE;

  constructor(public payload: string) { }
}
export class PushInfoMessage implements Action {
  readonly type = PUSH_INFO_MESSAGE;

  constructor(public payload: string) { }
}
export class PushWarnMessage implements Action {
  readonly type = PUSH_WARN_MESSAGE;

  constructor(public payload: string) { }
}
export class PushErrorMessage implements Action {
  readonly type = PUSH_ERROR_MESSAGE;

  constructor(public payload: string) { }
}
export class PushMultipleMessage implements Action {
  readonly type = PUSH_MULTIPLE_MESSAGE;

  constructor(public payload: GrowlMessage[]) { }
}
export type Actions
  = PushMessage
  | PushInfoMessage
  | PushWarnMessage
  | PushErrorMessage
  | PushMultipleMessage
  | PushSuccessMessage
  | SliceMessage
  | ClearMessage
