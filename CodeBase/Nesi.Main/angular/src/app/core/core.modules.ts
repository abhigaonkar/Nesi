import {TokenService} from '../services/authentication/tokenService';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { XHRBackend, RequestOptions } from '@angular/http';

import { HttpService } from './http.service';
import { httpServiceFactory } from './http-service.factory';
import { LoaderService } from './loader/loader.service';
import { LoaderComponent } from './loader/loader.component';
import { NesiRequestOptions } from './nesi-request-options';
import { LOGGING_ERROR_HANDLER_PROVIDERS } from './errorhandler/logging-error-handler';
import { ErrorLogService } from './errorhandler/error-log-service';
import { ProgressBarModule } from 'primeng/primeng';

@NgModule({
  imports: [
    CommonModule,
    ProgressBarModule,
  ],
  exports: [
    LoaderComponent
  ],
  declarations: [
    LoaderComponent
  ],
  providers: [
    LoaderService,
    {
      provide: HttpService,
      useFactory: httpServiceFactory,
      deps: [XHRBackend, RequestOptions, LoaderService]
    },
    ErrorLogService,

    // CAUTION: This providers collection overrides the CORE ErrorHandler with our
    // custom version of the service that logs errors to the ErrorLogService.
    LOGGING_ERROR_HANDLER_PROVIDERS,
  ]
})

export class CoreModule { }
