// <copyright file="CircularReferenceException.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Exception if a cell references itself somewhere.
    /// </summary>
    public class CircularReferenceException : Exception
    {
        /// <summary>
        /// Default error message.
        /// </summary>
        public const string DefaultMessage = "#error: Cell is referencing itself";

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularReferenceException"/> class.
        /// </summary>
        public CircularReferenceException()
            : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularReferenceException"/> class.
        /// </summary>
        /// <param name="message">The message to be passed to the exception.</param>
        public CircularReferenceException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularReferenceException"/> class.
        /// </summary>
        /// <param name="message">The message to be passed to the exception.</param>
        /// <param name="innerException">Inner exception.</param>
        public CircularReferenceException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
