// <copyright file="ExpressionTree.cs" company="Benjamin Michaelis">
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
    /// Class for Expression Tree.
    /// </summary>
    public class ExpressionTree
    {
        private Dictionary<string, double> variables = new();
        private OperatorNodeFactory operatorFactory = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">The string representing the new expression to construct tree from.</param>
        public ExpressionTree(string expression)
        {
            foreach(string symbol in GetSymbols(expression) )
            {
                switch (symbol)
                {
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        this.operatorFactory.CreateOperatorNode(symbol);
                        break;
                    default:
                        this.variables.Add(symbol, 0.0);
                        break;
                }

            }
        }

        public static bool IsOperator(string item) => "+-/*".Contains(item);
        public static bool IsOperator(char item) => IsOperator(item.ToString());

        public static IEnumerable<string> GetSymbols(string expression)
        {
            StringBuilder operand = new();
            foreach(char symbol in expression)
            {
                if(IsOperator(symbol))
                {
                    yield return operand.ToString();
                    operand.Clear();
                }
                else
                {
                    operand.Append(symbol);
                }
            }
            yield return operand.ToString();
        }

        /// <summary>
        /// Sets the specified variable within the ExpressionTree variables dictionary.
        /// </summary>
        /// <param name="variableName">Passes in the variable name.</param>
        /// <param name="variableValue">Passes in the variable value.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            if(!this.variables.ContainsKey(variableName))
            {
                throw new Exception("Variable name doesn't exist to set.");
            }

            this.variables[variableName] = variableValue;
        }

        /// <summary>
        /// This method has no parameters but evaluates the expression to a double value.
        /// </summary>
        /// <returns>The evaluation of the expression.</returns>
        public double Evaluate()
        {
            double result = variables.Values.First();
            foreach(int value in variables.Values.Skip(1))
            {
                result = result + value;
            }
            return result;
        }
    }
}
