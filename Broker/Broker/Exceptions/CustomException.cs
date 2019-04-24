namespace Broker.Exceptions
{
    using System;

    /// <summary>
    /// Custom Exception placeholder class
    /// </summary>
    internal class CustomException : Exception
    {
        public string ResourceReferenceProperty { get; set; }

        public CustomException() { }

        public CustomException(string message) : base(message) { }

        public CustomException(string message, Exception inner)
           : base(message, inner)
        {
        }
    }
}
