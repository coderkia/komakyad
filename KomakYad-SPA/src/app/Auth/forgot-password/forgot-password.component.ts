import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  @Output() cancelled = new EventEmitter();
  form: FormGroup;
  loading: boolean;
  reCaptchaToken: string;
  showSucceedMessage = false;
  constructor(private authService: AuthService, private alertify: AlertifyService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createForm();
  }

  forgotPass() {
    this.loading = true;
    const email = this.form.value.email;
    this.authService.restorePass(email).subscribe(response => {
      this.showSucceedMessage = true;
      this.loading = false;
    }, error => {
      this.alertify.error(error);
      this.loading = false;
    });
  }

  cancel() {
    this.cancelled.emit();
  }

  createForm() {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }
}
