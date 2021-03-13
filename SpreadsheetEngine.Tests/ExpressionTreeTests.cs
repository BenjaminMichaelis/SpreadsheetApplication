// <copyright file="ExpressionTreeTests.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
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
        [InlineData("A", "0", 0)]
        [InlineData("A", "5", 5)]
        [InlineData("A+B", "10+100", 110)]
        public void EvaluateTests(string expression, string values, double expected)
        {
            ExpressionTree tree = new ExpressionTree(expression);
            foreach (int value in ExpressionTree.GetSymbols(values).Where(item => !ExpressionTree.IsOperator(item))
                .Select(item => int.Parse(item)))
            {
                tree.SetVariable(expression, value);
            }
            Assert.Equal(expected, tree.Evaluate());
        }

        /// <summary>
        /// Test get symbols function.
        /// </summary>
        /// <param name="expression">Pass in expression to test.</param>
        [Theory]
        [InlineData("A+B+C")]
        public void GetSymbols(string expression)
        {
            StringBuilder result = new (string.Empty);
            foreach(string symbol in ExpressionTree.GetSymbols(expression))
            {
                result.Append(symbol);
            }

            Assert.Equal(expression, result.ToString());
        }
    }
}
