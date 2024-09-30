import { Post } from "./post.model";
import { User } from "./user.model";

export class Like {
    likeID : number =0;
    postID : number =0;
    userID : number =0;
    likedAt : string = "";
    post : Post = new Post();
    user : User = new User();
}
