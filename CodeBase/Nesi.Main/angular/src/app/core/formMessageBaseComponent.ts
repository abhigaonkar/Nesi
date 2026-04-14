import { OnInit, Output, EventEmitter } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../reducers';
import * as fromMessage from '../actions/layout/growlMessage';
import { PushInfoMessage, PushMessage, PushSuccessMessage } from '../actions/layout/growlMessage';
import { GrowlMessage } from '../models/layout/growlMessage';
import { CONFIG } from '../configuration';
import { FormGroup } from '@angular/forms';
import { MessageBase } from './messageBaseComponent';
import { CoreService } from '../services/shared/core.service';
import { pushResponseMessage } from '../services/helper/MessageHelper';
import { DataExtra } from '../models/core/dataExtra';

export abstract class FormMessageBase extends MessageBase {

  public userform: FormGroup;
  @Output()
  public onChange = new EventEmitter();
  @Output()
  public beforeSubmit = new EventEmitter();
  @Output()
  public afterSubmit = new EventEmitter();

  public submitting = false;

  public postUrl: string;
  public formSubmitAttempt = false;

  public fullPath: string;
  public uploadedFiles: any[] = [];
  public inputErrorMsg = 'Required.';

  public doSearch = false;

  public initFormvalue: any;

  public submitedValue: any;

  public extraData: any;

  public responseMessageAfterPost: string;


