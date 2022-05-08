using System;
using System.Collections.Generic;
using Play.Common;

namespace Play.Post.Service.Entities
{
    public class Post : IEntity
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public String Text { get; set; }

        public String Image { get; set; }

        public String Link { get; set; }

        public int LikeCount { get; set; }

        public int DislikeCount { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}