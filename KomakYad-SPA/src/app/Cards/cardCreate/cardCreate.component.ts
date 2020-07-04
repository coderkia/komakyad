import { Component, OnInit} from '@angular/core';
import { CardRequest } from 'src/app/_models/cardRequest';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CardService } from 'src/app/_services/card.service';
import { AuthService } from 'src/app/_services/auth.service';
import { CollectionResponse } from 'src/app/_models/collectionResponse';

@Component({
  selector: 'app-card-create',
  templateUrl: './cardCreate.component.html',
  styleUrls: ['./cardCreate.component.css']
})
export class CardCreateComponent implements OnInit {
  createdCards: CardRequest[];
  cardForm: FormGroup;
  collection: CollectionResponse;

  constructor(private alertify: AlertifyService, private route: ActivatedRoute, private formbuilder: FormBuilder,
    private router: Router, private cardService: CardService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.collection = data.collection;
    });
    this.createCardForm();
    document.getElementById('question').focus();
  }
  createCardForm() {
    this.cardForm = this.formbuilder.group({
      answer: ['', [Validators.required, Validators.maxLength(2000)]],
      question: ['', [Validators.required, Validators.maxLength(2000)]],
      example: ['', [Validators.maxLength(2000)]]
    });
  }
  save() {
    const card = {
      answer: this.cardForm.value.answer,
      question: this.cardForm.value.answer,
      example: this.cardForm.value.answer,
      collectionId: this.collection.id,
    };
    this.cardService.create(card).subscribe(response => {
      this.alertify.success('Card created successfuly.');
      this.collection.cardsCount++;
      this.cardForm.reset();
      document.getElementById('question').focus();
    }, error => {
      this.alertify.error(error);
    });
  }
  cancel() {
    this.router.navigate(['/collections']);
  }
}
