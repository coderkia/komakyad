import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CollectionResponse } from 'src/app/_models/collectionResponse';
import { CollectionRequest } from 'src/app/_models/collectionRequest';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CollectionService } from 'src/app/_services/collection.service';
import { AuthService } from 'src/app/_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-collection-create',
  templateUrl: './CollectionCreate.component.html',
  styleUrls: ['./CollectionCreate.component.css']
})
export class CollectionCreateComponent implements OnInit {
  collection: CollectionResponse;
  collectionForm: FormGroup;
  isPrivate: boolean;

  constructor(private alertify: AlertifyService, private formbuilder: FormBuilder,
    private router: Router, private collectionService: CollectionService, private authService: AuthService) { }

  ngOnInit() {
    this.createcollectionForm();
  }
  createcollectionForm() {
    this.collectionForm = this.formbuilder.group({
      title: ['', [Validators.required, Validators.maxLength(450)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      isPrivate: [true, [Validators.required, Validators.maxLength(2000)]]
    });
  }

  save() {
    const collectionRequest: CollectionRequest = {
      authorId: this.authService.currentUser.id,
      description: this.collectionForm.value.description,
      title: this.collectionForm.value.title,
      isPrivate: this.collectionForm.value.isPrivate
    };
    this.collectionService.create(collectionRequest).subscribe(response => {
      this.alertify.success('Collection created.');
      this.router.navigate(['/collections/' + response.body.id + '/card']);
    }, error => {
      this.alertify.error(error);
    });
  }
  cancel() {
    this.router.navigate(['/collections']);
  }
}
