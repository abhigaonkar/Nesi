import { CONFIG } from '../../configuration';

export function ToyyyyMMdd(fromdate: any): string {
  const date = ConvertToDate(fromdate);
  const year = date.getFullYear().toString();
  const m = date.getMonth() + 1;
  let smonth = m.toString();
  if (m < 10) { smonth = '0' + m.toString() }
  const d = date.getDate();
  let sdate = d.toString();
  if (d < 10) { sdate = '0' + d.toString() }
  const r = year + '-' + smonth + '-' + sdate;
  return r;
}

export function ToyyyyMMddHHmmss(fromdate: any): string {
  if (!fromdate) {
    return '';
  }
  const date = ConvertToDate(fromdate);
  if (!date) {
    return '';
  }
  const year = date.getFullYear().toString();
  const m = date.getMonth() + 1;
  const hh = date.getHours();
  const min = date.getMinutes();
  const ss = date.getSeconds();

  let smonth = m.toString();
  if (m < 10) { smonth = '0' + m.toString() }
  const d = date.getDate();
  let sdate = d.toString();
  if (d < 10) { sdate = '0' + d.toString() }
  let shh = hh.toString();
  if (hh < 10) { shh = '0' + shh; }
  let smin = min.toString();
  if (min < 10) { smin = '0' + smin; }
  let sss = ss.toString();
  if (ss < 10) { sss = '0' + sss; }
  const r = year + '-' + smonth + '-' + sdate + ' ' + shh + ':' + smin + ':' + sss;
  return r;
}

export function ToDateOnly(d: Date): Date {
  const t = ToyyyyMMdd(d) + ' 00:00:00';

  const arr = t.split(/[- :]/);
  const t2 = new Date(Number(arr[0]), Number(arr[1]) - 1, Number(arr[2]), Number(arr[3]), Number(arr[4]), Number(arr[5]));
  t2.setTime(t2.getTime() + t2.getTimezoneOffset() * 60 * 1000);
  return t2;
}

export function ToDateOnlyS(d: any): Date {
  if (String(d).toLowerCase().indexOf('t') === -1) {
    d = d + 'T00:00:00';
  }
  const t = d.replace('T', ' ');
  const arr = t.split(/[- :]/);
  const t2 = new Date(Number(arr[0]), Number(arr[1]) - 1, Number(arr[2]), Number(arr[3]), Number(arr[4]), Number(arr[5]));
  //  t2.setTime(t2.getTime() + t2.getTimezoneOffset() * 60 * 1000);
  return t2;
}

export function Today(): Date {
  const dt = new Date();
  return new Date(dt.getFullYear(), dt.getMonth(), dt.getDate());
}
export function IsWeekend(d: Date): boolean {
  // CONFIG.LOG(d, 'isweekend');
  return d && d.getDay() === 0 || d.getDay() === 6;
}
export function DateLess(d1: Date, d2: Date) {
  return ToDateOnly(d1) < ToDateOnly(d2);
}

export function DateLessToday(d: any) {
  const newD = new Date(d);
  if (!newD) { return false; }
  return DateLess(newD, new Date());
}

export function DayDiff(d1: Date, d2: Date): number {
  const dd1 = ToDateOnly(d1);
  const dd2 = ToDateOnly(d2);

  return Math.ceil((dd1.getTime() - dd2.getTime()) / (1000 * 60 * 60 * 24));
}

function ConvertToDate(date: any): Date {
  if (!date) { return null };
  try {
    return new Date(date);
  } catch (e) {
    return null;
  }


}

//
// UTC version of day diff by Yanwei Wang
//
export function DayDiff_UTC(d1: Date, d2: Date): number {
  const dd1 = Date.UTC(d1.getFullYear(), d1.getMonth(), d1.getDate());
  const dd2 = Date.UTC(d2.getFullYear(), d2.getMonth(), d2.getDate());
  let d = dd1 - dd2;
  let diff =  Math.ceil( d / (1000 * 60 * 60 * 24));
  return diff;
}