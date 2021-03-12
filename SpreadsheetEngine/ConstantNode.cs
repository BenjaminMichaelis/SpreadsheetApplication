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
        /// ConstanceNode Constructor.
        /// </summary>
        /// <param name="value">value to be set.</param>
        public ConstantNode(double value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Overrides evaluate in node, returning current value.
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            return Value;
        }
    }
}
