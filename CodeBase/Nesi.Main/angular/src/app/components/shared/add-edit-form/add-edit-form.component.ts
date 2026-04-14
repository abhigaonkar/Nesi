import { Component, Input, AfterViewInit, OnInit, Output, EventEmitter, ViewChild, ElementRef, NgModule } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { DataService } from './../../nesi-datatable/service/dataservice';
import { AddEditFormService } from './add-edit-form.service';
import { TreeNode } from 'primeng/primeng';
import { CONFIG } from 'app/configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromMessage from '../../../actions/layout/growlMessage';
import { validateSpaces, validateCharacters } from '../custom-validators/custom.validator';

@Component({
  selector: 'app-add-edit-form',
  templateUrl: './add-edit-form.component.html',
  providers: [AddEditFormService],
})

export class AddEditFormComponent implements OnInit {
  @Input() rowData: any;
  @Input() queryParam: any;

  @Input() columnArray: ColumnArray[];
  @Input() genericApiObject: any;
  @Input() actionType: string;
  @Input('gridName') gridName: string;
  @Output() emitRecord = new EventEmitter();
  @Output() closeDialog = new EventEmitter();
  public addEditForm: FormGroup;
  public editableFields: ColumnArray[];
  public lookupOptions: any = {};
  public lookupPageCount: any;
  public distinctLoader: boolean = false;
  public isProcessing: boolean = false;
  public selectedAutocompleteOption: any[] = [];
  public previousValueDependentOn: any = {}

  constructor(public _fb: FormBuilder,
    public addEditFormService: AddEditFormService,
    public dataService: DataService,
    protected store: Store<fromRoot.State>,
    public datePipe: DatePipe
  ) {
    this.addEditForm = this._fb.group({});
  }

  ngOnInit(): void {
    if (this.columnArray) {
      if (this.fileManagerToBeLoaded()) {
        this.getProjectFileDirectory();
      }
      this.createForm();
    }
  }

  public projectFileDirectory: Array<TreeNode>;
  public defaultCustomerAssetFolderName: string;
  public getProjectFileDirectory() {
    let rowData = this.rowData;
    if (rowData) {
      this.addEditFormService.getList<TreeNode>(CONFIG.apiURL.core.fileManager.customerAsset + rowData.id)
        .subscribe(
          (res: TreeNode[]) => {
            this.projectFileDirectory = res;
          }
        );
    }
  }

  /**
   * Saves record to database
   * @param  {} newRecord
   * @returns void
   */
  saveRecord(newRecord): void {
    for (let i = 0; i < this.columnArray.length; i++) {
      if (this.columnArray[i]['type'] && this.columnArray[i]['type'] == 'boolean') {
        if (newRecord[this.columnArray[i]['field']] == '') {
          newRecord[this.columnArray[i]['field']] = false;
        }
      } else if (this.columnArray[i]['type'] && this.columnArray[i]['type'] == 'date') {
        newRecord[this.columnArray[i]['field']] = this.datePipe.transform(newRecord[this.columnArray[i]['field']], 'yyyy-MM-dd');
      }
    }

    if (this.queryParam) {
      this.queryParam.forEach(x => {
        newRecord[x.coulumnname] = x.value;
      });
    }
    const url = this.genericApiObject[this.actionType];
    this.isProcessing = true;

    this.addEditFormService.saveRecord(newRecord, url, this.actionType).subscribe((data) => {
      const emitObject = {
        fireSaveEvent: false,
        record: newRecord,
        action: this.actionType
      };
      this.closeDialog.emit();
      if (data && data.json()) {
        const res = data.json().data;
        if (res) {
          if (String(res).toLocaleLowerCase().indexOf('success') > -1) {
            this.store.dispatch(new fromMessage.PushSuccessMessage(res));
            emitObject.fireSaveEvent = true;
            this.emitRecord.emit(emitObject);
          } else {
            this.store.dispatch(new fromMessage.PushWarnMessage(res));
          }
        }
        else {
          this.emitRecord.emit(emitObject);
          this.store.dispatch(new fromMessage.PushSuccessMessage('Record has been successfully saved.'));
        }
      } else {
        this.emitRecord.emit(emitObject);
        this.store.dispatch(new fromMessage.PushSuccessMessage('Record has been successfully saved.'));
      }
      this.isProcessing = false;
    },
      (err) => {
        this.isProcessing = false;
        throw (err);
      }
    )
  }

  /**
   * close Form
   * @returns void
   */
  cancel(): void {
    this.closeDialog.emit();
  }

