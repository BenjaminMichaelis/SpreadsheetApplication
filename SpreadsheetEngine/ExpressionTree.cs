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
        private Node? rootNode;
        private OperatorNodeFactory operatorNodeFactory = new();

        /// <summary>
        /// Gets or sets private dictionary variables.
        /// </summary>
        public Dictionary<string, double> Values { get => this.variables; set => this.variables = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">The string representing the new expression to construct tree from.</param>
        public ExpressionTree(string expression)
        {
            this.rootNode = this.Build(expression);
        }

        /// <summary>
        /// Checks if a string contains an operator.
        /// </summary>
        /// <param name="item">The string to check is passed in.</param>
        /// <returns>True or false if it contains an operator.</returns>
        public static bool IsLeftParenthesis(char item) => IsLeftParenthesis(item.ToString());

        /// <summary>
        /// Checks if a string contains an operator.
        /// </summary>
        /// <param name="item">The string to check is passed in.</param>
        /// <returns>True or false if it contains an operator.</returns>
        public static bool IsRightParenthesis(char item) => IsRightParenthesis(item.ToString());

        /// <summary>
        /// Checks if a string contains an operator.
        /// </summary>
        /// <param name="item">The string to check is passed in.</param>
        /// <returns>True or false if it contains an operator.</returns>
        public static bool IsLeftParenthesis(string item) => "(".Contains(item);

        /// <summary>
        /// Checks if a string contains an operator.
        /// </summary>
        /// <param name="item">The string to check is passed in.</param>
        /// <returns>True or false if it contains an operator.</returns>
        public static bool IsRightParenthesis(string item) => ")".Contains(item);

        /// <summary>
        /// Operates the ShuntingYardAlgorithm on a given string expression.
        /// http://math.oxford.emory.edu/site/cs171/shuntingYardAlgorithm/.
        /// </summary>
        /// <param name="expression">Expression string to run shunting yard algorithm on.</param>
        /// <returns>Returns a list with the postfix expression.</returns>
        public List<string> ShuntingYardAlgorithm(string expression)
        {
            List<string> postfixExpression = new();
            Stack<char> operators = new();
            int operandStart = -1;
            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];
                if (this.IsOperatorOrParenthesis(c))
                {
                    // 1.
                    if (operandStart != -1)
                    {
                        string operand = expression.Substring(operandStart, i - operandStart);
                        postfixExpression.Add(operand);
                        operandStart = -1;
                    }

                    // 2.
                    if (IsLeftParenthesis(c.ToString()))
                    {
                        operators.Push(c);
                    }

                    // 3.
                    else if (IsRightParenthesis(c.ToString()))
                    {
                        char op = operators.Pop();
                        while (!IsLeftParenthesis(op.ToString()))
                        {
                            postfixExpression.Add(op.ToString());
                            op = operators.Pop();
                        }
                    }

                    // 4.
                    else if (this.operatorNodeFactory.IsOperator(c))
                    {
                        if (operators.Count == 0 || IsLeftParenthesis(operators.Peek()))
                        {
                            operators.Push(c);
                        }

                        // 5.
                        else if (this.IsHigherPrecedence(c, operators.Peek()) || (this.IsSamePrecedence(c, operators.Peek()) && this.IsRightAssociative(c)))
                        {
                            operators.Push(c);
                        }

                        // 6.
                        else if (this.IsLowerPrecedence(c, operators.Peek()) || (this.IsSamePrecedence(c, operators.Peek()) && this.IsLeftAssociative(c)))
                        {
                            do
                            {
                                char op = operators.Pop();
                                postfixExpression.Add(op.ToString());
                            }
                            while (operators.Count > 0 && (this.IsLowerPrecedence(c, operators.Peek()) || (this.IsSamePrecedence(c, operators.Peek()) && this.IsLeftAssociative(c))));

                            operators.Push(c);
                        }
                    }
                }
                else if (operandStart == -1)
                {
                    operandStart = i;
                }
            }

            if (operandStart != -1)
            {
                postfixExpression.Add(expression.Substring(operandStart, expression.Length - operandStart));
                operandStart = -1;
            }

            while (operators.Count > 0)
            {
                postfixExpression.Add(operators.Pop().ToString());
            }

            return postfixExpression;
        }

        /// <summary>
        /// Set the variable value to the string name.
        /// </summary>
        /// <param name="variableName">The name of the variable.</param>
        /// <param name="variableValue">The value to assign.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            this.variables[variableName] = variableValue;
        }

        /// <summary>
        /// Evaluate expression tree.
        /// </summary>
        /// <returns>Returns the result of the tree in a double.</returns>
        public double Evaluate()
        {
            return this.rootNode.Evaluate();
        }

        /// <summary>
        /// Checks if operator is left associative.
        /// </summary>
        /// <param name="c">Operator char.</param>
        /// <returns>Returns true or false.</returns>
        public bool IsLeftAssociative(char c)
        {
            if (this.operatorNodeFactory.GetAssociativity(c) == OperatorNode.Associative.Left)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if operator is right associative.
        /// </summary>
        /// <param name="c">Operator char.</param>
        /// <returns>Returns true or false.</returns>
        public bool IsRightAssociative(char c)
        {
            if (this.operatorNodeFactory.GetAssociativity(c) == OperatorNode.Associative.Right)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if left operator is lower precedence than the right side.
        /// Lower Precedence is higher when we look at http://web.deu.edu.tr/doc/oreily/java/langref/ch04_14.htm.
        /// </summary>
        /// <param name="c">Left operator char.</param>
        /// <param name="v">Right operator char.</param>
        /// <returns>Returns true or false.</returns>
        public bool IsLowerPrecedence(char c, char v)
        {
            if (this.operatorNodeFactory.GetPrecedence(c) > this.operatorNodeFactory.GetPrecedence(v))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if left operator is same precedence as the right side.
        /// </summary>
        /// <param name="c">Left operator char.</param>
        /// <param name="v">Right operator char.</param>
        /// <returns>Returns true or false.</returns>
        public bool IsSamePrecedence(char c, char v)
        {
            if (this.operatorNodeFactory.GetPrecedence(c) == this.operatorNodeFactory.GetPrecedence(v))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if left operator is higher precedence than the right side.
        /// Lower Precedence is higher when we look at http://web.deu.edu.tr/doc/oreily/java/langref/ch04_14.htm.
        /// </summary>
        /// <param name="c">Left operator char.</param>
        /// <param name="v">Right operator char.</param>
        /// <returns>Returns true or false.</returns>
        public bool IsHigherPrecedence(char c, char v)
        {
            if (this.operatorNodeFactory.GetPrecedence(c) < this.operatorNodeFactory.GetPrecedence(v))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a string contains an operator.
        /// </summary>
        /// <param name="item">The string to check is passed in.</param>
        /// <returns>True or false if it contains an operator.</returns>
        public bool IsOperatorOrParenthesis(char item)
        {
            if ("()".Contains(item.ToString()) || this.operatorNodeFactory.IsOperator(item))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Node Build(string expression)
        {
            Stack<Node> nodes = new();
            var posfixExpression = this.ShuntingYardAlgorithm(expression);
            foreach (var item in posfixExpression)
            {
                if (item.Length == 1 && this.IsOperatorOrParenthesis(item[0]))
                {
                    OperatorNode node = this.operatorNodeFactory.CreateOperatorNode(item[0]);
                    node.Right = nodes.Pop();
                    node.Left = nodes.Pop();
                    nodes.Push(node);
                }
                else
                {
                    double num = 0.0;
                    if (double.TryParse(item, out num))
                    {
                        nodes.Push(new ConstantNode(num));
                    }
                    else
                    {
                        nodes.Push(new VariableNode(item, ref this.variables));
                    }
                }
            }

            return nodes.Pop();
        }
    }
}
