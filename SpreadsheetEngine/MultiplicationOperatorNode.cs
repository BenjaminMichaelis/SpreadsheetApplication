// <copyright file="MultiplicationOperatorNode.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

namespace CptS321
{
    /// <summary>
    /// Operator Node for multiplication.
    /// </summary>
    // Static rn, could be instance in a Expression Tree Node.
    public class MultiplicationOperatorNode : OperatorNode
    {
        /// <summary>
        /// Gets character class is associated with.
        /// </summary>
        public static char Operator => '*';

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
        /// <returns>Returns multiplication of left by right.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() * this.Right.Evaluate();
        }
    }
}
