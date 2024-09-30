import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PostService } from '../../services/post.service';
import { NavbarComponent } from '../navbar/navbar.component';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-feed',
  standalone: true,
  imports: [NavbarComponent,MatCardModule,CommonModule],
  templateUrl: './feed.component.html',
  styleUrl: './feed.component.scss'
})
export class FeedComponent implements OnInit,OnChanges {
  posts: any[] = [];
  isLiked: boolean = false;
  constructor(private postService: PostService,private toastr : ToastrService){}
  ngOnChanges(changes: SimpleChanges): void {
    throw new Error('Method not implemented.');
  }

  ngOnInit(): void {
    this.postService.getAllPosts().subscribe(res => {
      // this.posts = posts;
      // var result=JSON.parse(`${res}`);
      console.log(res["$values"]);
      this.posts = res["$values"];
    });
  }

  likePost(){
    this.isLiked = !this.isLiked;
    if(this.isLiked){
    this.toastr.success('Post Liked Successfully','Success');
    }else{
      this.toastr.warning('Post Unliked Successfully','Success');
    }
  }
}