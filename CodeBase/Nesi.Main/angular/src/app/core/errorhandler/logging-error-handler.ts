// Import the core angular services.
import { ErrorHandler } from '@angular/core';
import { forwardRef } from '@angular/core';
import { Inject } from '@angular/core';
import { Injectable } from '@angular/core';
import { ErrorLogService } from './error-log-service';
import { LoggingErrorHandlerOptions, ErrorOutputOptions } from './loggingErrorHandlerOptions';
import { Router } from '@angular/router';
import { GrowlMessage } from '../../models/layout/growlMessage';
import { LOGGING_ERROR_HANDLER_OPTIONS, ERROR_OUTPUT_OPTIONS, CONFIG } from '../../configuration';
import { Message } from '../../models/banner/message';


@Injectable()
export class LoggingErrorHandler implements ErrorHandler {

  private errorLogService: ErrorLogService;
  private options: LoggingErrorHandlerOptions = LOGGING_ERROR_HANDLER_OPTIONS;
  private errorOutputOptions: ErrorOutputOptions = ERROR_OUTPUT_OPTIONS;
  IgnoreErrors = [
    'Expression has changed after it was checked.',
    'ExpressionChangedAfterItHasBeenCheckedError',
    'signalr',
    'Cannot read property \'toString\' of undefined',
    'It looks like you\'re using the disabled attribute with a reactive form directive',
    'Cannot read property \'name\' of undefined',
    'attempt to use a destroyed view: detectchanges',
    'Response with status: 0',
  ];

  // I initialize the service.
  // --
  // CAUTION: The core implementation of the ErrorHandler class accepts a boolean
  // parameter, `rethrowError`; however, this is not part of the interface for the
  // class. In our version, we are supporting that same concept; but, we are doing it
  // through an Options object (which is being defaulted in the providers).
  constructor(
    errorLogService: ErrorLogService
  ) {

    this.errorLogService = errorLogService;
  }


  // ---
  // PUBLIC METHODS.
  // ---


  // I handle the given error.


  public handleError(error: any): void {



    if (error.message) {
      const msg = String(error.message).toLowerCase();
      CONFIG.LOG(msg, 'error message');
      if (msg.indexOf('error: loading chunk') > -1) {
        window.location.reload(true);
        return;
      }


      let ignore = false;
      this.IgnoreErrors.forEach(err => {
        CONFIG.LOG(msg.indexOf(err.toLowerCase()), 'err index in ingore error');
        if (msg.indexOf(err.toLowerCase()) > -1) {

          ignore = true;
        }
      });

      if (ignore) {
        return;
      }

      // return;
    }

    if (error && String(error.message) && String(error.message).startsWith('[')) {
      this.errorLogService.sendToMessage(error);
      return;
    }



    // Log to the console.
    try {
      // console.log(error);
      switch (error.status) {
        case 400:
          error.message = error.message || '[400] ' + (error.statusText || 'WebAPI Server internal error.');
          break;
        case 401:
          error.message = error.message || '[401] unauthorized. <br/> Your signin token was exprired, please signin again.';
          break;
        case 500:
          error.message = error.message || '[500] ' + (error.statusText || 'WebAPI Server internal error.');
          break;
        case 404:
          error.message = error.message || '[404] ' + (error.statusText || 'Page not found.');
          break;
        case 403:
            error.message = error.message || '[403] ' + (error.statusText || 'Forbidden');
            break;
        case 0:
          error.message = error.message || '[0] ' + (error.statusText || 'WebAPI Server Connection error.');
          break; 
        default:

          break;
      }
      // if (!CONFIG.ISDEV() && error.status !== 401) {
      //   error.Message = error.message;
      //   error.message = '[500] An error occurred, please cut a ticket if this continues.';
      //   //  this.errorLogService.sendToMessage(error);
      // }


    } catch (handlingError) {

      // console.group('ErrorHandler');
      // console.warn('Error when trying to output error.');
      // console.error(handlingError);
      CONFIG.LOG(handlingError, 'error handler');

    }


    // Send to the error-logging service.
    try {

      this.options.unwrapError
        ? this.errorLogService.logError(this.findOriginalError(error), this.errorOutputOptions)
        : this.errorLogService.logError(error, this.errorOutputOptions)
        ;

    } catch (loggingError) {
      // console.group('ErrorHandler');
      // console.warn('Error when trying to log error to', this.errorLogService);
      // console.warn(loggingError);
      // console.groupEnd();
      CONFIG.LOG(loggingError, 'error handler');

    }

    if (this.options.rethrowError) {

      // throw (error);

    }
    return;
  }


  // ---
  // PRIVATE METHODS.
  // ---


  // I attempt to find the underlying error in the given Wrapped error.
  private findOriginalError(error: any): any {

    while (error && error.originalError) {

      error = error.originalError;

    }

    return (error);

  }

}


// I am the collection of providers used for this service at the module level.
// Notice that we are overriding the CORE ErrorHandler with our own class definition.
// --
// CAUTION: These are at the BOTTOM of the file so that we don't have to worry about
// creating futureRef() and hoisting behavior.
export let LOGGING_ERROR_HANDLER_PROVIDERS = [
  {
    provide: ErrorHandler,
    useClass: LoggingErrorHandler
  }
];
