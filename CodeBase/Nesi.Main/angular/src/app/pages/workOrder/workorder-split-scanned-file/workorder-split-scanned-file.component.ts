import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'nesi-workorder-split-scanned-file',
  templateUrl: './workorder-split-scanned-file.component.html',
  styleUrls: ['./workorder-split-scanned-file.component.css']
})
export class WorkorderSplitScannedFileComponent implements OnInit {
  public pdf_path: string;

  constructor() { }

  ngOnInit() {
  }

  loaded(event) {
    this.pdf_path = event.pdf_path;
  }
}
