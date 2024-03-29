import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  registerForm: FormGroup;
  reCaptchaToken: string;
  loading: boolean;

  constructor(private authService: AuthService, private alertify: AlertifyService, private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      agreement: [false, Validators.requiredTrue]
    }, { validators: this.passwordMatchValidator });
  }
  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { mismatch: true };
  }
  register() {
    this.loading = true;
    const registerModel = {
      username: this.registerForm.value.username,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword,
      email: this.registerForm.value.email,
      firstName: this.registerForm.value.firstName,
      lastName: this.registerForm.value.lastName,
      reCaptchaToken: this.reCaptchaToken,

    }
    this.authService.register(registerModel).subscribe(() => {
      this.alertify.success('registeration successfull');
      this.login(registerModel);
      this.loading = false;
    }, error => {
      this.alertify.error(error);
      this.loading = false;
    });
  }

  login(loginModel: any) {
    this.authService.login(loginModel).subscribe(response => {
      this.alertify.success('Logged in successfully');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/dashboard']);
    });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }

}
