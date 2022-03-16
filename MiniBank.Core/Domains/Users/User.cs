using System;

namespace MiniBank.Core.Domains.Users
{
    public class User
    {
        public Guid Id { get; set; }
        
        public string Login { get; set; }
        
        public string Email { get; set; }
    }
}