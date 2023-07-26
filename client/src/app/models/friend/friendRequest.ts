import { UserDetails } from '../user/userDetails';

export interface FriendRequest {
  id: number;
  requestFrom: UserDetails;
  requestDate: Date;
}
