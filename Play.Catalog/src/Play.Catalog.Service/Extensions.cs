using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }

        public static AccountDto AsDto(this Account account)
        {
            return new AccountDto(account.Id, account.Username, account.Password, account.Email, account.PhoneNumber, account.Gender, account.Biography, account.IsPublic , account.Job, account.Biography, account.FollowedAccounts,account.FollowersAccounts);
        }
    }
}