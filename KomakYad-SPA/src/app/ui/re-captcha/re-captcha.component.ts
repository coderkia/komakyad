import { Component, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-re-captcha',
  templateUrl: './re-captcha.component.html',
  styleUrls: ['./re-captcha.component.css']
})
export class ReCaptchaComponent implements OnInit {
  @ViewChild('recaptcha', { static: true }) recaptchaElement: ElementRef;
  @Output() token = new EventEmitter();
  constructor() { }

  ngOnInit() {
    this.addRecaptchaScript();
  }

  addRecaptchaScript() {
 
    window['grecaptchaCallback'] = () => {
      this.renderReCaptcha();
    }
   
    (function(d, s, id, obj){
      var js, fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) { obj.renderReCaptcha(); return;}
      js = d.createElement(s); js.id = id;
      js.src = "https://www.google.com/recaptcha/api.js?onload=grecaptchaCallback&amp;render=explicit";
      fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'recaptcha-jssdk', this));
   
  }

  renderReCaptcha() {
    window['grecaptcha'].render(this.recaptchaElement.nativeElement, {
      'sitekey' : environment.siteKey,
      'callback': (response) => {
          this.token.emit(response);
      }
    });
  }
}
