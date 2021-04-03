using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    public class CircularReferenceException : Exception
    {
        public CircularReferenceException()
            : base("#error: Cell is referencing itself")
        {
        }

        public CircularReferenceException(string? message)
            : base(message)
        {
        }

        // Creates a new Exception.  All derived classes should
        // provide this constructor.
        // Note: the stack trace is not started until the exception
        // is thrown
        //
        public CircularReferenceException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
