import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, Router } from '@angular/router';
import { NavComponent } from './components/shared/nav.component';

@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterOutlet, NavComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'frontend';

  constructor(public router: Router) {}

  showNavigation(): boolean {
    return !this.router.url.includes('/login');
  }
}
