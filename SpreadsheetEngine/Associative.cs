﻿// <copyright file="Associative.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    /// <summary>
    /// Contains the enum for partial class OperatorNode.
    /// </summary>
    public abstract partial class OperatorNode
    {
        /// <summary>
        /// Enum to mark associativity.
        /// </summary>
        public enum Associative
        {
            /// <summary>
            /// Right Associative.
            /// </summary>
            Right,

            /// <summary>
            /// Left Associative.
            /// </summary>
            Left,
        }
    }
}
