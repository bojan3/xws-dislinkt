using System;
namespace Play.Catalog.Service.Entities
{
    public class db_Account2Account
    {
        public Guid ID {get;set;} // id-1
        public Guid FollowerID {get; set;} //id - 2
        public Guid FollowedID {get; set;} // id 3
        public bool IsApproved {get;set;} 
    }
}