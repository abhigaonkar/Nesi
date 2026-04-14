import { Component, OnInit, OnDestroy, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../../services/authentication/tokenService';
import { CONFIG } from '../../configuration';
import { DomSanitizer } from '@angular/platform-browser';
import { AuthorizeService } from '../../services/authentication/authorize.Service';
import { SignalRService } from '../../services/authentication/signalR.service';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-signout',
  templateUrl: './signout.component.html',
  styleUrls: ['./signout.component.css']
})
export class SignOutComponent implements OnInit, OnDestroy {

  signOutUrl

  constructor(
    private ts: TokenService,
    private as: AuthorizeService,
    private ss: SignalRService,
    private router: Router,
    private cs: CoreService,
    private renderer: Renderer2
  ) {
    this.renderer.addClass(document.body, 'login-body');
  }
  ngOnDestroy() {
    this.renderer.removeClass(document.body, 'login-body');
    this.cs.unloadBootstrapCSS();
  }
  ngOnInit() {
    try {
      this.as.signOut().subscribe(
        () => this.signOut(),
        (err) => this.signOut()
      );
    } catch (e) {
      this.signOut();
    }
  }

  signOut(): void {
    // if (!CONFIG.ISDEBUG()) {
    //   this.ss.FireSignalREvent(CONFIG.SignalR.Events.SignOut.name)
    // }
    try {
      this.ss.FireSignOutAll();
      if (!CONFIG.ISDEBUG()) {
        this.as.signOutN1().subscribe(
          (res) => {
            CONFIG.LOG(res, 'Signout success');
          },
          (err) => {
            CONFIG.LOG(err, 'Signout failure');
          }
        );
      }
    } catch (e) {

    }
    this.ts.clearAuthData();
    this.cs.loadBootstrapCSS();
    this.router.navigate(['signin']);
  }
}
