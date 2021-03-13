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
    }
}
