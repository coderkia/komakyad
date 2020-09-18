import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetPasswordRequest } from 'src/app/_models/resetPasswordRequest';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {

  loading: boolean;
  errors: string;
  failed: boolean;
  form: FormGroup;
  error: string;
  succeed: boolean;
  token: string;
  username: string;
  constructor(private authService: AuthService, private route: ActivatedRoute, private formBuilder: FormBuilder,
    private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.token = params.token;
      this.username = params.username;
    });
    this.createForm();
  }

  resetPass() {
    this.loading = true;
    const request: ResetPasswordRequest = {
      password: this.form.value.password,
      token: this.token,
      username: this.username,
      confirmPassword: this.form.value.confirmPassword
    };
    console.log(request);
    this.authService.resetPass(request).subscribe(res => {
      this.alertify.success('Your password has been reset successfully!');
      this.authService.login({ username: this.username, password: request.password }).subscribe(response => {
        this.alertify.success('Logged in successfully');
        this.loading = false;
      }, error => {
        this.alertify.error(error);
        this.loading = false;
      }, () => {
        this.router.navigate(['/dashboard']);
      });
    }, error => {
      this.alertify.error('Failed to change password');
      this.loading = false;
      this.errors = error;
      this.failed = true;
    });
  }

  createForm() {
    this.form = this.formBuilder.group({
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required],
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { mismatch: true };
  }
}
