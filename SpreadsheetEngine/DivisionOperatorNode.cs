// <copyright file="DivisionOperatorNode.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    /// <summary>
    /// Operator Node for division.
    /// </summary>
    // Static rn, could be instance in a Expression Tree Node.
    public class DivisionOperatorNode : OperatorNode
    {
        /// <summary>
        /// Gets character class is associated with.
        /// </summary>
        public static char Operator => '/';

        /// <summary>
        /// Gets precedence according to http://web.deu.edu.tr/doc/oreily/java/langref/ch04_14.htm.
        /// </summary>
        public static ushort Precedence => 6;

        /// <summary>
        /// Gets the associativity.
        /// </summary>
        public static Associative Associativity => Associative.Left;

        /// <summary>
        /// Recursive evaluation of left tree then right tree.
        /// </summary>
        /// <returns>Returns division answer.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() / this.Right.Evaluate();
        }
    }
}
