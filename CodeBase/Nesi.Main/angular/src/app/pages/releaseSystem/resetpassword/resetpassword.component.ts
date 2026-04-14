import { Component, OnInit, Renderer2, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';
import { CoreService } from '../../../services/shared/core.service';
import { Subscription } from 'rxjs';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from '../../../core/messageBaseComponent';
import { ApiResult } from '../../../models/core/ApiResult';
import { ValidateService } from 'services/shared/validateService';

@Component({
  selector: 'nesi-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css']
})
export class ResetPasswordComponent extends MessageBase implements OnInit, AfterViewInit {

  validateForm: FormGroup;
  resetForm: FormGroup;
  username: string;
  token: string;
  sub: Subscription;
  verified = false;
  password: string;
  repassword: string;
  canSubmit: any = true;
  currentYear: any = new Date().getFullYear()

  PasswordTooShort = false
  NoNonletterOrDigit = false
  NoLowercase = false
  NoUppercase = false
  NoDigit = false

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
    protected store: Store<fromRoot.State>,
    private cs: CoreService,
    private vs: ValidateService,
    private renderer: Renderer2
  ) {
    super(store);
    this.renderer.addClass(document.body, 'login-body');

    this.canSubmit = true;
    this.validateForm = fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]]
    });

    this.resetForm = fb.group({
      password: ['', [Validators.required, Validators.minLength(8)]],
      repassword: ['', [Validators.required, Validators.minLength(8)]]
    });
  }


  get usenameNotValidated(): boolean {
    return this.vs.isNotValidateRequestAndMinLength(this.validateForm, 'username');
  }
  get passwordNotValidated(): boolean {

    const password: string = this.resetForm.controls['password'].value;
    let hasError = false;

    if (password.length === 0) {
      return false;
    }

    if (password.length < 8) {
      hasError = true;
      this.PasswordTooShort = true;
    } else {
      this.PasswordTooShort = false;
    }

    //if ((/^[a-zA-Z0-9]+$/.test(password))) {
      //hasError = true;
     // this.NoNonletterOrDigit = true;
    //} else {
     // this.NoNonletterOrDigit = false;
    //}

    if (!this.hasLowerCase(password)) {
      hasError = true;
      this.NoLowercase = true;
    } else {
      this.NoLowercase = false;
    }

    if (!this.hasUpperCase(password)) {
      hasError = true;
      this.NoUppercase = true;
    } else {
      this.NoUppercase = false;
    }

   // if (!this.hasNumber(password)) {
    //  hasError = true;
    //  this.NoDigit = true;
   // } else {
    //  this.NoDigit = false;
    //}

    return hasError;
  }
  hasLowerCase(str) {
    if (str.toUpperCase() !== str) {
        return true;
    }
    return false;
  }

hasUpperCase(str) {
  if (str.toLowerCase() !== str) {
      return true;
  }
  return false;
  }

  hasNumber(myString) {
    return /\d/.test(myString);
  }

  get repasswordNotValidated(): boolean {
    return this.vs.isNotValidateRequestAndMinLength(this.resetForm, 'repassword');
  }
  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.token = params['token'];
    });
  }

  ngAfterViewInit(): void {
    this.cs.loadBootstrapCSS();
  }

  verifyUsernameToken() {

    if (this.validateForm.valid) {
      this.canSubmit = false;
      this.cs.postObject<ApiResult>(CONFIG.apiURL.password.validateResetToken, { data: this.token, data2: this.username })
      .subscribe((response: ApiResult) => {
        if (response.success) {
          this.verified = true;
        } else {
          this.verified = false;
          this.PushErrorMessage(response.message);
        }
      });
    } else {
     this.PushErrorMessage('Input field is invalid.');
    }

    
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
  resetPassword() {
    this.cs.postObject<ApiResult>(CONFIG.apiURL.password.reset, {
      username: this.username,
      token: this.token,
      password: this.password,
      repassword: this.repassword
    }).subscribe((response: ApiResult) => {
      if (response.success) {
        this.router.navigate(['/signin']);
      } else {
        this.PushErrorMessage(response.message);
      }
    }
    );
  }
}
