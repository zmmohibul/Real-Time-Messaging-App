import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UserDetails } from '../../models/user/userDetails';
import { ActiveTab } from '../home/home.component';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss'],
})
export class UserDetailsComponent {
  @Output() sendFriendRequestClick = new EventEmitter<UserDetails>();
  @Output() acceptFriendRequestClick = new EventEmitter<UserDetails>();

  @Input() user: UserDetails | undefined = undefined;
  @Input() currentTab: ActiveTab | undefined = undefined;

  get ActiveTab() {
    return ActiveTab;
  }

  onSendFriendRequestClick(user: UserDetails) {
    this.sendFriendRequestClick.emit(user);
  }

  onAcceptFriendRequestClick(user: UserDetails) {
    this.acceptFriendRequestClick.emit(user);
  }
}
