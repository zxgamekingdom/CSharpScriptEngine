using System;

namespace CSharpScriptEngine.Library
{
    public class EmptyCollectionException : Exception
    {
        public EmptyCollectionException() : base("不能为空集合")
        {
        }

        public EmptyCollectionException(string? message) : base(message)
        {
        }

        public EmptyCollectionException(string? message, Exception? innerException) :
            base(message, innerException)
        {
        }
    }
}
