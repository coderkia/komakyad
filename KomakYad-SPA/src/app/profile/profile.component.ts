import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user: User;
  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.user = this.authService.currentUser;
  }

}
