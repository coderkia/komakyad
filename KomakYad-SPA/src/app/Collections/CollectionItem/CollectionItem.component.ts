import { Component, OnInit, Input } from '@angular/core';
import { CollectionResponse } from 'src/app/_models/collectionResponse';
import { AuthService } from 'src/app/_services/auth.service';

@Component({

  selector: 'app-collection-item',
  templateUrl: './CollectionItem.component.html',
  styleUrls: ['./CollectionItem.component.css']
})
export class CollectionItemComponent implements OnInit {
  @Input() model: CollectionResponse;

  constructor(public authService: AuthService) { }


  ngOnInit() {
  }

}
