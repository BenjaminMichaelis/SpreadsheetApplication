// <copyright file="Program.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using SpreadsheetEngine;

namespace ConsoleApp
{
    /// <summary>
    /// Is main entry point class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main program entry point.
        /// </summary>
        /// <param name="args">additional argument to add into Main.</param>
        public static void Main(string[] args)
        {
            ExpressionTree? currentTree = null;
            string currentExpression = string.Empty;
            bool exitApplication = false;
            do
            {
                ShowMenu(currentExpression);

                string? userInput = Console.ReadLine();
                if (!int.TryParse(userInput, out int menuSelection))
                {
                    Console.WriteLine($"{userInput} is not a valid integer\n");
                    continue; // skip the rest of the lool and go to the next iteration
                }

                switch (menuSelection)
                {
                    // 1. Enter a new expression
                    case 1:
                        Console.WriteLine("Enter new expression:");
                        currentExpression = Console.ReadLine();
                        currentTree = new(currentExpression);
                        break;

                    // 2. Set a variable value
                    case 2:
                        if (currentTree == null)
                        {
                            Console.WriteLine("Tree is currently empty, please enter expression with option 1");
                        }
                        else
                        {
                            Console.WriteLine("Enter variable name:");
                            string variableName = Console.ReadLine();
                            Console.WriteLine("Enter variable value:");
                            string variableValue = Console.ReadLine();
                            double newVariableValue = double.Parse(variableValue);
                            currentTree.SetVariable(variableName, newVariableValue);
                        }

                        break;

                    // 3. Evaluate tree
                    case 3:
                        Console.WriteLine(currentTree.Evaluate().ToString());
                        break;

                    // 4. Quit
                    case 4:
                        Console.WriteLine("Done; Exiting Program");
                        exitApplication = true;
                        break;
                    default:
                        Console.WriteLine($"Menu input {menuSelection} is invalid, please try again with a valid input.");
                        break;
                }

                // Skip a line for clarity
                Console.WriteLine();
            }
            while (!exitApplication);
        }

        /// <summary>
        /// Prints menu to the Console.
        /// </summary>
        private static void ShowMenu(string currentExpression)
        {
            Console.WriteLine($"\"Main Menu (current expression=\"{currentExpression})");
            Console.WriteLine("1. Enter a new expression");
            Console.WriteLine("2. Set a variable value");
            Console.WriteLine("3. Evaluate tree");
            Console.WriteLine("4. Quit");
        }
    }
}
