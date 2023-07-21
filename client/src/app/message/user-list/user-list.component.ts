import { Component, Input } from '@angular/core';
import { UserDetails } from '../../models/user/userDetails';
import { PresenceService } from '../../services/presence.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent {
  @Input() users: UserDetails[] = [];

  constructor(public presenceService: PresenceService) {}
}
