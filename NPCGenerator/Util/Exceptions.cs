using System;

namespace NPCGenerator.Util
{
    public class GenerationException : Exception
    {
        public GenerationException(string message)
            : base( message )
        { }
        public GenerationException(string message, Exception inner)
            : base( message, inner )
        { }
    }

    public class JsonLoadException : Exception
    {
        public JsonLoadException(string message)
            : base( message )
        { }
        public JsonLoadException(string message, Exception inner)
            : base( message, inner )
        { }
    }
}
