using System;

namespace WebAppCore.BLL.Exceptions
{
    public class ConflictRequestException : Exception
    {
        public ConflictRequestException(string message) : base(message)
        {
        }
    }
}
