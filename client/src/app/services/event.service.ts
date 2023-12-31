import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { User } from '../models/user/user';
import { UserDetails } from '../models/user/userDetails';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;

  constructor(private toastr: ToastrService, private router: Router) {}

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + '/event', {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('UserIsOnline', (username) => {
      this.toastr.info(username + ' connected');
    });

    this.hubConnection.on('UserIsOffline', (username) => {
      this.toastr.info(username + ' disconnected');
    });

    this.hubConnection.on('GetOnlineUsers', (usernames) => {});

    this.hubConnection.on('NewFriendRequest', (requestFrom) => {
      this.toastr.info(requestFrom.userName + ' sent you a friend request');
    });

    this.hubConnection.on('FriendRequestAccepted', (requestFrom) => {
      this.toastr.info(requestFrom.userName + ' accepted your friend request');
    });
  }

  sendFriendRequest(user: UserDetails) {}

  acceptFriendRequest(requestId: number) {}

  stopHubConnection() {
    this.hubConnection?.stop().catch((error) => console.log(error));
  }
}
