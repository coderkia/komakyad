import { Component, OnInit, Input } from '@angular/core';
import { ReadCard } from 'src/app/_models/readCard';

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
    this.cardIndex++;
  }

  previousCard() {
    this.cardIndex--;
  }
}
