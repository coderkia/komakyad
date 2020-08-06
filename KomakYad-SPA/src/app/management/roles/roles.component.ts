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
  roles: { id: string, name: string, checked?: boolean }[];
  randomNumber: number;
  @Input() user: User;
  constructor(private adminService: AdminService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.adminService.getRoles().subscribe(response => {
      this.adminService.getUserRoles(this.user.username).subscribe(activeRoles => {
        this.roles = [];
        response.body.forEach(element => {
          let checked = false;
          activeRoles.body.forEach(active => {
            if (active === element.name) {
              checked = true;
            }
          });
          this.roles.push({ id: element.id.toString(), name: element.name, checked });
        });
      }, error => {
        this.alertify.error(error);
      });



    }, error => {
      this.alertify.error(error);
    });
  }


  roleChanged(roleName: string, checked: boolean) {
    if (checked) {
      this.adminService.addRole(this.user.id, roleName).subscribe(response => {
        this.alertify.success(roleName + ' role is added.');
      }, error => {
        this.alertify.error(error);
      });
    } else {
      this.adminService.removeRole(this.user.id, roleName).subscribe(response => {
        this.alertify.warning(roleName + ' role is removed.');
      }, error => {
        this.alertify.error(error);
      });
    }
  }

}
