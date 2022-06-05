using System;
using Play.Common;
using System.Collections.Generic;

namespace Play.Catalog.Service.Entities
{

    public enum Gender
    {
        male,
        female
    }


    public class Account : IEntity
    {
        public Guid Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public String Biography { get; set; }
        public String Job { get; set; }
        public String Education { get; set; }
        public bool IsPublic { get; set; }
        //public DateTime DateOfBirth { get; set; }

        public List<db_Account2Account> FollowedAccounts { get; set; } = new List<db_Account2Account>();
        public List<db_Account2Account> FollowersAccounts { get; set; } = new List<db_Account2Account>();
        public List<db_Account2Account> WaitingForApprove { get; set; } = new List<db_Account2Account>();
    }

}