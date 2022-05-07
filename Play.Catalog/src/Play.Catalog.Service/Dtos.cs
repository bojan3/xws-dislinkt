using System;
using System.ComponentModel.DataAnnotations;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Dtos
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreateDate);

    public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

    public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

    public record AccountDto(Guid Id, string Username, string Password, string Email, string PhoneNumber, Gender Gender, string Biography);

    public record CreateAccountDto(string Username, string Password, string Email, string PhoneNumber, Gender Gender, string Biography);

    public record UpdatedAccountDto([Required] string Username, string Password, string Email, string PhoneNumber, Gender Gender, string Biography);

}