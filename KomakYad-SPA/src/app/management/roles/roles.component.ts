import { Component, OnInit, Input } from '@angular/core';
import { Role } from 'src/app/_models/role';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent implements OnInit {
  activeRoles: Array<Role>;
  allRoles: Array<Role>;
  randomNumber: number;
  @Input() user: User;
  constructor(private adminService: AdminService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.generateRandomNumber();
    this.adminService.getRoles().subscribe(response => {
      this.allRoles = response.body;
      console.log('all roles', this.allRoles);
    }, error => {
      this.alertify.error(error);
    });

    this.adminService.getUserRoles(this.user.username).subscribe(response => {
      this.activeRoles = response.body;
      console.log('active roles', this.activeRoles);
    }, error => {
      this.alertify.error(error);
    });
  }

  generateRandomNumber() {
    const num = Math.random() * 10000;
    this.randomNumber = Math.floor(num);
  }

  roleChanged(roleName: string, checked: boolean) {
    if (checked) {
      this.adminService.addRole(this.user.username, roleName).subscribe(response => {
        this.alertify.success(roleName + ' Role is added.');
      }, error => {
        this.alertify.error(error);
      });
    } else {
      this.adminService.removeRole(this.user.username, roleName).subscribe(response => {
        this.alertify.success(roleName + ' Role is added.');
      }, error => {
        this.alertify.error(error);
      });
    }
  }
}
