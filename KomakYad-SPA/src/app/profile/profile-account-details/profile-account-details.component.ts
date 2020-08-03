import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { FormGroup, FormBuilder } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-profile-account-details',
  templateUrl: './profile-account-details.component.html',
  styleUrls: ['./profile-account-details.component.css']
})
export class ProfileAccountDetailsComponent implements OnInit {

  @Input() user: User;
  form: FormGroup;

  constructor(private formbuilder: FormBuilder, private authService: AuthService, private alertify: AlertifyService) { }
  ngOnInit() {
    console.log(this.user);
    this.createForm();
  }
  createForm() {
    this.form = this.formbuilder.group({
      firstName: this.user.firstName,
      lastName: this.user.lastName,
    });
  }
  save() {
    const profile = {
      firstName: this.form.value.firstName,
      lastName: this.form.value.lastName,
    };
    this.authService.updateProfile(this.authService.currentUser.id, profile)
      .subscribe(response => {
        this.alertify.success('Profile updated');
        this.authService.currentUser.firstName = profile.firstName;
        this.authService.currentUser.lastName = profile.lastName;
        this.createForm();
      }, error => {
        this.alertify.error(error);
      });
  }
}
