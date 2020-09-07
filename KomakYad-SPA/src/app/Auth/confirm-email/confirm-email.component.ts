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
  loading: boolean;
  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    if (this.authService.loggedIn() === false) {
      this.router.navigate(['/home']);
    }
    this.email = this.authService.currentUser.email;
  }

  sendConfirmationEmail() {
    this.loading = true;
    this.authService.sendConfirmationEmail(this.email).subscribe(response => {
      this.loading = false;
      this.sent = true;
    }, error => {
      this.alertify.error(error);
    });
  }
}
