import { Component, OnInit, Input } from '@angular/core';
import { ReadCard } from 'src/app/_models/readCard';
import { ReadResult } from 'src/app/_models/enums/readResult';

@Component({
  selector: 'app-read',
  templateUrl: './read.component.html',
  styleUrls: ['./read.component.css']
})
export class ReadComponent implements OnInit {
  @Input() readCards: Array<ReadCard>;
  cardIndex: number;
  failedCount: number;
  succeedCount: number;
  constructor() {
  }

  ngOnChanges() {
    this.cardIndex = 0;
    this.failedCount = 0;
    this.succeedCount = 0;
  }
  ngOnInit() {
  }

  nextCard() {
    if (this.readCards.length - 1 === this.cardIndex) {
      this.cardIndex = 0;
    }
    else {
      this.cardIndex++;
    }
  }

  previousCard() {
    if (this.cardIndex !== 0) {
      this.cardIndex--;
    }
    else {
      this.cardIndex = this.readCards.length - 1;
    }
  }

  onCardRead(readResult: ReadResult) {
    if (readResult === ReadResult.Succeed) {
      this.succeedCount++;
    }
    else if (readResult === ReadResult.Failed) {
      this.failedCount++;
    }
    this.nextCard();
  }
}
