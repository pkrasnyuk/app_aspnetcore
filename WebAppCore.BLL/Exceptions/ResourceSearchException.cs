using System;

namespace WebAppCore.BLL.Exceptions
{
    public class ResourceSearchException : Exception
    {
        public ResourceSearchException(string message) : base(message)
        {
        }
    }
}