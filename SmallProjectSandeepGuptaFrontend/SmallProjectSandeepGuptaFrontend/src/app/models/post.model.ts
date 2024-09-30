import { User } from "./user.model";

export class Post {
  postID?: number = 0;
  userID?: number = 0;
  title: string = '';
  description: string = '';
  createdAt?: string = '';
  likesCount?: number = 0;
  user? : User = new User();

}
