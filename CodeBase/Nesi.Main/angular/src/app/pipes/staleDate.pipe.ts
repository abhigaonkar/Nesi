import { DatePipe, formatDate, } from '@angular/common';
import { Inject, Pipe, PipeTransform, LOCALE_ID, ɵstringify } from '@angular/core';

@Pipe({ name: 'staleDate' })
export class StaleDatePipe implements PipeTransform {
  constructor(@Inject(LOCALE_ID) private locale: string) { }
  transform(value: Date | string | number, format?: string, timezone?: string, locale?: string): string
    | null;
  transform(value: null | undefined, format?: string, timezone?: string, locale?: string): null;
  transform(
    value: Date | string | number | null | undefined, format?: string, timezone?: string,
    locale?: string): string | null;
  transform(
    value: Date | string | number | null | undefined, format = 'mediumDate', timezone?: string,
    locale?: string): string | null {
    if (value == null || value === '' || value !== value || value === '--') {
      return null;
    }
    try {
      let date = new Date();
      let aMonthAgo = new Date(date.setMonth(date.getMonth() - 1));
      if (formatDate(value, 'yyyy-MM-dd', locale || this.locale) < formatDate(aMonthAgo, 'yyyy-MM-dd', locale || this.locale)) {
        return 'Stale';
      }
      return formatDate(value, format, locale || this.locale, timezone);
    } catch (error) {
      throw Error(`InvalidPipeArgument: '${value}' for pipe '${ɵstringify(DatePipe)}'`)
    }
  }
}
