import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginResponse: any;
  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  login() {
    // this.http.post('').subscribe(response => {
    //   this.loginResponse = response;
    // }, error =>{
    //   console.log(error);
    // });
  }
}
