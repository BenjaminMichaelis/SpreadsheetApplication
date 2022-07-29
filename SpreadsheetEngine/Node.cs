// <copyright file="Node.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    /// <summary>
    /// Abstract Node.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// To be implemented and overriden in operator nodes.
        /// </summary>
        /// <returns>will return double of the evaluation result.</returns>
        public abstract double Evaluate();
    }
}
