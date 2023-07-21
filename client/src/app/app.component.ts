import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { AuthenticationService } from './services/authentication.service';
import { take } from 'rxjs';
import { User } from './models/user/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'client';

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const userString = localStorage.getItem('user');
    if (!userString) {
      this.router.navigateByUrl('/auth');
      return;
    }

    const user: User = JSON.parse(userString);
    this.authenticationService.setCurrentUser(user);
    this.router.navigateByUrl('/messages');
  }
}
