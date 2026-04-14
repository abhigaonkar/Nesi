import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromMessage from '../../../actions/layout/growlMessage';
import { CustomersService } from 'app/services/pages/customer.services';
import { SelectItem } from 'primeng/primeng';

@Component({
  selector: 'nesi-customerrequestadminNew',
  templateUrl: './customerrequestadminNew.component.html',
  styleUrls: ['./customerrequestadminNew.component.css']
})
export class CustomerRequestAdminNewComponent implements OnInit {

  @Output() close = new EventEmitter();
  @Output() saved = new EventEmitter<void>();
  @Input() tablename: string;

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
    
    // Load industrial types if this is end market segment
    if (this.tablename === 'cus_end_market_segment') {
      this.addIndustrialTypeValidation();
      this.loadIndustrialTypes();
    }
    
    console.log("table Name", this.tablename);
  }

  addIndustrialTypeValidation() {
    this.form.get('industrial_type_id').setValidators([Validators.required]);
    this.form.get('industrial_type_id').updateValueAndValidity();
  }

loadIndustrialTypes() {
  this.custS.getIndustrialTypes().subscribe({
    next: (res) => {
      const data = JSON.parse(res._body);
      console.log('RAW DATA:', data); // See what we're getting
      
      // Backend already filters WHERE is_active = 1
      // So just map directly without filtering
      this.industrialTypes = data.map((item: any) => ({
        label: item.name,
        value: item.id
      }));
      
      console.log('DROPDOWN-ITEMS:', this.industrialTypes);
    },
    error: (err) => {
      console.error('Error loading industrial types:', err);
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

    const payload = {
      EntityName: this.tablename,
      Name: val.name,
      IsActive: val.is_active
    };

    // Add IndustrialTypeId only for end market segment
    if (this.tablename === 'cus_end_market_segment') {
      payload['IndustrialTypeId'] = val.industrial_type_id;
    }

    this.saving = true;

    this.custS.create(payload).subscribe({
      next: (res) => {
        const data = JSON.parse(res._body);
        this.store.dispatch(new fromMessage.PushSuccessMessage(data));
        this.saving = false;
        this.saved.emit();
        this.close.emit();
      },
      error: (err) => {
        this.saving = false;
        console.error(err);
        this.store.dispatch(new fromMessage.PushErrorMessage('Failed to create record'));
      }
    });
  }

  cancel() {
    this.close.emit();
  }
}