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
            TraverseAvailableOperators((op, type) => operators.Add(op, type));
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

        /// <summary>
        /// Returns a list of all supported operators
        /// </summary>
        /// <returns>The operators</returns>
        public List<char> GetOperators()
        {
            return new List<char>(operators.Keys);
        }

        /// <summary>
        /// CreateOperatorNode will return an OperatorNode.
        /// </summary>
        /// <param name="operator">A string with the operator to create a node type of.</param>
        /// <returns></returns>
        public OperatorNode CreateOperatorNode(char @operator)
        {
            if (operators.ContainsKey((@operator)))
            {
                object operatorNodeObject = System.Activator.CreateInstance(operators[@operator]);
                if (operatorNodeObject is OperatorNode)
                {
                    return (OperatorNode)operatorNodeObject;
                }
            }

            throw new Exception("Unhandled operator");
        }

        public OperatorNode CreateOperatorNode(string @operator) => CreateOperatorNode(@operator[0]);

        public ushort GetPrecedence(char @operator)
        {
            ushort precedenceValue = 0;
            if (operators.ContainsKey((@operator)))
            {
                Type type = operators[@operator];
                PropertyInfo propertyInfo = type.GetProperty("Precedence");
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue((type));
                    if (propertyValue is ushort)
                    {
                        precedenceValue = (ushort)propertyValue;
                    }
                }
            }

            return precedenceValue;
        }

        public OperatorNode.Associative GetAssociativity(char @operator)
        {
            OperatorNode.Associative associative = OperatorNode.Associative.Right;
            if (operators.ContainsKey(@operator))
            {
                Type type = operators[@operator];
                PropertyInfo propertyInfo = type.GetProperty("Associativity");
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue((type));
                    if (propertyValue is OperatorNode.Associative)
                    {
                        associative = (OperatorNode.Associative)propertyValue;
                    }
                }
            }

            return associative;
        }

        public bool IsOperator(char c)
        {
            return operators.ContainsKey(c);
        }
    }
}
