import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { User } from '../models/user/user';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();

  constructor(private toastr: ToastrService, private router: Router) {}

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + '/presence', {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('UserIsOnline', (username) => {
      this.toastr.info(username + ' connected');

      this.onlineUsers$.pipe(take(1)).subscribe({
        next: (usernames) => {
          this.onlineUsersSource.next([...usernames, username]);
        },
      });
    });

    this.hubConnection.on('UserIsOffline', (username) => {
      this.toastr.info(username + ' disconnected');

      this.onlineUsers$.pipe(take(1)).subscribe({
        next: (usernames) => {
          this.onlineUsersSource.next(usernames.filter((x) => x !== username));
        },
      });
    });

    this.hubConnection.on('GetOnlineUsers', (usernames) => {
      this.onlineUsersSource.next(usernames);
    });

    this.hubConnection.on('NewFriendRequest', (requestFrom) => {
      this.toastr.info(requestFrom.userName + ' sent you a friend request');
    });
  }

  stopHubConnection() {
    this.hubConnection?.stop().catch((error) => console.log(error));
  }
}
