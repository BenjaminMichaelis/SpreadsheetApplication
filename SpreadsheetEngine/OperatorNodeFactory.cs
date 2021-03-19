// <copyright file="OperatorNodeFactory.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Creates OperatorNodes.
    /// </summary>
    public class OperatorNodeFactory
    {
        private Dictionary<char, Type> operators = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
        /// </summary>
        public OperatorNodeFactory()
        {
            // Instantiate the delegate with a lambda expression
            this.TraverseAvailableOperators((op, type) => this.operators.Add(op, type));
        }

        /// <summary>
        /// Returns a list of all supported operators.
        /// </summary>
        /// <returns>The operators.</returns>
        public List<char> GetOperators()
        {
            return new List<char>(this.operators.Keys);
        }

        /// <summary>
        /// CreateOperatorNode will return an OperatorNode.
        /// </summary>
        /// <param name="operator">A string with the operator to create a node type of.</param>
        /// <returns>Returns an operator node.</returns>
        public OperatorNode CreateOperatorNode(char @operator)
        {
            if (this.operators.ContainsKey(@operator))
            {
                object operatorNodeObject = System.Activator.CreateInstance(this.operators[@operator]);
                if (operatorNodeObject is OperatorNode)
                {
                    return (OperatorNode)operatorNodeObject;
                }
            }

            throw new Exception("Unhandled operator");
        }

        /// <summary>
        /// Creates an operator node from a string by passing the first char to create it.
        /// </summary>
        /// <param name="operator">string of operator.</param>
        /// <returns>Returns an operatorNode.</returns>
        public OperatorNode CreateOperatorNode(string @operator) => this.CreateOperatorNode(@operator[0]);

        /// <summary>
        /// Gets the precedence of an operator.
        /// </summary>
        /// <param name="operator">The char representing an operator.</param>
        /// <returns>Returns a int value of the precedence.</returns>
        public ushort GetPrecedence(char @operator)
        {
            ushort precedenceValue = 0;
            if (this.operators.ContainsKey(@operator))
            {
                Type type = this.operators[@operator];
                PropertyInfo propertyInfo = type.GetProperty("Precedence");
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue(type);
                    if (propertyValue is ushort)
                    {
                        precedenceValue = (ushort)propertyValue;
                    }
                }
            }

            return precedenceValue;
        }

        /// <summary>
        /// Gets the associativity of an operator.
        /// </summary>
        /// <param name="operator">the char of the operator.</param>
        /// <returns>Returns the Associative enum of that operator.</returns>
        public OperatorNode.Associative GetAssociativity(char @operator)
        {
            OperatorNode.Associative associative = OperatorNode.Associative.Right;
            if (this.operators.ContainsKey(@operator))
            {
                Type type = this.operators[@operator];
                PropertyInfo propertyInfo = type.GetProperty("Associativity");
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue(type);
                    if (propertyValue is OperatorNode.Associative)
                    {
                        associative = (OperatorNode.Associative)propertyValue;
                    }
                }
            }

            return associative;
        }

        /// <summary>
        /// Checks if a char is an operator or not.
        /// </summary>
        /// <param name="c">char to check if it is an operator.</param>
        /// <returns>True or false if the char is an operator.</returns>
        public bool IsOperator(char c)
        {
            return this.operators.ContainsKey(c);
        }

        private delegate void OnOperator(char op, Type type);

        private void TraverseAvailableOperators(OnOperator onOperator)
        {
            {
                // get the type declaration of OperatorNode
                Type operatorNodeType = typeof(OperatorNode);

                // Iterate over all loaded assemblies:
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    // Get all types that inherit from our OperatorNode class using LINQ
                    IEnumerable<Type> operatorTypes =
                    assembly.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));

                    // Iterate over those subclasses of OperatorNode
                    foreach (var type in operatorTypes)
                    {
                        // for each subclass, retrieve the Operator property
                        PropertyInfo operatorField = type.GetProperty("Operator");
                        if (operatorField != null)
                        {
                            // Get the character of the Operator
                            object value = operatorField.GetValue(type);

                            // If the property is not static, use the following code instead: object value = operatorField.GetValue(Activator.CreateInstance(type, new ConstantNode("0"), new ConstantNode("0")));
                            if (value is char)
                            {
                                char operatorSymbol = (char)value;

                                // And invoke the function passed as parameter
                                // with the operator symbol and the operator class
                                onOperator(operatorSymbol, type);
                            }
                        }
                    }
                }
            }
        }
    }
}
