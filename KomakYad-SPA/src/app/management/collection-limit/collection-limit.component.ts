import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-collection-limit',
  templateUrl: './collection-limit.component.html',
  styleUrls: ['./collection-limit.component.scss']
})
export class CollectionLimitComponent implements OnInit {
  @Input() user: User;
  constructor(private alertify: AlertifyService, private adminService: AdminService) { }

  ngOnInit() {
  }

  setLimit() {
    this.adminService.setCollectionLimit(this.user.id, this.user.collectionLimit).subscribe(response => {
      this.alertify.success('Limitation has set.');
    }, error => {
      this.alertify.error(error);
    });
  }
}
