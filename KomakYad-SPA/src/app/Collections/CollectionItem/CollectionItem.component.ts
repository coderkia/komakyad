import { Component, OnInit, Input } from '@angular/core';
import { CollectionResponse } from 'src/app/_models/collectionResponse';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ReadService } from 'src/app/_services/read.service';
import { CollectionService } from 'src/app/_services/collection.service';

@Component({

  selector: 'app-collection-item',
  templateUrl: './CollectionItem.component.html',
  styleUrls: ['./CollectionItem.component.css']
})
export class CollectionItemComponent implements OnInit {
  @Input() model: CollectionResponse;
  showAddToReads: boolean;
  constructor(public authService: AuthService, private collectionService: CollectionService, private alertifyService: AlertifyService) { }


  ngOnInit() {
  }

  changePolicy(policy: string) {
    this.collectionService.changePolicy(this.model.id, policy).subscribe(response => {
      this.alertifyService.success('Policy successfully changed.');
      this.model.isPrivate = policy === 'private';
    }, error => {
      this.alertifyService.error(error);
    });
  }

  cancelAddReadMode(showAddToReads: boolean) {
    this.showAddToReads = showAddToReads;
  }
}
