import { Component, OnInit, Input } from '@angular/core';
import { SafeUrl, DomSanitizer } from '@angular/platform-browser';
import { CONFIG } from '../../../configuration';

@Component({
  selector: 'nesi-workorder-show-scanned-file',
  templateUrl: './workorder-show-scanned-file.component.html',
  styleUrls: ['./workorder-show-scanned-file.component.css']
})
export class WorkorderShowScannedFileComponent implements OnInit {
  @Input() set url(value: string) {
    if (!value) {
      return;
    }
    if (CONFIG.ISDEV()) {
      value = 'https://sparkops.web.localhost' + value + '#view=fit';
    }
    this.trustedUrl = this.sanitizer.bypassSecurityTrustResourceUrl(value);
  }

  public trustedUrl: SafeUrl;

  constructor(
    private sanitizer: DomSanitizer,

  ) { }

  ngOnInit() {
  }



}
