import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { ProfileUpdate } from '../_models/profileUpdate';
import { ChangePassRequest } from '../_models/changePassRequest';
import { ResetPasswordRequest } from '../_models/resetPasswordRequest';

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
          this.currentUser = user.user;
        })
      );
  }
  loadRoles() {
    if (this.loggedIn()) {
      this.roles = this.jwtHelper.decodeToken(localStorage.getItem('token')).role;
    } else {
      this.roles = [];
    }
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

  changePassword(request: ChangePassRequest) {
    return this.http.post(this.baseUrl + 'changePass', request);
  }

  sendConfirmationEmail(email: string) {
    return this.http.post(this.baseUrl + 'getEmailConfirmationToken(' + email + ')', {});
  }

  confirmationEmail(request: { token: string; username: string; }) {
    return this.http.post(this.baseUrl + 'confirmEmail', request);
  }

  restorePass(email: string) {
    return this.http.post(this.baseUrl + 'restorePass', { email });
  }

  resetPass(request: ResetPasswordRequest) {
    return this.http.post(this.baseUrl + 'resetPass', request);
  }
}
