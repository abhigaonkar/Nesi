import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'phoneFormat' })
export class PhoneFormatPipe implements PipeTransform {

  transform(value: string): string {
    // tslint:disable-next-line:max-line-length
    if (value === '  ' || value === '' || value == null) {
      return '';
    } else {
      const value2 = value;
      value = value.replace(/\s/g, '').replace(/\-/g, '').replace(/\(/g, '').replace(/\)/g, '').replace(/\./g, '');
      if (value.length === 10) {
        return ['(', value.slice(0, 3), ')', value.slice(3, 6) + '-' + value.slice(6)].join('');
      } else {
        return value2;
      }
    }
  }
}
