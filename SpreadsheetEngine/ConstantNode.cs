// <copyright file="ConstantNode.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Class for Constant Node.
    /// </summary>
    public class ConstantNode : Node
    {
        private double Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// ConstantNode Constructor.
        /// </summary>
        /// <param name="value">value to be set.</param>
        public ConstantNode(double value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Overrides evaluate in node, returning current value.
        /// </summary>
        /// <returns>Returns the value.</returns>
        public override double Evaluate()
        {
            return this.Value;
        }
    }
}
