using System;

namespace MiniBank.Data.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string message)
            : base(message)
        {
        }
    }
}