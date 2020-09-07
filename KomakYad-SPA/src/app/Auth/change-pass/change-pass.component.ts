import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ChangePassRequest } from 'src/app/_models/changePassRequest';

@Component({
  selector: 'app-change-pass',
  templateUrl: './change-pass.component.html',
  styleUrls: ['./change-pass.component.scss']
})
export class ChangePassComponent implements OnInit {

  form: FormGroup;
  loading: boolean;
  errors: string;
  failed: boolean;
  constructor(private authService: AuthService, private alertify: AlertifyService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createForm();
  }

  createForm() {
    this.form = this.formBuilder.group({
      currentPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required],
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('newPassword').value === g.get('confirmPassword').value ? null : { mismatch: true };
  }

  changePassword() {
    this.loading = true;
    const request: ChangePassRequest = {
      currentPassword: this.form.value.currentPassword,
      newPassword: this.form.value.newPassword,
      confirmPassword: this.form.value.confirmPassword
    };
    this.authService.changePassword(request).subscribe(response => {
      this.alertify.success('Your password is changed.');
      this.form.reset();
      this.loading = false;
      this.failed = false;
    }, error => {
      this.alertify.error('Failed to change password');
      this.loading = false;
      this.errors = error;
      this.failed = true;
    });
  }
}
