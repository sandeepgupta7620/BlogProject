import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Post } from '../models/post.model';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  baseUrl = `${environment.backendUrl}/api/posts`;
  
  constructor(private http: HttpClient) {}

  // Function to create headers with JWT token
  private createHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');  // Retrieve the JWT token from localStorage
    let headers = new HttpHeaders();
    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }
    return headers;
  }

  
  getFeed(): Observable<Post> {
    const data = {
      UserID: localStorage.getItem('UserID')
    }
    const headers = this.createHeaders();
    return this.http.post<Post>(`${this.baseUrl}/feedByUserId`, data , { headers });
  }

  getAllPosts(): Observable<any> {
    const headers = this.createHeaders();
    return this.http.get<any>(this.baseUrl, { headers });
  }

  
  addPost(title: string, description: string): Observable<any> {
    const data = {
      userId: localStorage.getItem("UserID"),
      title: title,
      description: description
    }
    const headers = this.createHeaders();
    return this.http.post<any>(`${this.baseUrl}`, data, {headers});
    
  }

  updatePost(title: string, description: string, postId: number): Observable<any> {
    const data = {
      postId: postId,
      userId: localStorage.getItem("UserID"),
      title: title,
      description: description
    }
    const headers = this.createHeaders();
    return this.http.put<any>(`${this.baseUrl}/${postId}`, data, { headers });
    
  }

  delete(postId: number | string): Observable<any> {
    
    return this.http.delete<any>(`${this.baseUrl}/${postId}`);
  }

  likePost(postId: number): Observable<any> {
    const userId = localStorage.getItem('UserID');
    const headers = this.createHeaders();
    return this.http.post<any>(`${this.baseUrl}/${postId}/like`, { userId }, { headers });
  }
}
