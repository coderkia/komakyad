import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ReadCollectionResponse } from 'src/app/_models/readCollectionResponse';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ReadService } from 'src/app/_services/read.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-read-collection-details',
  templateUrl: './readCollectionDetails.component.html',
  styleUrls: ['./readCollectionDetails.component.css']
})
export class ReadCollectionDetailsComponent implements OnInit {
  @Input() readCollection: ReadCollectionResponse;

  showDecks = [0, 1, 2, 3, 4, 5];

  decks: any = [
    { deck: 0, class: 'deck-gray', cards: null, overview: null },
    { deck: 1, class: 'deck-orange', cards: null, overview: null },
    { deck: 2, class: 'deck-sky', cards: null, overview: null },
    { deck: 3, class: 'deck-orangered', cards: null, overview: null },
    { deck: 4, class: 'deck-yellow', cards: null, overview: null },
    { deck: 5, class: 'deck-green', cards: null, overview: null },
    { deck: 6, class: 'deck-green', cards: null, overview: null },
  ];
  currentDeck = 0;
  readModeActive: boolean;
  overviewLoaded = false;
  constructor(private alertify: AlertifyService, private readService: ReadService, private authService: AuthService) { }

  ngOnInit() {
    if (!this.overviewLoaded) {
      this.loadOverview();
    }

  }

  startReading(deck: number) {
    if (this.decks[deck].cards !== null) {
      if (this.decks[deck].cards.length < 1) {
        this.alertify.warning('There is no card to read in Deck ' + deck);
        return;
      }
      this.currentDeck = deck;
      this.readModeActive = true;
      return;
    }
    this.readService.getReadCards(deck, this.readCollection.id, this.authService.currentUser.id)
      .subscribe(response => {
        this.currentDeck = deck;
        this.decks[deck].cards = response;
        if (this.decks[deck].cards.length < 1) {
          this.alertify.warning('There is no card to read in Deck ' + deck);
          return;
        }
        this.readModeActive = true;
      }, error => {
        this.alertify.error(error);
      });

  }

  getBacklogCount(deck: number) {
    if (this.decks[deck].overview) {
      return this.decks[deck].overview.totalCount - this.decks[deck].overview.dueCount;
    }
    else {
      return 'Loading...';
    }
  }


  loadOverview() {
    this.showDecks.forEach(i => {
      this.readService.getTodayReadCollectionOverview(this.readCollection.id, this.authService.currentUser.id, this.decks[i].deck)
        .subscribe(response => {
          this.decks[i].overview = response;
          this.overviewLoaded = true;
        }, error => {
          this.alertify.error(error);
        });
    });
  }

  cardMoved(destinationDeck: number) {
    this.decks[destinationDeck].overview.totalCount++;
    if ( this.getBacklogCount(this.currentDeck) > 0) {
      this.decks[this.currentDeck].overview.totalCount--;
    }
  }
}
