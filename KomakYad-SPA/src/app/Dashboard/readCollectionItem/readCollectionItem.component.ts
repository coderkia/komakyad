import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ReadCollectionRequest } from 'src/app/_models/readCollectionRequest';
import { ReadCollectionResponse } from 'src/app/_models/readCollectionResponse';

@Component({
  selector: 'app-read-collection-item',
  templateUrl: './readCollectionItem.component.html',
  styleUrls: ['./readCollectionItem.component.css']
})
export class ReadCollectionItemComponent implements OnInit {
  @Input() readCollection: ReadCollectionResponse;
  @Output() read = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  readThis() {
    console.log('read clicked');
    this.read.emit(this.readCollection);
  }
}
