using System;

namespace ECom.Core
{
    public class MissingApiKeyException : Exception
    {
        public MissingApiKeyException(string message) : base(message)
        {

        }
    }
}
