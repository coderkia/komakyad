import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css']
})
export class EmailConfirmationComponent implements OnInit {
  error: string;
  succeed: boolean;
  loading = true;
  constructor(private authService: AuthService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const token = params.token;
      const username = params.username;
      this.authService.confirmationEmail({ token, username }).subscribe(response => {
        this.succeed = true;
        this.loading = false;
        localStorage.removeItem('token');
      }, error => {
        this.succeed = false;
        this.loading = false;
        this.error = error;
      });
    });
  }

}
