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
        private OperatorNode? rootNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">The string representing the new expression to construct tree from.</param>
        public ExpressionTree(string expression)
        {
            this.rootNode = this.ParseNodes(expression);
        }

        /// <summary>
        /// Check that the expression has matching Parenthesis.
        /// </summary>
        /// <param name="expression">Expression to check.</param>
        /// <returns>True if expression has matching parenthesis, false if not.</returns>
        public static bool MatchingParenthesis(string expression)
        {
            Dictionary<char, char> bracketPairs = new Dictionary<char, char>()
            {
                { '[', ']' },
                { '{', '}' },
                { '(', ')' },
            };

            Stack<char> matchingBrackets = new Stack<char>();

            try
            {
                foreach (char currentChar in expression)
                {
                    if (bracketPairs.Keys.Contains(currentChar))
                    {
                        matchingBrackets.Push(currentChar);
                    }
                    else
                        if (bracketPairs.Values.Contains(currentChar))
                        {
                            if (currentChar == bracketPairs[matchingBrackets.First()])
                            {
                                matchingBrackets.Pop();
                            }
                            else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch
            {
                // Exception will be caught in case a closing bracket is found first, prior to opening brackets. This implies that the string is not balanced.
                return false;
            }

            return matchingBrackets.Count == 0 ? true : false;
        }

        /// <summary>
        /// Checks if a string contains an operator.
        /// </summary>
        /// <param name="item">The string to check is passed in.</param>
        /// <returns>True or false if it contains an operator.</returns>
        public static bool IsOperator(string item) => "+-/*".Contains(item);

        /// <summary>
        /// Checks if a char is an operator.
        /// </summary>
        /// <param name="item">Passes in the char to check and uses string IsOperator logic.</param>
        /// <returns>True or false if it contains an operator.</returns>
        public static bool IsOperator(char item) => IsOperator(item.ToString());

        /// <summary>
        /// Get the next symbol in the expression.
        /// </summary>
        /// <param name="expression">The string expression to get the next symbol from.</param>
        /// <returns>Returns static string.</returns>
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
                    else
                    {
                        break;
                    }
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

        /// <summary>
        /// Gets the first symbols before an operator in a expression string.
        /// </summary>
        /// <param name="expression">Expression string to get first variable symbols from.</param>
        /// <returns>Returns a string (IEnumberable type) with the string until the operator.</returns>
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
        /// ParseNodes takes an expression and creates a tree of nodes from it.
        /// </summary>
        /// <param name="expression">string expression to create tree from.</param>
        /// <returns>Returns an operator node that is the root of the tree.</returns>
        public OperatorNode ParseNodes(string expression)
        {
            string trimmedExpression = string.Concat(expression.Where(c => !char.IsWhiteSpace(c)));
            StringBuilder firstVariableString = new();
            StringBuilder secondVariableString = new();
            int currentNodeIndex = 0;
            int operatorSplitIndex = 0;
            int operatorsInSubstringCount = 0;
            OperatorNode? tempRoot = null;

            if (MatchingParenthesis(expression) == false)
            {
                throw new Exception("Parenthesis in expression do not match.");
            }

            for (int i = 0; i < trimmedExpression.Length; i++)
            {
                if (IsOperator(trimmedExpression[i]) || i == trimmedExpression.Length - 1)
                {
                    if (operatorsInSubstringCount < 1)
                    {
                        operatorSplitIndex = i;
                    }

                    if (i == trimmedExpression.Length - 1)
                    {
                        secondVariableString.Append(trimmedExpression[i]);
                    }

                    operatorsInSubstringCount++;
                }

                if (operatorsInSubstringCount == 2)
                {
                    if (tempRoot == null)
                    {
                        OperatorNode newNode = OperatorNodeFactory.CreateOperatorNode(trimmedExpression[operatorSplitIndex].ToString());
                        newNode.Left = this.VariableNodeCreator(firstVariableString);
                        newNode.Right = this.VariableNodeCreator(secondVariableString);
                        tempRoot = newNode;
                    }
                    else
                    {
                        OperatorNode newNode = OperatorNodeFactory.CreateOperatorNode(trimmedExpression[operatorSplitIndex].ToString());
                        newNode.Left = this.VariableNodeCreator(firstVariableString);
                        newNode.Right = tempRoot;
                        tempRoot = newNode;
                    }

                    operatorsInSubstringCount = 0;
                    currentNodeIndex = i;
                    firstVariableString.Clear();
                    secondVariableString.Clear();
                }
                else
                {
                    if (operatorsInSubstringCount < 1)
                    {
                        firstVariableString.Append(trimmedExpression[i]);
                    }

                    if (operatorsInSubstringCount == 1 && !IsOperator(trimmedExpression[i]))
                    {
                        secondVariableString.Append(trimmedExpression[i]);
                    }
                }
            }

            return tempRoot;
        }

        /// <summary>
        /// Creates a variable node or constant node depending on the string passed in.
        /// </summary>
        /// <param name="variableString">String to create node from.</param>
        /// <returns>Returns constantNode or VariableNode.</returns>
        public Node VariableNodeCreator(StringBuilder variableString)
        {
            double constant;
            if (double.TryParse(variableString.ToString(), out constant))
            {
                return new ConstantNode(constant);
            }
            else
            {
                return new VariableNode(variableString.ToString(), ref this.variables);
            }
        }

        /// <summary>
        /// Sets the specified variable within the ExpressionTree variables dictionary.
        /// </summary>
        /// <param name="variableName">Passes in the variable name.</param>
        /// <param name="variableValue">Passes in the variable value.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            this.variables[variableName] = variableValue;
        }

        /// <summary>
        /// This method has no parameters but evaluates the expression to a double value.
        /// </summary>
        /// <returns>The evaluation of the expression.</returns>
        public double Evaluate()
        {
            return this.rootNode?.Evaluate() ?? 0.0f;
        }
    }
}