  createDefaultOption(columns, rowData) {
    if (this.actionType == 'edit' || this.actionType == 'create') {
      columns.forEach((element, index) => {
        if (element.lookup != null) {
          let value = this.rowData[element.lookup.values[0].value];
          this.selectedAutocompleteOption[index] = {
            'label': value,
            'value': this.rowData[element.field]
          }
          let option = [];
          let obj = {};
          obj['label'] = this.selectedAutocompleteOption[index]['label']
          obj['value'] = this.selectedAutocompleteOption[index]['value']
          option.push(obj);
          this.lookupOptions[element.field] = { 'endpoint': element.lookup.endpoint, options: option };
        }
      });
    }
  }

  createForm() {
    let fieldsObject: any = {};
    this.editableFields = this.columnArray.filter(data => { return data.editable === true; });
    this.editableFields.sort(function (a, b) {
      return a['order'] - b['order'];
    });
    this.createDefaultOption(this.editableFields, this.rowData);
    this.editableFields.map(element => {
      this.addEditForm.addControl(element.field, new FormControl('', this.getValidations(element)));
    });
  }

  getValidations(element): Validators {
    let validationsArray = [];
    if (element.validations) { // Validations are coming from API
      const currentItemValidations = JSON.parse(element.validations);
      for (let key in currentItemValidations) {
        if (key == 'required') {
          validationsArray.push(Validators.required);
          validationsArray.push(validateSpaces);
        } else if (key == 'maxLength') {
          validationsArray.push(Validators.maxLength(currentItemValidations[key]));
        }
      }
    }

    if (element.lookup != null) { // Look-up requires minimum 2 characters to work
      validationsArray.push(Validators.minLength(2));
    }

    if (element.field && this.gridName == 'CustomerAsset' && element.field == 'model') {
      validationsArray.push(validateCharacters(['"'])); // Allow " character and restric others
    } else if (element.type == 'string' || element.type == 'notes' || element.type == 'number') {
      validationsArray.push(validateCharacters([]));
    }
    return validationsArray;
  }

  /**
   * Returns paired column of current column. Eg: For 'Customer' column in Customer Assets report this function returns 'Location' column
   * @param  {} currentField
   */
  getComboLookupColumns(currentField) {
    const pairedColumn = this.columnArray.filter((col) => {
      let matchValue = '';
      if (col.lookup && col.lookup.inputs) {
        matchValue = col.lookup.inputs[0].value;
      }
      return matchValue == currentField;
    });
    return pairedColumn;
  }

  loadOptions(event, column) {
    const comboLookupColumns = this.getComboLookupColumns(column.field);
    comboLookupColumns.length ? this.lookupOptions[comboLookupColumns[0].field] = [] : null;
    let targetControl = event.originalEvent.target;
    let haveLookup = false;
    let postObject = {
      globalfilter: event.query
    };
    if (column.lookup != null && column.lookup.inputs == null) {
      let dataURL = column.lookup.endpoint;
      this.isProcessing = true;
      this.addEditFormService.getLookupData(dataURL, postObject, column, (data) => {
        let tempObj = {};
        tempObj = {
          endPoint: column.lookup.endpoint,
          options: data
        }
        if (this.actionType === 'edit') {
          this.lookupOptions[column.field].options = [];
        }
        this.lookupOptions[column.field] = tempObj;
        targetControl.focus();

        // Enable save button only if options are found
        if (data.length) {
          this.isProcessing = false;
        }
      });
    }

  }

  getFieldType(value) {
    if (value.toLowerCase().indexOf('string') != -1) {
      return 'string';
    } else if (value.toLowerCase().indexOf('boolean') != -1) {
      return 'boolean';
    } else if ((value.toLowerCase().indexOf('datetime') != -1) || (value.toLowerCase().indexOf('date') != -1)) {
      return 'date';
    } else if (value.toLowerCase().indexOf('phone') != -1) {
      return 'phone';
    } else if (value.toLowerCase().indexOf('currency') != -1) {
      return 'currency';
    } else if (value.toLowerCase().indexOf('notes') != -1) {
      return 'notes';
    } else if (value.toLowerCase().indexOf('email') != -1) {
      return 'email';
    } else if (value.toLowerCase().indexOf('percent') != -1) {
      return 'percent';
    } else {
      return 'number';
    }
  }

