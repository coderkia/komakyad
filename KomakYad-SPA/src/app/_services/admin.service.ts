import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../_models/filters/pagination';
import { CardResponse } from '../_models/cardResponse';
import { User } from '../_models/user';
import { map } from 'rxjs/operators';
import { Role } from '../_models/role';

@Injectable({
  providedIn: 'root'
})
export class AdminService {


  baseUrl = environment.apiUrl + 'admin/';
  constructor(private http: HttpClient) { }

  getUsers(currentPage: number, itemPerPage: number) {
    let params = new HttpParams();

    if (currentPage != null) {
      params = params.append('pageNumber', currentPage.toString());
    }
    if (itemPerPage != null) {
      params = params.append('pageSize', itemPerPage.toString());
    }

    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

    return this.http.get<User[]>(environment.apiUrl + 'users', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getUserRoles(username: string) {
    return this.http.get<string[]>(this.baseUrl + 'user/' + username + '/roles', { observe: 'response' });
  }

  getRoles() {
    return this.http.get<Role[]>(this.baseUrl + 'roles', { observe: 'response' });
  }

  addRole(userId: number, roleName: string) {
    return this.http.post(this.baseUrl + 'user/' + userId + '/addRole(' + roleName + ')', {});
  }

  removeRole(userId: number, roleName: string) {
    return this.http.post(this.baseUrl + 'user/' + userId + '/removeRole(' + roleName + ')', {});
  }

  lockUser(userId: number) {
    return this.http.patch(this.baseUrl + 'lockUser(' + userId + ')', {});
  }
  unlockUser(userId: number) {
    return this.http.patch(this.baseUrl + 'unlockUser(' + userId + ')', {});
  }
}
