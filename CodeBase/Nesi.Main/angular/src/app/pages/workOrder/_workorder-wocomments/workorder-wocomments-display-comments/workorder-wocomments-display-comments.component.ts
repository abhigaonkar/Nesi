import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nesi-workorder-wocomments-display-comments',
  templateUrl: './workorder-wocomments-display-comments.component.html',
  styleUrls: ['./workorder-wocomments-display-comments.component.css']
})
export class WorkorderWocommentsDisplayCommentsComponent implements OnInit {

  @Input() list: any[];

  public height = 200;
  constructor() { }

  ngOnInit() {
  }

}
