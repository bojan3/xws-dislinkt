using System;

namespace Play.Post.Service.Entities
{
    public class Comment
    {
        public Guid AccountId { get; set; }

        public String Text { get; set; }
    }
}