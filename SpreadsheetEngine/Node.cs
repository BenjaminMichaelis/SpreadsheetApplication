using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
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

        /// <summary>
        /// Gets or sets property to be implemented and override in Variable nodes.
        /// </summary>
        public abstract string Name { get; set; }
    }
}
