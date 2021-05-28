// <copyright file="VariableNode.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Class to create a variable node.
    /// </summary>
    public class VariableNode : Node
    {
        /// <summary>
        /// Gets or sets name of variable node.
        /// </summary>
        public string Name { get; set; }

        private Dictionary<string, double> vars;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="name">name of node.</param>
        /// <param name="vars">variables for dictionary.</param>
        public VariableNode(string name, ref Dictionary<string, double> vars)
        {
            this.Name = name;
            this.vars = vars;
        }

        /// <summary>
        /// If the name is contained, search for it and if it exists, return it.
        /// </summary>
        /// <returns>Returns a double if it is found in the dictionary.</returns>
        public override double Evaluate()
        {
            double value = 0.0;
            if (this.vars.ContainsKey(this.Name))
            {
                value = this.vars[this.Name];
            }

            return value;
        }
    }
}
