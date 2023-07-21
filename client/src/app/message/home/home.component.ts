import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserDetails } from '../../models/user/userDetails';
import { QueryParameters } from '../../models/helpers/queryParameters';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  users: UserDetails[] = [];
  userQueryParameter = new QueryParameters();

  constructor(private userService: UserService) {}

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
}
