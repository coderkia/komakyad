import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CardService } from 'src/app/_services/card.service';
import { CardResponse } from 'src/app/_models/cardResponse';
import { CardRequest } from 'src/app/_models/cardRequest';

@Component({
  selector: 'app-card-edit',
  templateUrl: './cardEdit.component.html',
  styleUrls: ['./cardEdit.component.css']
})
export class CardEditComponent implements OnInit {
  @Input() card: CardResponse;
  @Input() collectionId: number;
  @Output() done = new EventEmitter();

  cardForm: FormGroup;

  constructor(private alertify: AlertifyService, private route: ActivatedRoute, private formbuilder: FormBuilder,
    private router: Router, private cardService: CardService) { }

  ngOnInit() {
    this.createCardForm();
  }

  createCardForm() {
    this.cardForm = this.formbuilder.group({
      answer: [this.card.answer, [Validators.required, Validators.maxLength(2000)]],
      question: [this.card.question, [Validators.required, Validators.maxLength(2000)]],
      example: [this.card.example, [Validators.maxLength(2000)]]
    });
  }

  save() {
    this.card.answer = this.cardForm.value.answer;
    this.card.question = this.cardForm.value.question;
    this.card.example = this.cardForm.value.example;
    const cardRequest: CardRequest = {
      id: this.card.id,
      answer: this.cardForm.value.answer,
      question: this.cardForm.value.question,
      example: this.cardForm.value.example,
      jsonData: this.card.jsonData,
      collectionId: this.collectionId
    };
    this.cardService.update(cardRequest).subscribe(response => {
      this.alertify.success('The card updated successfuly.');
      this.card.answer = this.cardForm.value.answer;
      this.card.question = this.cardForm.value.question;
      this.card.example = this.cardForm.value.example;
      this.done.emit();
    }, error => {
      this.alertify.error(error);
    });
  }
  cancel() {
    this.done.emit();
    console.log('canceled');
  }
}
