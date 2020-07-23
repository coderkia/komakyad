import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
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
  @Output() cardMoved = new EventEmitter();
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
      this.overview.totalCount--;
      this.cardMoved.emit(this.readCards[this.cardIndex].currentDeck + 1);
    }
    else if (readResult === ReadResult.Failed) {
      this.overview.downCount++;
      this.overview.dueCount--;
      this.overview.totalCount--;
      switch (this.readCards[this.cardIndex].currentDeck) {
        case 0:
        case 1:
        case 2:
        case 3:
          this.cardMoved.emit(0);
          break;
          case 4:
            this.cardMoved.emit(3);
            break;
          case 5:
            this.cardMoved.emit(4);
            break;
        default:
          break;
      }
    }
    this.nextCard();
  }

}
