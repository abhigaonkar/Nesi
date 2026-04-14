

export interface LoggingErrorHandlerOptions {
  rethrowError: boolean;
  unwrapError: boolean;
}


export interface ErrorOutputOptions {
  sendToConsole: boolean;
  sendToGrowlMessage: boolean;
  sendToServer: boolean;
}
