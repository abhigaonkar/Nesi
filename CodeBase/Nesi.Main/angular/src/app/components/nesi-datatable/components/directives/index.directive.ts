import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { Dropdown } from '../primeng-custom-library/dropdown/dropdown';
import { Calendar } from 'primeng/components/calendar/calendar';

@Directive({
  selector: '[index]'
})
export class IndexDirective {

  constructor(public el: ElementRef) {
   }

  @Input('index') index: number;
  @Input('dropdown') dropdown: Dropdown;
  @Input('calendar') Calendar: Calendar;
}