/* Created by this command: ng g guard auth */
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService) { }

  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      if (!this.authService.currentUser.emailConfirmed) {
        this.router.navigate(['/confirmEmail']);
        return false;
      }
      return true;
    }

    this.alertify.error('Unauthorized request');
    this.router.navigate(['/home']);
    return false;
  }
}
