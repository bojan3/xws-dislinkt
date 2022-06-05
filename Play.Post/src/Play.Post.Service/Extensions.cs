using Play.Post.Service.Dtos;
using Play.Post.Service.Entities;

namespace Play.Post.Service
{
    public static class Extensions
    {
        public static PostDto AsDto(this Play.Post.Service.Entities.Post post)
        {
            return new PostDto(post.Id, post.AccountId, post.Text, post.Image, post.Link, post.LikeCount, post.DislikeCount, post.CreatedDate);
        }

        
        public static CommentDto AsDto(this Comment comment)
        {
            return new CommentDto(comment.AccountId, comment.Text);
        }
    }
}