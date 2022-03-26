using System;

namespace MiniBank.Data.Users
{
    public class UserDbModel
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }
    }
}