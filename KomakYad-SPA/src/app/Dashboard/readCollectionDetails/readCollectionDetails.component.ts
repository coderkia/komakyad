import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ReadCollectionResponse } from 'src/app/_models/readCollectionResponse';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ReadCard } from 'src/app/_models/readCard';
import { ReadService } from 'src/app/_services/read.service';
import { AuthService } from 'src/app/_services/auth.service';
import { map } from 'rxjs/operators';
import { ReadComponent } from '../read/read.component';

@Component({
  selector: 'app-read-collection-details',
  templateUrl: './readCollectionDetails.component.html',
  styleUrls: ['./readCollectionDetails.component.css']
})
export class ReadCollectionDetailsComponent implements OnInit {
  @Input() readCollection: ReadCollectionResponse;
  @ViewChild(ReadComponent) readComponent;

  showDecks = [0, 1, 2, 3, 4, 5];

  decks: any = [
    { deck: 0, class: 'deck-gray', cards: null },
    { deck: 1, class: 'deck-orange', cards: null },
    { deck: 2, class: 'deck-sky', cards: null },
    { deck: 3, class: 'deck-orangered', cards: null },
    { deck: 4, class: 'deck-yellow', cards: null },
    { deck: 5, class: 'deck-green', cards: null },
    { deck: 6, class: 'deck-green', cards: null },
  ];
  currentDeck = 5;
  readModeActive: boolean;
  constructor(private alertify: AlertifyService, private readService: ReadService, private authService: AuthService) { }

  ngOnInit() {
  }

  startReading(deck: number) {
    if (this.decks[deck].cards !== null) {
      this.currentDeck = deck;
      this.readModeActive = true;
      return;
    }
    this.readService.getReadCards(deck, this.readCollection.id, this.authService.currentUser.id)
      .subscribe(response => {
        this.currentDeck = deck;
        this.readModeActive = true;
        this.decks[deck].cards = response;
      }, error => {
        this.alertify.error(error);
      });

  }

}
