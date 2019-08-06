using System;

// ReSharper disable UnusedMember.Global

namespace DevOpsTools
{
    /// <inheritdoc />
    public class ClientException : Exception
    {
        public ClientException(string message) : base(message)
        {
        }

        public ClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        //public ClientException()
        //{
        //}
    }
}

