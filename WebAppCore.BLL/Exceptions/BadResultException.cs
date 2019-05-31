using System;

namespace WebAppCore.BLL.Exceptions
{
    public class BadResultException : Exception
    {
        public BadResultException(string message) : base(message)
        {
        }
    }
}
