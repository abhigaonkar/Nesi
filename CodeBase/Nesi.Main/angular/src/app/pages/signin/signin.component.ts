import { Component, OnInit, Renderer2, OnDestroy, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';
import { TokenService } from 'services/authentication/tokenService';
import { SignInData } from 'models/authentication/signInData';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromMessage from 'actions/layout/growlMessage';
import * as fromCurrentUser from 'actions/layout/currentUser';
import { Observable } from 'rxjs/Observable';
import { Fvr } from 'models/layout/fvr';
import { Subscription } from 'rxjs/Subscription';
import { DeviceService } from 'services/authentication/device';
import * as fromLayoutProfiles from 'actions/layout/layoutPorfile';
import { DomSanitizer } from '@angular/platform-browser';
import { CONFIG, HostContains } from '../../configuration';
import { SignalRService } from 'services/authentication/signalR.service';
import { AuthorizeService } from 'services/authentication/authorize.Service';
import { PasswordService } from 'services/authentication/password.service';
import { ValidateService } from 'services/shared/validateService';
import { MenuService } from 'services/layout/menuService';
import { NesiMenuItem } from 'models/layout/nesiMenuItem';
import { CoreService } from 'services/shared/core.service';
import { WindowRef } from 'app/services/shared/windowRef';
import { ApiResult } from 'models/core/ApiResult';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})

export class SignInComponent implements OnInit, OnDestroy, AfterViewInit {

  showAll = false;
  signInForm: FormGroup;
  changePasswordForm: FormGroup;
  currentYear: any = new Date().getFullYear();

  @ViewChild('newPass') inputEl: ElementRef;
  @ViewChild('hrefsrc') hrefSrc: ElementRef;
  titleAlert: any = 'You need to specify at least 3 characters';
  canSubmit: any = true;
  public errorMessage: string;
  sub: Subscription;
  // indicate if we need to check FVRS, only after login check FVRS.
  loading: boolean;
  ForcePasswordUsernameEditable: boolean;

  rebootTime = 0;
  rebooTime_refresh = 0;
  interval: any;

  _originPath = '';
  set originPath(value: string) {
    this._originPath = (value);
    sessionStorage.setItem(CONFIG.authentication.originPath, this._originPath);
  }

  get originPath() {
    return this._originPath;
  }

  fvrs$: Observable<Fvr[]>;
  time: any;
  showRequestPassword = false;
  showForgotPassword = false;
  showChangePassword = false;
  ctrlUrl: string;
  sid: SignInData;


