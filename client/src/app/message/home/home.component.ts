import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserDetails } from '../../models/user/userDetails';
import { QueryParameters } from '../../models/helpers/queryParameters';
import { FriendService } from '../../services/friend.service';
import { Friend } from '../../models/friend/friend';
import { ToastrService } from 'ngx-toastr';

export enum ActiveTab {
  Messages = 'Messages',
  FriendRequests = 'FriendRequests',
  FindFriend = 'FindFriend',
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  users: UserDetails[] = [];
  userQueryParameter = new QueryParameters();
  activeTab = ActiveTab.Messages;
  userInView: UserDetails | undefined = undefined;

  constructor(
    private userService: UserService,
    private friendService: FriendService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadFriends();
    console.log(this.users);
  }

  userSearchInput(event: Event) {
    let searchTerm = (event.target as HTMLInputElement).value;
    if (!searchTerm) {
      this.users = [];
      return;
    }

    console.log(searchTerm);
    this.userQueryParameter.searchTerm = searchTerm;

    this.userService.getAllUsers(this.userQueryParameter).subscribe({
      next: (list) => {
        this.users = list.items;
        console.log(this.users);
      },
    });
  }

  loadFriends() {
    this.friendService.getAllFriends().subscribe({
      next: (friends) => {
        this.users = friends.map((friend) => friend.friend);
      },
    });
  }

  loadFriendRequests() {
    this.friendService.getAllFriendRequests().subscribe({
      next: (friendRequests) => {
        this.users = friendRequests.map((fr) => fr.requestFrom);
      },
    });
  }

  tabButtonGroupClick(tab: ActiveTab) {
    this.activeTab = tab;
    this.users = [];
    this.userInView = undefined;

    if (this.activeTab === ActiveTab.Messages) {
      this.loadFriends();
    }

    if (this.activeTab === ActiveTab.FriendRequests) {
      this.loadFriendRequests();
    }
  }

  onUserClick(user: UserDetails) {
    this.userInView = user;
  }

  onSendFriendRequestClick(user: UserDetails) {
    this.friendService.sendFriendRequest(user).subscribe({
      next: () => {
        this.toastr.info(`Request sent to user`);
      },
      error: (err) => {
        this.toastr.error(`${err.error.errorMessage}`);
        console.log(err);
      },
    });
  }

  onAcceptFriendRequestClick(user: UserDetails) {}

  get ActiveTab() {
    return ActiveTab;
  }
}
