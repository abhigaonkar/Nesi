
import { Component, OnInit, Renderer2, OnDestroy, AfterViewInit, ElementRef, ViewChild,EventEmitter,Output } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';
import { TokenService } from 'services/authentication/tokenService';
import { SignInData } from 'models/authentication/signInData';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromMessage from 'actions/layout/growlMessage';
import * as fromCurrentUser from 'actions/layout/currentUser';
import { Observable } from 'rxjs/Observable';
import { Fvr } from 'models/layout/fvr';
import { Subscription } from 'rxjs/Subscription';
import { DeviceService } from 'services/authentication/device';
import * as fromLayoutProfiles from 'actions/layout/layoutPorfile';
import { DomSanitizer } from '@angular/platform-browser';
import { CONFIG } from '../../../configuration';
import { SignalRService } from 'services/authentication/signalR.service';
import { AuthorizeService } from 'services/authentication/authorize.Service';
import { PasswordService } from 'services/authentication/password.service';
import { ValidateService } from 'services/shared/validateService';
import { MenuService } from 'services/layout/menuService';
import { NesiMenuItem } from 'models/layout/nesiMenuItem';
import { CoreService } from '../../../services/shared/core.service';
import { WindowRef } from 'app/services/shared/windowRef';
import { ApiResult } from 'models/core/ApiResult';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-change-password-banner',
  templateUrl: './change-password-banner.component.html',
  styleUrls: ['./change-password-banner.component.css']
})
// tslint:disable-next-line:class-name
export class ChangePasswordBannerComponent extends FormMessageBase implements OnInit {

  showAll = false;
  canSubmit: any = true;
  username: string;
  @Output() cancel = new EventEmitter();
  isLdapUser = false;
  showChangePassword = false;
  loading: boolean;
  tesing =true;
  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    private as: AuthorizeService,
    private ps: PasswordService,
    private vs: ValidateService,
    private ms: MenuService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private route: Router,

  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.layout.changepassword2);

  }

  submitValidate(): boolean {
    return this.match_pw();
  }

  createForm() {
    this.userform = this.fb.group({
      // tslint:disable-next-line:max-line-length
      'username':['', [Validators.required, Validators.minLength(3)]],
      'oldPassword': ['', [Validators.required,Validators.minLength(4)]],
      'newPassword': ['', [Validators.required,Validators.minLength(8)]],
      // tslint:disable-next-line:max-line-length
      'confirmPassword': ['', [Validators.required,Validators.minLength(8)]],
      'sync_pass': false
    },{
      validator: !this.matchingPasswords('newPassword', 'confirmPassword')
    });

    
    const new_pwd = this.userform.get('newPassword');
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

    this.initFormvalue = {
      username:this.username,
      oldPassword: '',
      newPassword: '',
      confirmPassword: '',
      sync_pass: false,
    };

  }

  get confirmpasswordCpNotMatch(): boolean {
  
    var mischange=this.userform.get('newPassword').value===this.userform.get('confirmPassword').value;

    return mischange//this.changePasswordForm.hasError('mismatchedPasswords')
      && this.userform.get('newPassword').value.length>=0;
  }

  get hasConfirmPWDValue():boolean{
    if(this.userform.get('confirmPassword').value!=null)
    {
      return this.userform.get('confirmPassword').value.length>0;
    }
    return false;
 }

  get hasNewPWDValue():boolean{
    if(this.userform.get('newPassword').value!=null)
    {
       return this.userform.get('newPassword').value.length>0;
    }
     return false;
 }

  get PasswordTooShort():boolean{
    if (this.userform.get('newPassword').value.length <8) {
      return true;
    }
    return false;
  }

  get PasswordNoUppercase():boolean{
    if (this.userform.get('newPassword').value.toLowerCase() !== this.userform.get('newPassword').value) {
        return false;
    }
    return true;
  }

  get PasswordNoLowercase():boolean{
    if (this.userform.get('newPassword').value.toUpperCase() !== this.userform.get('newPassword').value) {
        return false;
    }
    return true;
  }

  get newpasswordError(): string {
    return this.userform.get('newPassword').getError('data');
  }

  match_pw(): boolean {
    if (this.userform.get('newPassword').value !== this.userform.get('confirmPassword').value) {
      this.PushErrorMessage('Passwords do not match!');
      return false;
    }
    return true;
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

  postChangePassword() {
    if (this.userform.valid) {
      this.ps.changePassword(this.userform.value)
        .subscribe((msg: string) => {
          if (msg.indexOf('success') > -1) {
            this.store.dispatch(new fromMessage.PushSuccessMessage(msg));
            this.showChangePassword = false;
             this.ts.passFocedChangePassword();
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



  onValidateTokenSuccess(data: any) {
    if (this.ts.isAuthentication) {
      this.loading = true;
      // if use not a switcher user and need to change password.
      CONFIG.LOG(this.ts.isFocedChangePassword, 'onvalidatesucess is forcedchangepassword');
      if (this.ts.isFocedChangePassword) {
       // this.openChangePassword();
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
        //  this.goPage();
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

  onValidateTokenError(err: any) {
    if (this.ts.currentAuthData) {
      //  this.store.dispatch(new fromMessage.PushErrorMessage('Authentication failed.'));
    }
    this.showAll = true
    this.canSubmit = true;
    this.ts.clearAuthData();
    // throw(err);
  }

  submitReset() {
    this.userform.reset();
  }

  submitSuccess() {
    this.cancel.emit();
    this.submitting = true;
    this.signOut();
  }

  signOut() {
    setTimeout(() => {
      this.ClearMessage();
      setTimeout(() => {
        alert('Password has been changed successfully, please sign in again.');
        this.route.navigate(['/signout']);
      }, 300);
    }, 500);
  }


  ngOnInit() {
    this.isLdapUser = this.ts.currentUser.isLdapUser;

    // this is not needed right now
    this.cs.getString(CONFIG.apiURL.layout.changepasswordgetusername1)
      .subscribe(
        (response) => {
          this.username = response;
          this.userform.setValue({
            username: this.username.trim(),
            oldPassword: '',
            newPassword: '',
            confirmPassword: '',
            sync_pass: false
          });
        }
      );

      
 
  }

}
