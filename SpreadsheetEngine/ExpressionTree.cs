﻿// <copyright file="ExpressionTree.cs" company="Benjamin Michaelis">
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
        private OperatorNode? rootNode;




        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">The string representing the new expression to construct tree from.</param>
        public ExpressionTree(string expression)
        {

            // expression take off from front first.length
            // expression next char == operator node
            // second == next Get symbols
            // repeat
            //OperatorNode = GetSymbols(expression)
            //operatorNode = operatorFactory.CreateOperatorNode()
            //foreach(string symbol in GetSymbols(expression) )
            //{
                
            //}
        }

        public static Node ParseExpression(string expression)
        {
            string? operand = null;
            Node? node = null;
            string symbol;
            while ((symbol = GetNextSymbol(ref expression)) is { })
            { 
                if (operand is null)
                {
                    operand = symbol;
                }
                else
                {
                    OperatorNode operatorNode = OperatorNodeFactory.CreateOperatorNode(symbol);
                    operatorNode.Left = new VariableNode(operand, 0);
                    operatorNode.Right = ParseExpression(expression);
                }
            }
            return node;
        }

        public static bool IsOperator(string item) => "+-/*".Contains(item);
        public static bool IsOperator(char item) => IsOperator(item.ToString());

        public static string GetNextSymbol(ref string expression)
        {
            StringBuilder result = new();
            int charactersProcessed = 0;
            foreach (char symbol in expression)
            {
                if (IsOperator(symbol))
                {
                    if (result.Length == 0)
                    {
                        result.Append(symbol);
                        charactersProcessed++;
                        break;
                    }
                    else break;
                }
                else
                {
                    result.Append(symbol);
                }
                charactersProcessed++;
            }
            expression = expression[charactersProcessed..];
            return result.ToString();
        }


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