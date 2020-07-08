import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CardResponse } from 'src/app/_models/cardResponse';

@Component({
  selector: 'app-cardItem',
  templateUrl: './cardItem.component.html',
  styleUrls: ['./cardItem.component.css']
})
export class CardItemComponent implements OnInit {
  @Output() edit = new EventEmitter();
  @Output() delete = new EventEmitter();
  @Input() card: CardResponse;
  @Input() itIsOwnerCard = false;
  showDeleteForm = false;

  constructor() { }

  ngOnInit() {
  }

  editCard() {
    this.edit.emit(this.card);
  }

  deleteCard(response: boolean) {
    if (response) {

    }
    this.showDeleteForm = false;
  }
}
