import { Component, OnInit } from '@angular/core';
import { PostService } from '../../services/post.service';

import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input'; // Add MatInputModule
import { Subject, takeUntil } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
  ], // Add MatInputModule to imports
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'], // Fixed styleUrl to styleUrls
})
export class DashboardComponent implements OnInit {
  posts: any[] = [];
  modal: boolean = false;
  modal2: boolean = false;
  id!: number;
  deleteid!: number | string;
  
  protected _onDestroy = new Subject<void>();

  constructor(private postService: PostService, private router: Router,private toastr : ToastrService) {}

  ngOnInit(): void {
    this.getUserFeed();
  }

  getUserFeed(){
    this.postService
      .getFeed()
      .pipe(takeUntil(this._onDestroy))
      .subscribe({
        next: (res: any) => {
          console.log(res['$values']);
          this.posts = res['$values'];
        }
        
      });
  }

  

  openModal() {
    this.modal = true;
  }

  openModal2(partId : number) {
    this.modal2 = true;
    this.id = partId;
  }

  onSubmit(event: Event): void {
    event.preventDefault();
    this.modal = false;
    const form = event.target as HTMLFormElement;
    const title = (form.elements.namedItem('title') as HTMLInputElement).value;
    const description = (form.elements.namedItem('description') as HTMLInputElement).value;

    this.postService.addPost(title, description).subscribe();
    this.toastr.success('New Post Created SuccessFully!','Success');
    this.getUserFeed();
    setTimeout(() => {
      window.location.reload();
    }, 1500);
    
    
  }

  onUpdate(event: Event, postId: number): void {
    event.preventDefault();
    this.modal2 = false;
    const form = event.target as HTMLFormElement;
    const title = (form.elements.namedItem('title') as HTMLInputElement).value;
    const description = (form.elements.namedItem('description') as HTMLInputElement).value;
    this.id = postId;
    this.postService.updatePost(title, description, this.id).subscribe();
    this.toastr.success('Post Updated SuccessFully!','Success');
    this.getUserFeed();
    setTimeout(() => {
      window.location.reload();
    }, 1500);
  }

  delete(delid : string) {
    this.deleteid = delid;
    this.postService.delete(this.deleteid).subscribe();
    this.toastr.success('Post Deleted SuccessFully!','Success');
    this.getUserFeed();
    setTimeout(() => {
      window.location.reload();
    }, 1500);
  };


}
