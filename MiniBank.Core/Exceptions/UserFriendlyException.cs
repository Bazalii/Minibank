using System;

namespace MiniBank.Core.Exceptions
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(string message)
            : base(message)
        {
        }
    }
}