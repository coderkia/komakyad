import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { ProfileUpdate } from '../_models/profileUpdate';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  currentUser;
  roles: Array<string>;
  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem('token', user.token);
            localStorage.setItem('user', JSON.stringify(user.user));
          }
          const decodedToken = this.jwtHelper.decodeToken(user.token);
          this.roles = decodedToken.role;
          console.log(this.roles);
          this.currentUser = user.user;
        })
      );
  }
  loadRoles(){
    this.roles = this.jwtHelper.decodeToken(localStorage.getItem('token')).role;
  }
  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  updateProfile(id: any, profile: ProfileUpdate) {
    return this.http.put(environment.apiUrl + 'profile/' + id, profile);
  }
}
