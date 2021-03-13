using System.Collections.Generic;

namespace CptS321
{
    /// <summary>
    /// Class to create a variable node.
    /// </summary>
    public class VariableNode : Node
    {
        public string Name { get; set; }

        public double Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// Variable Node constructor.
        /// </summary>
        /// <param name="name">name of node.</param>
        /// <param name="value">Value of the node.</param>
        public VariableNode (string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// If the name is contained, search for it and if it exists, return it.
        /// </summary>
        /// <returns>Returns a double if it is found in the dictionary.</returns>
        public override double Evaluate()
        {
            return this.Value;
        }
    }
}
