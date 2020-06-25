import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { ReadService } from 'src/app/_services/read.service';

@Component({
  selector: 'app-collection-add-reads',
  templateUrl: './CollectionAddToReads.component.html',
  styleUrls: ['./CollectionAddToReads.component.css']
})
export class CollectionAddToReadsComponent implements OnInit {
  @Output() cancelAddToReads = new EventEmitter();
  @Input() collectionId: number;

  form: FormGroup;
  constructor(private alertify: AlertifyService, private formbuilder: FormBuilder,
    private readService: ReadService, private authService: AuthService) { }

  createcollectionForm() {
    this.form = this.formbuilder.group({
      readPerDay: ['', [Validators.required, Validators.max(255), Validators.min(1)]],
      isReversed: [false]
    });
  }

  ngOnInit() {
    this.createcollectionForm();
  }

  save() {
    this.readService.addToReadCollection(this.collectionId, this.authService.currentUser.id,
      this.form.value.isReversed, this.form.value.readPerDay)
      .subscribe(response => {
        this.alertify.success('The collection is successfully added to your read dashboard.');
      }, error => {
        this.alertify.error(error);
      });
  }

  cancel() {
    this.cancelAddToReads.emit(false);
  }
}