  onSelect(event, itemKey) {
    let postObject = {};
    this.rowData[itemKey.lookup.values[0].value] = event.label;
    this.rowData[itemKey.field] = event.value;
    // Handling only for 1 value in inputs array of lookup
    for (let i = 0; i < this.columnArray.length; i++) {
      if (this.columnArray[i].lookup && this.columnArray[i].lookup.inputs && this.columnArray[i].lookup.inputs[0].value == itemKey.field) {
        this.lookupOptions[this.columnArray[i].field] = [];
        let filterURL = ',';
        this.columnArray[i].lookup.inputs.forEach((element, index, arr) => {
          filterURL = filterURL + element['key'] + '||' + this.rowData[element['value']] + (index == arr.length - 1 ? '' : '^');
        });

        let dataURL = this.columnArray[i].lookup.endpoint + filterURL;
        postObject['pagesize'] = 500000;
        postObject['pagecount'] = 1;
        this.isProcessing = true;
        this.addEditFormService.getLookupData(dataURL, postObject, this.columnArray[i], (data) => {
          let tempObj = {};
          tempObj = {
            endPoint: this.columnArray[i].lookup.endpoint + filterURL,
            options: data
          }
          // this.lookupOptions[this.columnArray[i].field] = {};
          this.editableFields.forEach((element, index) => {
            if (element.field == this.columnArray[i].field) {
              this.selectedAutocompleteOption[index] = {};
            }
          })
          this.lookupOptions[this.columnArray[i].field] = tempObj;
          this.rowData[this.columnArray[i].field] = this.lookupOptions[this.columnArray[i].field]['options'][0]['value'];
          this.rowData[this.columnArray[i].lookup.values[0].value] = this.lookupOptions[this.columnArray[i].field]['options'][0]['label'];
          this.isProcessing = false;
        })
      }
    }
  }

  setDropdown(event, itemKey) {
    this.rowData[itemKey.lookup.values[0].value] = event.originalEvent.target.innerText;
    this.rowData[itemKey.field] = event.value;
  }

  convertStringToLowerCase(str) {
    return str ? str.toString().toLowerCase() : str;
  }

  changeValue(value, itemKey) {
    this.rowData[itemKey] = !value;
  }

  isFieldInvalidAndDirty(field): boolean {
    const control = this.addEditForm.controls[field];
    return control.invalid && control.dirty;
  }

  fileManagerToBeLoaded() {
    return (this.actionType === 'edit' && this.gridName === 'CustomerAsset');
  }

  createOptionsForDropdown($event, column) {
    const currentField = column.field;
    const newDependeeValue = this.rowData[column.lookup.inputs[0].value];
    if (this.actionType === 'edit' || this.actionType === 'create') {
      let postObject = {};
      let filterURL = null;
      postObject['pagesize'] = 500000;
      postObject['pagecount'] = 1;
      if (this.actionType === 'edit' && newDependeeValue !== this.previousValueDependentOn[currentField]) {
        this.previousValueDependentOn[currentField] = newDependeeValue;
        filterURL = ',';
        column.lookup.inputs.forEach((element, index, arr) => {
          filterURL = filterURL + element['key'] + '||' + newDependeeValue + (index == arr.length - 1 ? '' : '^');
        });
      }
      let dataURL = column.lookup.endpoint;
      if (filterURL && dataURL.indexOf('?') > -1) {
        dataURL += filterURL;
      }
      this.isProcessing = true;
      this.addEditFormService.getLookupData(dataURL, postObject, column, (data) => {
        let defaultOption = this.lookupOptions[column.field].options[0];
        let defaultIndex;
        let garbage = data.filter((element, index) => {
          if (element.value === defaultOption.value) {
            defaultIndex = index;
            return element
          }
        })
        defaultOption = data.splice(defaultIndex, 1);
        data.unshift(defaultOption[0]);
        let tempObj = {};
        tempObj = {
          endPoint: column.lookup.endpoint + filterURL,
          options: data
        }
        this.editableFields.forEach((element, index) => {
          if (element.field == column.field) {
            this.selectedAutocompleteOption[index] = {};
          }
        })
        this.lookupOptions[column.field] = [];
        this.lookupOptions[column.field] = tempObj;
        this.isProcessing = false;
      })
    }
  }

}

interface ColumnArray {
  display: boolean;
  editable?: boolean;
  field?: string;
  filter?: boolean;
  filterType?: string;
  first: boolean;
  isKey?: boolean;
  label?: string;
  lookup?: {
    endpoint: string;
    inputs: any;
    key: string;
    values: { key: string, value: string }[]
  };
  sort?: boolean;
  summary?: any;
  type?: string;
  _$visited?: boolean;
  validations: string;
}
