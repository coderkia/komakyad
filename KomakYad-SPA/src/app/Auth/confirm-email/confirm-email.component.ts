import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {

  sent: boolean;
  email: string;
  errorMessage: string;
  loading = true;
  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    if (this.authService.loggedIn() === false) {
      this.router.navigate(['/home']);
      return;
    }
    this.email = this.authService.currentUser.email;
    this.sendConfirmationEmail();
  }

  sendConfirmationEmail() {
    this.authService.sendConfirmationEmail(this.email).subscribe(response => {
      this.sent = true;
      this.loading = false;
    }, error => {
      this.loading = false;
      this.sent = false;
      this.errorMessage = error;
    });
  }
}
