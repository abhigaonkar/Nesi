import {TokenService} from '../services/authentication/tokenService';
import { XHRBackend } from '@angular/http';
import { HttpService } from './http.service';
import { LoaderService } from './loader/loader.service';
import { NesiRequestOptions } from './nesi-request-options';
import { ErrorLogService } from './errorhandler/error-log-service';

function httpServiceFactory(backend: XHRBackend, options: NesiRequestOptions,
  loaderService: LoaderService ) {
    return new HttpService(backend, options, loaderService);
}

export { httpServiceFactory };
