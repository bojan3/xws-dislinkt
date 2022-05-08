using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Dtos
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreateDate);

    public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

    public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

    public record AccountDto(Guid Id, string Username, string Password, string Email, string PhoneNumber, Gender Gender, string Biography, bool IsPublic,String Job, String Education, List<db_Account2Account>FollowedAccounts , List<db_Account2Account> FollowersAccounts);
    
    public record CreateAccountDto(string Username, string Password, string Email, string PhoneNumber, Gender Gender, string Biography, bool IsPublic, String Job, String Education);

    public record UpdatedAccountDto([Required] string Username, string Password, string Email, string PhoneNumber, Gender Gender, string Biography, bool IsPublic, String Job, String Education);

    public record AccontPostDto(Guid Id, Guid AccountId, string Text, string Image, string Link, int LikeCount, int DislikeCount, DateTimeOffset CreatedDate);
}