import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CollectionResponse } from 'src/app/_models/collectionResponse';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CollectionService } from 'src/app/_services/collection.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-collectionedit',
  templateUrl: './CollectionEdit.component.html',
  styleUrls: ['./CollectionEdit.component.css']
})
export class CollectionEditComponent implements OnInit {
  collection: CollectionResponse;
  collectionForm: FormGroup;
  constructor(private alertify: AlertifyService, private route: ActivatedRoute, private formbuilder: FormBuilder,
              private router: Router, private collectionService: CollectionService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.collection = data.collection;
    });
    this.createcollectionForm();
  }
  createcollectionForm() {
    console.log(this.collection);
    this.collectionForm = this.formbuilder.group({
      title: [this.collection.title, [Validators.required, Validators.maxLength(450)]],
      description: [this.collection.description, [Validators.required, Validators.maxLength(2000)]]
    });
  }

  save() {
    this.collectionService.update(this.collection.id, this.collectionForm.value).subscribe(response => {
      this.alertify.success('Collection updated.');
      this.router.navigate(['/collections']);
    }, error => {
      this.alertify.error(error);
    });
  }
  cancel() {
    this.router.navigate(['/collections']);
  }
}
