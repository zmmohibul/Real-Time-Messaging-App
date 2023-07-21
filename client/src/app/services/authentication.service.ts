import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../models/user/user';
import { HttpClient } from '@angular/common/http';
import { LoginModel } from '../models/authentication/loginModel';
import { RegisterModel } from '../models/authentication/registerModel';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(
    private http: HttpClient,
    private presenceService: PresenceService
  ) {}

  login(model: LoginModel) {
    return this.http
      .post<User>(`${this.baseUrl}/authentication/login`, model)
      .pipe(
        map((res: User) => {
          const user = res;
          if (user) {
            this.setCurrentUser(user);
          }
          return user;
        })
      );
  }

  register(model: RegisterModel) {
    return this.http
      .post<User>(`${this.baseUrl}/authentication/register`, model)
      .pipe(
        map((res: User) => {
          const user = res;
          if (user) {
            this.setCurrentUser(user);
          }
          return user;
        })
      );
  }

  logOut() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);

    this.presenceService.stopHubConnection();
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);

    this.presenceService.createHubConnection(user);
  }
}
