using System;
using System.Runtime.Serialization;

namespace Shimakaze.Toolkit.Csf
{
    [Serializable]
    public class FileNotOpenException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public FileNotOpenException()
        {
        }

        public FileNotOpenException(string message) : base(message)
        {
        }

        public FileNotOpenException(string message, Exception inner) : base(message, inner)
        {
        }

        protected FileNotOpenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}