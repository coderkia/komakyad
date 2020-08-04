import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { Pagination } from 'src/app/_models/filters/pagination';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  users: Array<User>;
  loading: boolean;
  pagination: Pagination;
  selectedUser: User = null;
  constructor(private alertify: AlertifyService, private adminService: AdminService) { }

  ngOnInit() {
    this.loadUsers(1);
  }

  loadUsers(pageNumber: number) {
    this.loading = true;
    this.adminService.getUsers(pageNumber, 10)
      .subscribe(response => {
        this.users = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      }, error => {
        this.alertify.error(error);
        this.loading = false;
      });
  }

  showUserDetials(user: User) {
    this.selectedUser = user;
  }

  hideUserDetials() {
    this.selectedUser = null;
  }
}
