using System;

namespace MiniBank.Web.Exceptions
{
    public class ServiceNotRegisteredException : Exception
    {
        public ServiceNotRegisteredException(string message)
            : base(message)
        {
        }
    }
}