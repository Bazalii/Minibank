using System;

namespace MiniBank.Web.Exceptions;

public class NotAuthorizedException : Exception
{
    public NotAuthorizedException(string message)
        : base(message)
    {
    }
}