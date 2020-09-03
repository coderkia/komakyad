import { Component, OnInit, Input } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AdminService } from 'src/app/_services/admin.service';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-card-limit',
  templateUrl: './card-limit.component.html',
  styleUrls: ['./card-limit.component.scss']
})
export class CardLimitComponent implements OnInit {

  @Input() user: User;
  constructor(private alertify: AlertifyService, private adminService: AdminService) { }

  ngOnInit() {
  }

  setLimit() {
    this.adminService.setCardLimit(this.user.id, this.user.cardLimit).subscribe(response => {
      this.alertify.success('Limitation has set.');
    }, error => {
      this.alertify.error(error);
    });
  }
}
