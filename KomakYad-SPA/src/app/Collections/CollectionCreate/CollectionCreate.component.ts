import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CollectionResponse } from 'src/app/_models/collectionResponse';
import { CollectionRequest } from 'src/app/_models/collectionRequest';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CollectionService } from 'src/app/_services/collection.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-collection-create',
  templateUrl: './CollectionCreate.component.html',
  styleUrls: ['./CollectionCreate.component.css']
})
export class CollectionCreateComponent implements OnInit {
  collection: CollectionResponse;
  collectionForm: FormGroup;
  constructor(private alertify: AlertifyService, private route: ActivatedRoute, private formbuilder: FormBuilder,
              private router: Router, private collectionService: CollectionService, private authService: AuthService) { }

  ngOnInit() {
    this.createcollectionForm();
  }
  createcollectionForm() {
    console.log(this.collection);
    this.collectionForm = this.formbuilder.group({
      title: ['', [Validators.required, Validators.maxLength(450)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]]
    });
  }

  save() {
    const collectionRequest: CollectionRequest = {
      authorId : this.authService.currentUser.id,
      description : this.collectionForm.value.description,
      title : this.collectionForm.value.title
    };
    this.collectionService.create(collectionRequest).subscribe(response => {
      this.alertify.success('Collection created.');
      this.router.navigate(['/collections']);
    }, error => {
      this.alertify.error(error);
    });
  }
  cancel() {
    this.router.navigate(['/collections']);
  }
}