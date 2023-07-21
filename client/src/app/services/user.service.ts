import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { QueryParameters } from '../models/helpers/queryParameters';
import { environment } from '../../environments/environment';
import { UserDetails } from '../models/user/userDetails';
import { PaginatedList } from '../models/helpers/paginatedList';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAllUsers(queryParameters?: QueryParameters) {
    if (!queryParameters) {
      queryParameters = new QueryParameters();
    }
    return this.http.get<PaginatedList<UserDetails>>(`${this.baseUrl}/users`, {
      params: queryParameters.getHttpParamsObject(),
    });
  }
}
