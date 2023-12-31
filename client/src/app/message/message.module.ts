import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MessageRoutingModule } from './message-routing.module';
import { HomeComponent } from './home/home.component';
import { UserListComponent } from './user-list/user-list.component';
import { SharedModule } from '../shared/shared.module';
import { MessageThreadComponent } from './message-thread/message-thread.component';
import { UserDetailsComponent } from './user-details/user-details.component';

@NgModule({
  declarations: [
    HomeComponent,
    UserListComponent,
    MessageThreadComponent,
    UserDetailsComponent,
  ],
  imports: [CommonModule, MessageRoutingModule, SharedModule],
})
export class MessageModule {}
