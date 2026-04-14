import { Component, OnInit, Input } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';
import { ToDo } from '../../../models/layout/todo';

@Component({
  selector: 'bar-todoTable',
  templateUrl: './todoTable.component.html',
  styleUrls: ['./todoTable.component.css']
})
export class TodoTableTableComponent implements OnInit {

  @Input()
  todoList: ToDo[];
  @Input()
  showAll = false;
  @Input()
  recentNumber = 10;
  constructor() { }

  ngOnInit() {
  }

  get ToDoList(): ToDo[] {
    return this.showAll ? this.todoList : this.todoList.slice(0, this.recentNumber);
  }

}
