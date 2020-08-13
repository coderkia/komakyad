import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginResponse: any;
  loginForm: FormGroup;
  loading: boolean;

  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  login() {
    this.loading = true;
    const loginModel = {
      username: this.loginForm.value.username,
      password: this.loginForm.value.password,
    };
    this.authService.login(loginModel).subscribe(response => {
      this.alertify.success('Logged in successfully');
      this.loading = false;
    }, error => {
      this.alertify.error(error);
      this.loading = false;
    }, () => {
      this.router.navigate(['/dashboard']);
    });
  }
}
