import { Component, OnInit, Input } from '@angular/core';
import { CollectionResponse } from 'src/app/_models/collectionResponse';

@Component({
  selector: 'app-CollectionItem',
  templateUrl: './CollectionItem.component.html',
  styleUrls: ['./CollectionItem.component.css']
})
export class CollectionItemComponent implements OnInit {
  @Input() model: CollectionResponse;
  constructor() { }


  ngOnInit() {
  }

}
