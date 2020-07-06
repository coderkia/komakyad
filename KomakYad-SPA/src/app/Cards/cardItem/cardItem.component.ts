import { Component, OnInit, Input } from '@angular/core';
import { CardResponse } from 'src/app/_models/cardResponse';

@Component({
  selector: 'app-cardItem',
  templateUrl: './cardItem.component.html',
  styleUrls: ['./cardItem.component.css']
})
export class CardItemComponent implements OnInit {

  @Input() card: CardResponse;

  constructor() { }

  ngOnInit() {
  }

}
