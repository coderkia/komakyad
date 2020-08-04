import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'KomakYad-SPA';
  constructor(private authService: AuthService) { }
  ngOnInit() {
    const user = localStorage.getItem('user');
    if (user) {
      this.authService.currentUser = JSON.parse(user);;
      this.authService.loadRoles();
    }
  }
}
