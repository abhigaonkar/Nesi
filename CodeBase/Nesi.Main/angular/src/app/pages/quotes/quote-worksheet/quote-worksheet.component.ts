import { Component, OnInit, Input } from '@angular/core';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { ActivatedRoute } from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-worksheet',
  templateUrl: './quote-worksheet.component.html',
  styleUrls: ['./quote-worksheet.component.css']
})
export class QuoteWorksheetComponent implements OnInit {
  @Input() edit_disabled = false;

  constructor(

  ) { }

  ngOnInit() {

  }


}
