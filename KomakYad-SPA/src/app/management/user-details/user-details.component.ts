import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { RolesComponent } from '../roles/roles.component';
import { Role } from 'src/app/_models/role';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {
  @Input() user: User;
  roles: Array<Role>;
  constructor(private adminService: AdminService, private alertify: AlertifyService) { }

  ngOnInit() {
    
  }

}
