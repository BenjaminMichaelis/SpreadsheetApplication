﻿// <copyright file="OperatorNode.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    /// <summary>
    /// Contains OperatorNodes.
    /// </summary>
    public abstract partial class OperatorNode : Node
    {
        ///// <summary>
        ///// Contains potential operator for instance setup.
        ///// </summary>
        // public char Operator { get; set; }

        /// <summary>
        /// Gets or Sets to node on the left side.
        /// </summary>
        public Node? Left { get; set; }

        /// <summary>
        /// Gets or Sets node on the right side.
        /// </summary>
        public Node? Right { get; set; }
    }
}