  public get uploadURL() {
    return CONFIG.apiURL.host()
      + CONFIG.apiURL.core.fileManager.uploadFiles + encodeURI(this.fullPath);
  }

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
  ) {
    super(store);
  }

  // after file upload fire this method.
  onUploaded(event: any) {
    this.uploadedFiles = [];
    for (const file of event.files) {
      super.LOG(file, ' file onUploaded formmessagebasecomponent');
      this.uploadedFiles.push(file);
    }
    super.LOG(this.uploadedFiles, 'onUploaded formmessagebasecomponent');
  }

  searchSuccess(res: any) { }
  formValidateBefore() { }
  submitSuccess() { }
  submitFailed() { }
  submitBefore() { }
  disableFields() { }
  submitReset() {
    if (this.initFormvalue) {
      this.userform.reset(this.initFormvalue);
    }
    this.formSubmitAttempt = false;
    if (this.userform && this.userform.controls) {
      // tslint:disable-next-line:forin
      for (const prop in this.userform.controls) {
        this.userform.controls[prop].markAsPristine();
        this.userform.controls[prop].markAsUntouched();
      }
      this.userform.markAsPristine();
      this.userform.markAsUntouched();
    }
  }

  submitValidate(): boolean {
    return true;
  }

  getUrl(path: string): string {
    return path;
  }
  abstract createForm();

  setDateFieldNullToEmptyString(controlName: string) {
    if (!this.userform.get(controlName).value) {
      this.userform.get(controlName).setValue('');
    }
  }

  public isFieldHasValue(controlName: string): boolean {
    const control = this.userform.get(controlName);
    if (control && control.value && control.value.length > 0) {
      return true;
    } else {
      return false;
    }
  }

  public isDropdownInValid(controlName: string, errorName: string = null): boolean {
    return this.isFieldInValid(controlName, errorName, 'dropdown');
  }

  public isFieldMinLength(controlName: string, length: number): boolean {
    if (!controlName) { return true; }
    const control = this.userform.get(controlName);
    if (!control) { return true; }
    if (control.disabled) { return false; }
    const value = control.value;
    return value && value.length >= length;
  }

  public isFieldEmpty(controlName: string): boolean {
    if (!controlName) { return true; }
    const control = this.userform.get(controlName);
    if (!control) { return true; }
    if (control.disabled) { return false; }
    const value = control.value;
    return !value;
  }

  public isFieldInValid(controlName: string, errorName: string = null, errorType: string = null): boolean {
    if (!controlName) { return true; }

    const control = this.userform.get(controlName);
    if (!control) { return true; }
    if (control.disabled) { return false; }
    const value = control.value;
    if ((!value || value === 0) && errorType === 'dropdown') { control.setValue(null) }
    if (!errorName) {
      // CONFIG.LOG(control.errors, 'filed is invalid');
      return !(control.valid) && (control.dirty || control.touched || this.formSubmitAttempt)
    } else {
      return control.errors && control.errors[errorName] && (control.dirty || control.touched || this.formSubmitAttempt);
    }
  }

  public getFieldErrorData(controlName: string, errorfieldName: string): string {
    if (!controlName) { return null; }
    const control = this.userform.get(controlName);
    if (!control) { return null; }
    if (!control.errors) { return null; }
    return control.errors[errorfieldName];
  }

  public isCheckField(controlName: string): boolean {
    const control = this.userform.get(controlName);
    if (!control) { return true; }
    return control.dirty || control.touched || this.formSubmitAttempt;
  }

  public formHasError(err: string, controls: string[]) {
    let flag = false;
    controls.forEach(c => flag = flag || this.isCheckField(c));
    return this.formSubmitAttempt && flag && this.userform.hasError(err);
  }

  public selectedItem(options: any[], key: string, value: any): any {
    return options.find(x => x[key] === value);
  }

  public fieldHasError(controlName: string, errorName: string): boolean {
    const control = this.userform.get(controlName);
    if (!control) { return false; }
    return control.hasError(errorName);
  }

  public Init(value: string) {
    this.postUrl = value;
    this.createForm();
    this.submitReset();

  }

  // set a form value from text input value. if text is null then set with nullValue.
  public setFormValueFromTxt(valueControlName: string, txtControlName: string, nullValue: any) {
    if (this.userform.get(txtControlName).value) {
      this.userform.get(valueControlName).setValue(this.userform.get(txtControlName).value);
    } else {
      this.userform.get(valueControlName).setValue(nullValue);
    }
  }

  // if there is a upload controller, add this method into on onBeforeSend event.
  onBefoeUpload(event) {
    this.submitting = true;
    // before upload, we need set http request credential.
    this.cs.SetTokenBeforeUpload(event);
  }

  onSubmit(value: any = null) {
    // validate form before submit
    // if nto valid, then show all error msg
    window.scroll(0, 0);
    this.formValidateBefore();
    // tslint:disable-next-line:forin
    for (const prop in this.userform.controls) {
      this.userform.controls[prop].enable();
    }
    this.submitedValue = this.userform.value;
    super.LOG(this.submitedValue, 'submit value on submit form in formmessagebase');
    if (!this.userform.valid) {
      this.formSubmitAttempt = true;
      for (const prop in this.userform.controls) {
        if (!this.userform.controls[prop].valid) {
          super.LOG(prop, 'field is invalid');
        }
      }
      super.LOG(this.userform.errors, 'form is invalid on submit form in formmessagebase');
      return;
    }
    this.disableFields();
    this.submitBefore();
    this.beforeSubmit.emit(this.submitedValue);
    this.submitting = false;
    if (this.submitValidate()) {
      if (!this.doSearch) {
        this.formPost();
      } else {
        this.formSearch();
      }
    } else {
      this.disableFields();
    }
  }

  formPost() {
    this.submitting = true;
    CONFIG.LOG(this.submitedValue, 'submited value for post to api');
    this.cs.postDataExtra(this.getUrl(this.postUrl), this.submitedValue)
      .subscribe(
        (res: DataExtra) => {
          super.LOG(res, 'post form result data');
          this.extraData = res.extra ? res.extra : null;
          const event = {
            post: this.submitedValue,
            result: this.extraData
          }
          this.responseMessageAfterPost = res.data;
          if (super.PushResponseMessage(this.responseMessageAfterPost)) {
            this.submitReset();
            this.submitSuccess();
            this.disableFields();
            this.onChange.emit(event);
            this.userform.markAsPristine();
          } else {
            this.submitFailed();
            this.disableFields();
          }
          this.submitting = false;
          this.afterSubmit.emit(event);
        },
        (err: any) => {
          const event = {
            post: this.submitedValue,
            result: null
          }
          CONFIG.LOG(err, 'error message');
          if(err.status==0)
          {
              super.PushErrorMessage('HTTP Connection Error. Updating is failed.');
          }
          else
          {
              super.PushErrorMessage(err);
          }
          this.submitFailed();
          this.disableFields();
          this.submitting = false;
          this.afterSubmit.emit(event);
        }
      );
  }

  formSearch() {
    this.submitting = true;
    CONFIG.LOG(this.submitedValue, 'submited value for post to search api');
    this.cs.postObject<any>(this.getUrl(this.postUrl), this.submitedValue)
      .subscribe(
        (res: any) => {
          this.searchSuccess(res);
          this.disableFields();
          this.userform.markAsPristine();
          this.onChange.emit(this.submitedValue);
          this.submitting = false;
          this.afterSubmit.emit(this.submitedValue);
        },
        (err: any) => {
          if(err.status==0)
          {
              super.PushErrorMessage('HTTP Connection Error. Updating is failed.');
          }
          else
          {
              super.PushErrorMessage(err);
          }
          this.submitFailed();
          this.disableFields();
          this.submitting = false;
          this.afterSubmit.emit(this.submitedValue);
        }
      );
  }


  submit_delay(time: number) {
    setTimeout(() => {
      this.onSubmit(this.userform.value);
    }, time);
  }

}
