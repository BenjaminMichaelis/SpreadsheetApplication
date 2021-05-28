// <copyright file="AdditionOperatorNode.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Operator Node for addition.
    /// </summary>
    // Static rn, could be instance in a Expression Tree Node.
    public class AdditionOperatorNode : OperatorNode
    {
        /// <summary>
        /// Gets character class is associated with.
        /// </summary>
        public static char Operator => '+';

        /// <summary>
        /// Gets precedence according to http://web.deu.edu.tr/doc/oreily/java/langref/ch04_14.htm.
        /// </summary>
        public static ushort Precedence => 7;

        /// <summary>
        /// Gets the associativity.
        /// </summary>
        public static Associative Associativity => Associative.Left;

        /// <summary>
        /// Recursive evaluation of left tree then right tree.
        /// </summary>
        /// <returns>Returns addition of left and right.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() + this.Right.Evaluate();
        }
    }
}
