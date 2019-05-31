using System;

namespace WebAppCore.BLL.Exceptions
{
    public class PermissionsException : Exception
    {
        public PermissionsException(string message) : base(message)
        {
        }
    }
}