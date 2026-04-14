import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';
import { ToDo } from '../../../models/layout/todo';

@Component({
  selector: 'bar-icontodo',
  templateUrl: './icon.todo.component.html',
  styleUrls: ['./icon.todo.component.css']
})
export class IconTodoComponent implements OnInit {

@Input()
todoList: ToDo[];

  @Output() click = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onClick(event: any) {
    this.click.emit(event);
  }

  get badgeColor() {
    if (this.todoList.length > 20) {
      return 'blue';
    } else if (this.todoList.length > 10) {
      return 'purple';
    }
      return 'orange';

  }
}
