import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

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

constructor(private authService: AuthService, private alertify: AlertifyService, private formBuilder: FormBuilder) { }

ngOnInit() {
  this.createRegisterForm();
}

createRegisterForm() {
  this.registerForm = this.formBuilder.group({
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
  }, { validators: this.passwordMatchValidator });
}
passwordMatchValidator(g: FormGroup) {
  return g.get('password').value === g.get('confirmPassword').value ? null : { mismatch: true };
}
register() {
  this.authService.register(this.registerForm.value).subscribe(() => {
    this.alertify.success('registeration successfull');
  }, error => {
    this.alertify.error(error);
  });
}

cancel() {
  this.cancelRegister.emit(false);
}

}
