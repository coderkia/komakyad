import { Component, OnInit, Input } from '@angular/core';
import { ReadCard } from 'src/app/_models/readCard';
import { ReadResult } from 'src/app/_models/enums/readResult';
import { ReadOverview } from 'src/app/_models/readOverview';

@Component({
  selector: 'app-read',
  templateUrl: './read.component.html',
  styleUrls: ['./read.component.css']
})
export class ReadComponent implements OnInit {
  @Input() readCards: Array<ReadCard>;
  cardIndex: number;
  @Input() overview: ReadOverview;
  constructor() {
  }

  ngOnChanges() {
    this.cardIndex = 0;
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
      this.overview.upCount++;
      this.overview.dueCount--;
    }
    else if (readResult === ReadResult.Failed) {
      this.overview.downCount++;
      this.overview.dueCount--;
    }
    this.nextCard();
  }
}