  public get isChrome(): boolean {
    return !CONFIG.Only_Chrome || (this.device.isChrome || (CONFIG.ISDEBUG() || CONFIG.ISDEV()));
  }
  constructor(private fb: FormBuilder,
    private ts: TokenService,
    private ss: SignalRService,
    private as: AuthorizeService,
    private route: ActivatedRoute,
    private router: Router,
    private store: Store<fromRoot.State>,
    private device: DeviceService,
    private sanitizer: DomSanitizer,
    private ps: PasswordService,
    private vs: ValidateService,
    private ms: MenuService,
    private win: WindowRef,
    public cs: CoreService,
    private renderer: Renderer2

  ) {

    this.renderer.addClass(document.body, 'login-body');
    this.ctrlUrl = this.ts.CtrlURL;
    this.ts.CtrlURL = '';
    //  this.fvrs$ = this.store.select(fromRoot.getCurrentuser.getFVRList);
    //  CONFIG.LOG(ts.checkAuthentication(), 'checkAuthentication: ');
    //  CONFIG.LOG(ts.isSwitchedUser, 'isSwitchedUser: ');

    const soc = localStorage.getItem(CONFIG.authentication.signOutCheck);

    CONFIG.LOG(soc, 'Sign out check');
    if (!ts.checkAuthentication() || ts.isSwitchedUser || soc === '1') {
      localStorage.setItem(CONFIG.authentication.signOutCheck, '0');
      as.signOutN1().subscribe(
        (res) => {
          CONFIG.LOG(res, 'Signout success');
        },
        (err) => {
          CONFIG.LOG(err, 'Signout failure');
        }
      );
    }
   if (ts.currentUser && ts.currentUser.id) {
    CONFIG.LOG(ts.currentUser.id, 'ts.currentUser.id');
      this.as.switchUserN1(ts.currentUser.id).subscribe(
        n1response => {
        },
        n1error => {
          /*
            If the web api logged in ok, but N1 failed, we must indicate this
            to the user as an unusual situation
           */
          if (n1error.status === 401) {
            this.processSignInError('Unable to login, please contact support (2)');
            CONFIG.LOG(n1error, 'Signin failure');
          } else {
            this.processSignInError('Unable to login, please contact support (3)');
            CONFIG.LOG(n1error, 'Signin failure');
          }
        }
      )
   }

    this.canSubmit = true;
    this.signInForm = fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(4)]]
    });
    this.changePasswordForm = fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      oldPassword: ['', [Validators.required, Validators.minLength(4)]],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(8)]]
    }, {
        validator: !this.matchingPasswords('newPassword', 'confirmPassword')
      });


    const new_pwd = this.changePasswordForm.get('newPassword');
    new_pwd.valueChanges
      .filter(val => val && val.length >= 2)
      .debounceTime(500)
      .switchMap(x => this.cs.postObject<ApiResult>(CONFIG.apiURL.password.validate, { data: x }))
      .subscribe((res: ApiResult) => {
        if (!res.success) {
          new_pwd.setErrors({ data: res.message.split(',').join('<br/>') });
        } else {
          new_pwd.setErrors(null);
        }
      });


    this.fvrs$ = this.store.select(fromRoot.getCurrentuser.getFVRList);
    let path = '';
    route.queryParams.subscribe(
      params => path = params['origin']);

    CONFIG.LOG(path, 'queryParams in sign in');
    if (path != null) {
      if (path.indexOf('.aspx') >= 0) {
        const index = path.indexOf('.c');
        if (index === -1) {
          this.originPath = path;
        } else {
          path = path.substring(index + 3);
          path = CONFIG.Nesi1URL.host() + path;
          this.originPath = path;
        }
      } else {
        this.originPath = path;
      }
    } else {
      this.originPath = '';
    }

     let user = '';
     let pass = '';

     route.queryParams.subscribe(
       params => user = params['user']);
     route.queryParams.subscribe(
       params => pass = params['pass']);
      CONFIG.LOG(path, 'queryParams in sign in2');
     if (user != null && pass != null) {
       if (user.startsWith('___')) {
         user = atob(user.substr(3));
       }
       if (pass.startsWith('___')) {
         pass = atob(pass.substr(3));
       }
       CONFIG.LOG(path, 'queryParams in sign in3');
       this.sid = { username: user, password: pass, grant_type: 'password' };
       this.doSignIn(this.sid);
     }
  }

  ngAfterViewInit(): void {
    this.cs.loadBootstrapCSS();
  }
  ngOnInit() {
    this.loading = false;
    this.getRebootTime();
  }

  matchingPasswords(passwordKey: string, confirmPasswordKey: string) {
    return (group: FormGroup): { [key: string]: any } => {
      const password = group.controls[passwordKey];
      const confirmPassword = group.controls[confirmPasswordKey];

      if (password.value === confirmPassword.value) {
        return  {
          mismatchedPasswords: false
        }
      }


    }
  }

  ngOnDestroy(): void {

    if (this.time) {
      clearTimeout(this.time);
    }
    this.renderer.removeClass(document.body, 'login-body');
    this.cs.unloadBootstrapCSS();
  }

  doSignIn(signInData: SignInData) {
    this.as.signIn(signInData).subscribe(
      res => {
        this.ts.handleSignIn(res.json());
        CONFIG.LOG(res, 'signin');
        this.as.signInN1().subscribe(
          n1response => {
            this.onSignInSuccess();
          },
          n1error => {
            /*
              If the web api logged in ok, but N1 failed, we must indicate this
              to the user as an unusual situation
             */
            if (n1error.status === 401) {
              this.processSignInError('Unable to login, please contact support');
            } else {
              this.processSignInError('Unable to login, please contact support');
            }
          }
        )
      },
      err => {
        if(err.status==400)  // 400 Bad Request
        {
          this.onSignInError(err)
        }
        else{
          CONFIG.LOG(err.status, ' ERROR Sign in');
          this.processSignInError(err);
        }
      }
    );
  }

  signIn(): void {
    this.store.dispatch(new fromMessage.ClearMessage());
    if (this.signInForm.valid) {
      this.canSubmit = false;
      this.doSignIn(this.signInForm.value);
    } else {
      this.store.dispatch(new fromMessage.PushErrorMessage('Input fields are invalid.'));
    }
  }

  onSignInSuccess() {
    if (this.ts.checkAuthentication()) {
      this.as.validateToken().subscribe(
        res => {
          // CONFIG.LOG(res);
          this.ts.handleOnlineUser(res);
          this.onValidateTokenSuccess(res.user);
        },
        err => { this.onValidateTokenError(err); }
      );
    } else {

      this.onValidateTokenError(null);
    }
  }
  onSignInError(err: any) {
    let errorMessage = 'Invalid username/password';

    if (err._body) {
      const respBody = JSON.parse(err._body);
      if (respBody.error_description) {
        errorMessage = respBody.error_description;
      }
    }
    this.processSignInError(errorMessage);
  }

  processSignInError(err: string) {
    this.canSubmit = true;
    this.errorMessage = err;
    this.store.dispatch(new fromMessage.PushErrorMessage(this.errorMessage));
  }

  private pushSigninMessage() {
    //  this.ss.connectSignalR();
    // tslint:disable-next-line:triple-equals
    // CONFIG.LOG(this.ts.isSwitchedUser, 'Is swticher user in push sigin message');
    // if (!this.ts.isSwitchedUser && !CONFIG.ISDEBUG()) {
    //   // if user do not switch user push message.
    //   this.ss.FireSignalREvent(CONFIG.SignalR.Events.SignIn.name);
    // }
    this.ss.FireSignInAll();
  }
  SandboxProductionLogo()
  {
      const host = window.location.hostname.replace(/\./g, '').replace('www', '');
      return host === 'opssparkpowercorpcom'
              ? 'SparkOps_Horizontal.png'
              : host.match(/localhost/g)
                ? 'SparkOps_Horizontal_Development.png'
                : 'SparkOps_Horizontal_Sandbox.png';
  }
  private goHomePage() {
    this.canSubmit = true;
    this.showAll = false;
    this.store.dispatch(new fromLayoutProfiles.LoadLayoutProfiles());
    this.pushSigninMessage();
    this.win.invokeSignIn();
    const exp26 = new RegExp('&', 'g');
    for (let i = 0; i < 10; i++) {
      this.originPath = decodeURIComponent(this.originPath);
    }
    if (this.ctrlUrl) {
      CONFIG.LOG(this.ctrlUrl, 'ctrl url goto homepage');
      this.router.navigate([this.ctrlUrl]);
    } else if (this.originPath.indexOf('.aspx') >= 0) {

      if (this.originPath.indexOf('ticketpage.aspx') >= 0) {
        // this.win.boingNesi1(this.originPath, 'ticket', )
        //    CONFIG.LOG('ticket fix', this.win.generateNesi1Url(this.originPath));
        setTimeout(() => {
          window.location.href = this.win.generateNesi1Url(this.originPath);
        }, 1500)
      }
      this.router.navigate(['/home/0/' +
        encodeURIComponent(this.originPath.replace(exp26, '%26') + (this.originPath.indexOf('is_n1') < 0 ? '%26is_n1=true' : ''))]);

    } else if (this.originPath.indexOf('/home/1') >= 0) {
      this.router.navigate([this.originPath]);
    } else if (this.ts.CurrentURL && !this.ts.isSwitchedUser) {
      CONFIG.LOG(this.ts.CurrentURL, 'current url goto homepage');
      this.router.navigate([this.ts.CurrentURL]);
    } else if (CONFIG.ISDEBUG() && !this.ts.isSwitchedUser && CONFIG.DEBUG_HOMEPAGE() !== this.ts.deniedUrl) {
      CONFIG.LOG(CONFIG.DEBUG_HOMEPAGE(), 'CONFIG debug homepage goto homepage');
      this.router.navigate([CONFIG.DEBUG_HOMEPAGE()]);
    } else {

      if (this.originPath.indexOf('.aspx') >= 0) {
        this.router.navigate(['/home/0' +
          encodeURIComponent(this.originPath.replace(exp26, '%26') + (this.originPath.indexOf('is_n1') < 0 ? '%26is_n1=true' : ''))]);
      } else {
        CONFIG.LOG(document.referrer, 'referrer');
        this.router.navigate(['/home/1/']);
      }

    }
  }

  private goFvrPage() {
    this.canSubmit = true;
    this.showAll = true;
    // this.store.dispatch(new fromLayoutProfiles.LoadLayoutProfiles());
    this.pushSigninMessage();
    if (CONFIG.ISDEBUG() && CONFIG.SKIPFVR) {
      this.router.navigate([CONFIG.DEBUG_HOMEPAGE()]);
    } else {
      this.router.navigate(['/fvrs']);
    }
  }

  get usenameNotValidated(): boolean {
    return this.vs.isNotValidateRequestAndMinLength(this.signInForm, 'username');
  }

  get passwordNotValidated(): boolean {
    return this.vs.isNotValidateRequestAndMinLength(this.signInForm, 'password');
  }

  get usenameCpNotValidated(): boolean {
    return this.vs.isNotValidateRequestAndMinLength(this.changePasswordForm, 'username');
  }

  get oldpasswordCpNotValidated(): boolean {
    return this.vs.isNotValidateRequestAndMinLength(this.changePasswordForm, 'oldPassword');
  }
  get newpasswordCpNotValidated(): boolean {
    return this.vs.isNotValidateRequestAndMinLength(this.changePasswordForm, 'newPassword');
  }
  get confirmpasswordCpNotValidated(): boolean {
    return this.vs.isNotValidateRequestAndMinLength(this.changePasswordForm, 'confirmPassword')
  }
  get confirmpasswordCpNotMatch(): boolean {
    var newPassword=this.changePasswordForm.get('newPassword').value;
    var confirmPassword=this.changePasswordForm.get('confirmPassword').value;
    var mischange=this.changePasswordForm.get('newPassword').value===this.changePasswordForm.get('confirmPassword').value;
    return mischange//this.changePasswordForm.hasError('mismatchedPasswords')
      && this.changePasswordForm.get('newPassword').value.length>=0;
  }

  get hasConfirmPWDValue():boolean{
     return this.changePasswordForm.get('confirmPassword').value.length>0;
  }

  get hasNewPWDValue():boolean{
    return this.changePasswordForm.get('newPassword').value.length>0;
 }

  get newpasswordNotValidated(): boolean {
    return this.changePasswordForm.get('newPassword').hasError('data');
  }


  get PasswordTooShort():boolean{
    if (this.changePasswordForm.get('newPassword').value.length <8) {
      return true;
    }
    return false;
  }

  get PasswordNoUppercase():boolean{
    if (this.changePasswordForm.get('newPassword').value.toLowerCase() !== this.changePasswordForm.get('newPassword').value) {
        return false;
    }
    return true;
  }

  get PasswordNoLowercase():boolean{
    if (this.changePasswordForm.get('newPassword').value.toUpperCase() !== this.changePasswordForm.get('newPassword').value) {
        return false;
    }
    return true;
  }

  get newpasswordError(): string {
    var error;
    if(this.changePasswordForm.get('newPassword').hasError('data'))
    {
      error="<i class='fa fa-times'></i> ";
    }
     error =error+" "+this.changePasswordForm.get('newPassword').getError('data').replace("<br/>","<br/> <i class='fa fa-times'></i> ");
     return error;
  }

  getRebootTime() {
    const host = window.location.hostname.replace(/\./g, '').replace('www', '');
    this.cs.getData<number>(CONFIG.apiURL.currentUser.rebootTime + host)
      .subscribe(
        (res: number) => {
          if (!res) { res = 0; }
          CONFIG.LOG(res, 'reboot time in getreboottime in signin');
          this.rebootTime = res;
          if (this.rebootTime >= 0) {
            this.store.dispatch(new fromCurrentUser.SetRebootTime(this.rebootTime));
            if (this.ctrlUrl) {
              this.goHomePage();
            } else {
              this.onSignInSuccess();
            }
          } else {
            this.showAll = true;
            this.rebooTime_refresh = 0;
            this.interval = window.setInterval(
              () => {
                this.rebootTime++;
                this.rebooTime_refresh++;
                if (this.rebootTime >= 0 || this.rebooTime_refresh >= 30) {
                  window.clearInterval(this.interval);
                  window.location.reload(true);
                }
              }, 1000
            );
          }
        },
        (err: any) => {
          CONFIG.LOG(err, 'reboot time err in signin');
          this.store.dispatch(new fromMessage.PushErrorMessage(err));
          this.onValidateTokenError(err)
         // this.loading=false;
        }
      );
  }

  onValidateTokenSuccess(data: any) {
    if (this.ts.isAuthentication) {
      this.loading = true;
      // if use not a switcher user and need to change password.
      CONFIG.LOG(this.ts.isFocedChangePassword, 'onvalidatesucess is forcedchangepassword');
      if (this.ts.isFocedChangePassword) {
        this.openChangePassword();
        return;
      }
      CONFIG.LOG(this.ts.isFvrPassed, 'onvalidatesucess isfvrpassed?');
      // if (!this.ts.isFvrPassed && this.device.isMobile) {
      //   this.store.dispatch(new fromMessage.PushErrorMessage('Signin failed because you have a due FVR.'));
      //   this.ts.clearAuthData();
      //   this.canSubmit = true;
      //   return;
      // }


      CONFIG.LOG(this.ts.isContact, 'onvalidatesucess iscontact');
      // if user needs to change password, show change password window.
      this.ms.getMenus().subscribe((res: NesiMenuItem[]) => {
        this.ms.menusPageid = this.ms.ConvertMenuToInt(res);
        this.ms.getMobileMenus().subscribe((res2: NesiMenuItem[]) => {
          this.ms.mobilemenusPageid = this.ms.ConvertMenuToInt(res2);
          this.goPage();
        });
      },
        (err: any) => {
          //     this.store.dispatch(new fromMessage.PushErrorMessage('Authentication failed.'));
        }
      );
    } else {
      //  this.store.dispatch(new fromMessage.PushErrorMessage('Authentication failed.'));
    }
  }

  goPage() {
    if (!this.ts.isContact) {
      // fvr not passed need to show fvr page.
      CONFIG.LOG(this.device.isMobile, 'onvalidatesucess isMobile ');
      // this.store.dispatch(new fromCurrentUser.LoadFVRList());
      if (this.device.isMobile) {
        this.store.dispatch(new fromCurrentUser.ChangeToMobile());
        if (this.ts.currentUser && this.ts.currentUser.id) {
          this.cs.s_postData<any>(CONFIG.apiURL.currentUser.mobile_log, {
            data: this.ts.currentUser && this.ts.currentUser.id
          }).subscribe(res => { });
        }
      } else {
        CONFIG.LOG(this.ts.hasFvr, 'onvalidatesucess hasfvr ');
        this.store.dispatch(new fromCurrentUser.ChangeToDesktop());
      }
      if (this.ts.hasFvr) {
        this.goFvrPage();
      } else {
        this.goHomePage();
      }
      // contact do not need to check fvr.
    } else {
      this.goHomePage();
    }
  }
  sendForgotPassword(username: any) {
    this.ps.fogotPassword(username.value)
      .subscribe((response: ApiResult) => {
        if (response.success) {
          this.store.dispatch(new fromMessage.PushInfoMessage(response.message))
          this.showForgotPassword = false;
        } else {
          this.store.dispatch(new fromMessage.PushErrorMessage(response.message))
        }
      },
      (err:any)=>{
        this.store.dispatch(new fromMessage.PushErrorMessage(err))
      });
  }

  sendRequestPassword(email: any) {
    this.ps.requestPassword(email.value)
      .subscribe((response: ApiResult) => {
        if (response.success) {
          this.store.dispatch(new fromMessage.PushInfoMessage(response.message))
          this.showRequestPassword = false;
        } else {
          this.store.dispatch(new fromMessage.PushErrorMessage(response.message))
        }
      },
      (err:any)=>{
        this.store.dispatch(new fromMessage.PushErrorMessage(err))
      });

  }

  openChangePassword() {
    this.showChangePassword = true;
    this.canSubmit = true;
    this.showAll = true;
    let data: SignInData = this.signInForm.value;
    if (this.sid && this.sid.username !== '') {
      data = this.sid;
    } else {
      this.ForcePasswordUsernameEditable = true;
    }

    this.changePasswordForm.setValue({
      username: data.username.trim(),
      oldPassword: '',
      newPassword: '',
      confirmPassword: ''
    });
    setTimeout(() => {
      this.inputEl.nativeElement.focus();
    }, 300);
  }

  postChangePassword() {
    if (this.changePasswordForm.valid) {
      this.ps.changePassword(this.changePasswordForm.value)
        .subscribe((msg: string) => {
          if (msg.indexOf('success') > -1) {
            this.store.dispatch(new fromMessage.PushSuccessMessage(msg));
            this.showChangePassword = false;
            //  this.ts.passFocedChangePassword();
            this.onSignInSuccess();
          } else {
            this.store.dispatch(new fromMessage.PushErrorMessage(msg));
          }
        },
          (err: any) => {
            this.store.dispatch(new fromMessage.PushErrorMessage('Change password failed, please contact with IT.'));
          });
    } else {
      this.store.dispatch(new fromMessage.PushErrorMessage('Input fields are invalid.'));
    }
  }

  cancelChangePassword(): void {
    this.showChangePassword = false;
    this.canSubmit = true;

    this.ts.clearAuthData();


  }
  onValidateTokenError(err: any) {
    if (this.ts.currentAuthData) {
      //  this.store.dispatch(new fromMessage.PushErrorMessage('Authentication failed.'));
    }
    this.showAll = true
    this.canSubmit = true;
    this.ts.clearAuthData();
    // throw(err);
  }
}
