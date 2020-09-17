import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {

  form: FormGroup;
  loading: boolean;
  reCaptchaToken: string;
  constructor(private authService: AuthService, private alertify: AlertifyService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createForm()
  }
  createForm() {
    this.form = this.formBuilder.group({
      email: ['', Validators.required, Validators.email],
    });
  }
}
