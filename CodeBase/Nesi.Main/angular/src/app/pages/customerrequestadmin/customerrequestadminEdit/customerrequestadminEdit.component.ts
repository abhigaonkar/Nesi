import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromMessage from '../../../actions/layout/growlMessage';
import { CustomersService } from 'app/services/pages/customer.services';
import { SimpleChanges } from '@angular/core';
import { SelectItem } from 'primeng/primeng';

@Component({
  selector: 'nesi-customerrequestadminEdit',
  templateUrl: './customerrequestadminEdit.component.html',
  styleUrls: ['./customerrequestadminEdit.component.css']
})
export class CustomerRequestAdminEditComponent implements OnInit {

  @Output() close = new EventEmitter();
  @Output() saved = new EventEmitter<void>();
  @Input() tablename: string;
  @Input() tablenamedisplay: string;
  @Input() customerrequest: any;

  form: FormGroup;
  saving = false;
  industrialTypes: SelectItem[] = [];

  constructor(
    private fb: FormBuilder,
    private custS: CustomersService,
    protected store: Store<fromRoot.State>,
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [0],
      name: ['', [Validators.required, Validators.maxLength(200)]],
      industrial_type_id: [null],
      is_active: [true]
    });
    
    if (this.tablename === 'cus_end_market_segment') {
      this.addIndustrialTypeValidation();
      this.loadIndustrialTypes();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.customerrequest && this.customerrequest) {
      this.form.patchValue({
        id: this.customerrequest.id || '',
        name: this.customerrequest.name || '',
        industrial_type_id: this.customerrequest.industrial_type_id || null,
        is_active: this.customerrequest.is_active ? true : false
      });
    }

    if (changes.tablename) {
      if (this.tablename === 'cus_end_market_segment') {
        this.addIndustrialTypeValidation();
        this.loadIndustrialTypes();
      } else {
        this.form.get('industrial_type_id').clearValidators();
        this.form.get('industrial_type_id').setValue(null);
        this.form.get('industrial_type_id').updateValueAndValidity();
      }
    }
  }

  addIndustrialTypeValidation() {
    this.form.get('industrial_type_id').setValidators([Validators.required]);
    this.form.get('industrial_type_id').updateValueAndValidity();
  }

  loadIndustrialTypes() {
    this.custS.getIndustrialTypes().subscribe({
      next: (res) => {
        const data = JSON.parse(res._body);
        this.industrialTypes = data.map((item: any) => ({
          label: item.name,
          value: item.id
        }));
      },
      error: (err) => {
        this.store.dispatch(new fromMessage.PushErrorMessage('Failed to load industrial types'));
      }
    });
  }

  submit() {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(k => this.form.controls[k].markAsTouched());
      return;
    }

    if (!this.tablename) return;

    const val = this.form.value;

    const payload: any = {
      id: this.customerrequest.id,
      Value: this.tablename,
      Label: this.tablenamedisplay,
      name: val.name,
      is_active: val.is_active
    };

    if (this.tablename === 'cus_end_market_segment' && val.industrial_type_id) {
      payload.industrial_type_id = val.industrial_type_id;
    }

    this.saving = true;

    this.custS.update(this.customerrequest.id, payload).subscribe({
      next: (res) => {
        const data = JSON.parse(res._body);
        this.store.dispatch(new fromMessage.PushSuccessMessage(data));
        this.saving = false;
        this.saved.emit();
        this.close.emit();
      },
      error: (err) => {
        this.saving = false;
        this.store.dispatch(new fromMessage.PushErrorMessage('Failed to update record'));
      }
    });
  }

  cancel() {
    this.close.emit();
  }
}