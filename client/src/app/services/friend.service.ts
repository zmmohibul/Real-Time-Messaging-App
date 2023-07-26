import { Injectable } from '@angular/core';
import { BehaviorSubject, map, of, take, tap } from 'rxjs';
import { UserDetails } from '../models/user/userDetails';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Friend } from '../models/friend/friend';
import { FriendRequest } from '../models/friend/friendRequest';
import { AuthenticationService } from './authentication.service';
import { User } from '../models/user/user';

@Injectable({
  providedIn: 'root',
})
export class FriendService {
  baseUrl = environment.apiUrl;
  private friends: Friend[] = [];
  private friendRequests: FriendRequest[] = [];

  private friendsSource = new BehaviorSubject<Friend[]>([]);
  friends$ = this.friendsSource.asObservable();

  constructor(
    private http: HttpClient,
    private authenticationService: AuthenticationService
  ) {}

  getAllFriends() {
    if (this.friends.length) {
      return of(this.friends);
    }

    return this.http.get<Friend[]>(`${this.baseUrl}/friends`).pipe(
      map((friends: Friend[]) => {
        this.friends = [...friends];
        return friends;
      })
    );
  }

  loadFriends() {
    this.http.get<Friend[]>(`${this.baseUrl}/friends`).pipe(
      map((friends: Friend[]) => {
        this.friends = [...friends];
        this.friends$.pipe(take(1)).subscribe({
          next: (frns) => {
            this.friendsSource.next([...friends]);
          },
        });
        return friends;
      })
    );
  }

  getAllFriendRequests() {
    if (this.friendRequests.length) {
      return of(this.friendRequests);
    }

    return this.http
      .get<FriendRequest[]>(`${this.baseUrl}/friendRequests`)
      .pipe(
        map((friendRequests: FriendRequest[]) => {
          this.friendRequests = [...friendRequests];
          return friendRequests;
        })
      );
  }

  sendFriendRequest(user: UserDetails) {
    return this.http.post(
      `${this.baseUrl}/friendRequests?requestToUserId=${user.id}`,
      {}
    );
  }

  acceptFriendRequest(user: UserDetails) {
    let requestId = 0;
    for (let fr of this.friendRequests) {
      if (fr.requestFrom.id == user.id) {
        requestId = fr.id;
      }
    }

    return this.http
      .post<Friend>(`${this.baseUrl}/friendRequests/accept/${requestId}`, {})
      .pipe(
        tap((fr) => {
          this.friendRequests = this.friendRequests.filter(
            (fr) => fr.id != requestId
          );
          this.friends = [fr, ...this.friends];
        })
      );
  }
}
