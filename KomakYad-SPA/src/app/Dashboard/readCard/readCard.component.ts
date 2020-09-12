import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { ReadCard } from 'src/app/_models/readCard';
import { FormBuilder } from '@angular/forms';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { ReadService } from 'src/app/_services/read.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { ReadResult } from 'src/app/_models/enums/readResult';
import { TextStyle } from 'src/app/_models/textStyle';

@Component({
  selector: 'app-read-card',
  templateUrl: './readCard.component.html',
  styleUrls: ['./readCard.component.css']
})
export class ReadCardComponent implements OnInit {
  @Input() readCard: ReadCard;
  @Output() goNextCard = new EventEmitter();
  cardJsonDataChanged = false;
  @ViewChild('staticTabs', { static: false }) staticTabs: TabsetComponent;

  getreadResultCssClass() {
    if (this.readCard.readResult === ReadResult.Failed) {
      return 'read-result-failed';
    }
    if (this.readCard.readResult === ReadResult.Succeed) {
      return 'read-result-succeed';
    }
    return '';
  }
  constructor(private formBuilder: FormBuilder, private readService: ReadService, private alertify: AlertifyService,
    private authService: AuthService) { }

  ngOnChange() {
  }

  ngOnInit() {
    if (!this.readCard.jsonData) {
      this.readCard.jsonData = {};
    }
    if (!this.readCard.jsonData.textStyles) {
      this.readCard.jsonData.textStyles = [{}, {}, {}];
    }
  }

  onTabSelect(data: TabDirective): void {
  }

  isCardRead() {
    return this.readCard.readResult === ReadResult.Succeed || this.readCard.readResult === ReadResult.Failed;
  }

  setAsSucceed() {
    this.readCard.readResult = ReadResult.Succeed;
    const cardId = this.readCard.id;
    const userId = this.authService.currentUser.id;
    const status = this.readCard.readResult;
    this.staticTabs.tabs[0].active = true;
    this.goNextCard.emit(ReadResult.Succeed);
    this.readService.moveCard(cardId, userId, status)
      .subscribe(() => {
      }, error => {
        this.alertify.error(error);
      });
    if (this.cardJsonDataChanged) {
      console.log('saving json data', this.readCard.jsonData);
      this.readService.saveJsonData(cardId, userId, this.readCard.jsonData)
        .subscribe(() => {
          this.cardJsonDataChanged = false;
        }, error => {
          this.alertify.warning('Unable to save text style.');
        });
    }
  }

  setAsFailed() {
    this.readCard.readResult = ReadResult.Failed;
    const cardId = this.readCard.id;
    const userId = this.authService.currentUser.id;
    const status = this.readCard.readResult;
    this.staticTabs.tabs[0].active = true;
    this.goNextCard.emit(ReadResult.Failed);
    this.readService.moveCard(cardId, userId, status)
      .subscribe(() => {
      }, error => {
        this.alertify.error(error);
      });
    if (this.cardJsonDataChanged) {
      this.readService.saveJsonData(cardId, userId, this.readCard.jsonData)
        .subscribe(() => {
          this.cardJsonDataChanged = false;
        }, error => {
          this.alertify.warning('Unable to save text style.');
        });
    }
  }

  noChange() {
    this.goNextCard.emit(ReadResult.None);
    this.cardJsonDataChanged = false;
  }

  setStyleChanged(){
    this.cardJsonDataChanged = true;
  }
}
