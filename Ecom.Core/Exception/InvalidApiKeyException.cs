using System;

namespace ECom.Core
{
    public class InvalidApiKeyException : Exception
    {
        public InvalidApiKeyException(string message):base(message)
        {

        }
    }
}
