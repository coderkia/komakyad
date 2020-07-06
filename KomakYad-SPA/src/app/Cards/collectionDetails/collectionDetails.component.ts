import { Component, OnInit, Input } from '@angular/core';
import { CollectionResponse } from 'src/app/_models/collectionResponse';

@Component({
  selector: 'app-collectionDetails',
  templateUrl: './collectionDetails.component.html',
  styleUrls: ['./collectionDetails.component.css']
})
export class CollectionDetailsComponent implements OnInit {
  @Input() collection: CollectionResponse
  constructor() { }

  ngOnInit() {
  }

}
