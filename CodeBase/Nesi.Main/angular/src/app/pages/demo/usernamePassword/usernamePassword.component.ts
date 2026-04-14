import { Component, OnInit } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormGroup, FormBuilder, Validators, Validator } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-usernamePassword',
  templateUrl: './usernamePassword.component.html',
  styleUrls: ['./usernamePassword.component.css']
})
export class UsernamePasswordComponent extends FormMessageBase implements OnInit {

  public userform: FormGroup;
  public submitted: boolean;

  public currentProfile: any;
  public buProfile: any;
  public CalendedrMinDate: Date;

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.demo.checkpassword);
  }

  createForm() {
    this.userform = this.fb.group({
      'username': ['', [Validators.required, Validators.minLength(4), Validators.pattern(CONFIG.SQL_PATTERN)]],
      'password': ['', [Validators.required, Validators.minLength(4), Validators.pattern(CONFIG.SQL_PATTERN)]],
    }, {
      });

    this.initFormvalue = {
      'username': '',
      'password': '',
    };
  }
  ngOnInit() {
    this.cs.getObject<any>(CONFIG.apiURL.page.demo.getusername)
      .subscribe(
      (response: any) => {
        this.userform.get('username').setValue(response.username);
        this.userform.get('password').setValue(response.password);
      }
      );
  }

}
