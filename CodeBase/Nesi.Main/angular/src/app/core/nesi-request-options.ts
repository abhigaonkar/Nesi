import { BaseRequestOptions } from '@angular/http';
import { CONFIG } from '../configuration';

export class NesiRequestOptions extends BaseRequestOptions {

    public token: string;

    constructor () {

        super();

        let user;
        if (localStorage.getItem(CONFIG.authentication.authDataString)) {
          user = JSON.parse(atob(localStorage.getItem(CONFIG.authentication.authDataString)));
        }
        this.token = user && user.access_token;
        if (this.token) {
          this.headers.append('Content-Type', 'application/json');
          this.headers.append('Authorization', 'Bearer ' + this.token );
        }
        // if (nesiReduxOptions != null) {

        //     // tslint:disable-next-line:forin
        //     for (const option in nesiReduxOptions) {
        //         const optionValue = nesiReduxOptions[option];
        //         this[option] = optionValue;
        //     }
        // }
    }


}
