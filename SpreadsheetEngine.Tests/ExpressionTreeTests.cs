// <copyright file="ExpressionTreeTests.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CptS321;
using Xunit;

namespace SpreadsheetEngine.Tests
{
    /// <summary>
    /// Test methods in the spreadsheetClass.
    /// </summary>
    public class ExpressionTreeTests
    {
        /// <summary>
        /// Tests Evaluate.
        /// </summary>
        /// <param name="expression">The expression passed into the test.</param>
        /// <param name="valuesExpression">The value to assign to each variable.</param>
        /// <param name="expected">The expected result from the input.</param>
        [Theory]

        // [InlineData("(((((2+3)-(4+5)))))", -4.0)]
        // [InlineData("100/10*10", 100.0)]
        // [InlineData("0/0", double.NaN)]
        // [InlineData("1/0", double.PositiveInfinity)]
        // [InlineData("5/0", double.PositiveInfinity)]
        // [InlineData("10/(2*5)", 1.0)]
        // [InlineData("84-14+26", 96.0)]
        // [InlineData("7-4+2", 5.0)]
        // [InlineData("4+7-2", 9.0)]
        // [InlineData("5-4-3+10", 8.0)]
        // [InlineData("5-4+10-3+7-6", 9.0)]
        // [InlineData("10/(7-2)", 2.0)]
        // [InlineData("(12-2)/2", 5.0)]
        // [InlineData("2*3+5", 11.0)]
        // [InlineData("2+3*5", 17.0)]
        // [InlineData("2 + 3 * 5", 17.0)]
        // [InlineData("A-B-C", 0)]
        // [InlineData("3+5", 8.0)]
        // [InlineData("A", "0", 0)]
        // [InlineData("A", "5", 5)]
        [InlineData("A+B", "5+7", 12)]
        [InlineData("A+B", "100+70", 170)]
        [InlineData("Hello+World", "100+70", 170)]
        [InlineData("Hello+World+Everywhere", "100+70+30", 200)]
        [InlineData("Hello-World-Everywhere", "100-70-20", 10)]
        public void EvaluateTests(string expression, string valuesExpression, double expected)
        {
            ExpressionTree tree = new ExpressionTree(expression);
            IEnumerable<string>? operands = ExpressionTree.GetSymbols(expression)
                    .Where(item => !ExpressionTree.IsOperator(item));
            IEnumerable<int>? values = ExpressionTree.GetSymbols(valuesExpression)
                    .Where(item => !ExpressionTree.IsOperator(item))
                    .Select(item => int.Parse(item));

            IEnumerable<(string operand, int value)>? operandValues =
                operands.Zip(values, (operand, value) => (operand, value));
            foreach ((string operand, int value) in operandValues)
            {
                tree.SetVariable(operand, value);
            }

            Assert.Equal(expected, tree.Evaluate());
        }

        /// <summary>
        /// Test get symbols function.
        /// </summary>
        /// <param name="expression">Pass in expression to test.</param>
        /// <param name="expected">Expected List result.</param>
        [Theory]
        [MemberData(nameof(Data))]
        public void GetSymbols(string expression, List<string> expected)
        {
            List<string> result = new();
            foreach (string symbol in ExpressionTree.GetSymbols(expression))
            {
                result.Add(symbol);
            }

            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Gets Member data for GetSymbols function.
        /// </summary>
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { "A+B+C", new List<string> { "A", "B", "C" } },
            new object[] { "1+2+3", new List<string> { "1", "2", "3" } },
            new object[] { "AA+BBB+Hello", new List<string> { "AA", "BBB", "Hello" } },
        };

        /// <summary>
        /// Tests getNextSymbol.
        /// </summary>
        /// <param name="expression">Expression that is being passed in.</param>
        /// <param name="firstSymbol">The first symbol that should be passed from the method.</param>
        /// <param name="remainingExpression">The remainder of the expression that should be passed from the method.</param>
        [Theory]

        // [InlineData("A+B","A","+B")]
        // [InlineData("AB+B", "AB", "+B")]
        // [InlineData("ABC+BC", "ABC", "+BC")]
        // [InlineData("ABC+BC+D", "ABC", "+BC+D")]
        [InlineData("+BC+D", "+", "BC+D")]
        public void GetNextSymbol(string expression, string firstSymbol, string remainingExpression)
        {
            string actual = ExpressionTree.GetNextSymbol(ref expression);
            Assert.Equal(firstSymbol, actual);
            Assert.Equal(remainingExpression, expression);
        }

        ///// <summary>
        ///// Tests ParseExpressionNode.
        ///// </summary>
        // [Fact]
        // public void ParseExpressionNode()
        // {
        //    OperatorNode actual = (OperatorNode)ExpressionTree.ParseExpression("A+B");
        //    Assert.Equal("A", ((VariableNode)actual.Left).Name);
        //    Assert.Equal("B", ((VariableNode)actual.Right).Name);
        // }
    }
}
