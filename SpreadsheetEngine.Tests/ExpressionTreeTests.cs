﻿// <copyright file="ExpressionTreeTests.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace SpreadsheetEngine.Tests
{
    /// <summary>
    /// Test methods in the spreadsheetClass.
    /// </summary>
    public class ExpressionTreeTests
    {
        /// <summary>
        /// Test the shunting yard algorithm logic.
        /// </summary>
        [Fact]
        public void ShuntingYardAlgorithm()
        {
            Dictionary<string, List<string>> data = new();
            data.Add("3+5", new List<string> { "3", "5", "+" });
            data.Add("A3+5", new List<string> { "A3", "5", "+" });
            data.Add("A*(B+C)", new List<string> { "A", "B", "C", "+", "*" });
            foreach (KeyValuePair<string, List<string>> entry in data)
            {
                ExpressionTree exp = new(entry.Key);
                Assert.Equal(exp.ShuntingYardAlgorithm(entry.Key), entry.Value);
            }
        }

        /// <summary>
        /// Tests Evaluate with normal and edge cases such as divide by zero.
        /// </summary>
        /// <param name="expression">The expression passed into the test.</param>
        /// <param name="expected">The expected result from the input.</param>
        [Theory]
        [InlineData("A", 5)]
        [InlineData("D", 0)]
        [InlineData("Hello+World", 170)]
        [InlineData("Hello+Universe", 100)]
        [InlineData("Hello+World+Universe", 170)]
        [InlineData("Hello-World-Everywhere", 30)]
        [InlineData("(((((2+3)-(4+5)))))", -4.0)]
        [InlineData("48-178-87", -217)]
        [InlineData("10/(2*5)", 1.0)]
        [InlineData("3.2+5.4", 8.6000000000000014d)]
        [InlineData("84-14+26", 96.0)]
        [InlineData("7-4+2", 5.0)]
        [InlineData("4+7-2", 9.0)]
        [InlineData("5-4-3+10", 8.0)]
        [InlineData("5-4+10-3+7-6", 9.0)]
        [InlineData("10/(7-2)", 2.0)]
        [InlineData("(12-2)/2", 5.0)]
        [InlineData("2*3+5", 11.0)]
        [InlineData("2+3*5", 17.0)]
        [InlineData("2 + 3 * 5", 17.0)]
        [InlineData("A-B-C", -4)]
        [InlineData("3+5", 8.0)]
        [InlineData("A+B", 12)]
        [InlineData("A*B", 35)]
        [InlineData("100/10*10", 100.0)]
        [InlineData("0/0", double.NaN)]
        [InlineData("1/0", double.PositiveInfinity)]
        [InlineData("5/0", double.PositiveInfinity)]
        [InlineData(" A + B ", 12)]
        [InlineData(" A  * B ", 35)]
        [InlineData(" A* B", 35)]
        [InlineData("A*B ", 35)]
        public void EvaluateTests(string expression, double expected)
        {
            ExpressionTree tree = new(expression);
            tree.SetVariable("A", 5);
            tree.SetVariable("B", 7);
            tree.SetVariable("C", 2);
            tree.SetVariable("Hello", 100);
            tree.SetVariable("World", 70);

            Assert.Equal(expected, tree.Evaluate());
        }

        /// <summary>
        /// Test cell adding another cell reference.
        /// </summary>
        [Fact]
        public void VariablesDictUpToDate()
        {
            ExpressionTree tree = new("=A1+A2");
            Assert.Equal(2, tree.Values.Count);
            tree.Values.TryGetValue("A1", out double temp);
            Assert.Equal(0.0, temp);
            tree.Values.TryGetValue("A2", out temp);
            Assert.Equal(0.0, temp);
        }

        /// <summary>
        /// Tests the algorithm for removing whitespace from a string.
        /// </summary>
        /// <param name="expression">The expression to have whitespace removed from.</param>
        /// <param name="expected">The expected string with no whitespace.</param>
        [Theory]
        [InlineData(" A  B C ", "ABC")]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData("ABC ", "ABC")]
        [InlineData(" ABC", "ABC")]
        [InlineData(" AB C ", "ABC")]
        public void RemoveWhitespaceTests(string expression, string expected)
        {
            // \s indicates a white space and \s+ is any one or more white space characters"
            expression = Regex.Replace(expression, @"\s+", string.Empty);
            Assert.Equal(expected, expression);
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
    }
}
