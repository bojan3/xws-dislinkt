using System;

namespace Play.Post.Service.Dtos
{
    public record CreateAccountPostDto(Guid AccountId, string Text, string Image, string Link);


    // e sad ovde je vrv bolje da on vraca komentare kad se klikne na post, a ne da salje komentare sa postom
    public record PostDto(Guid Id, Guid AccountId, string Text, string Image, string Link, int LikeCount, int DislikeCount, DateTimeOffset createdDate);
}