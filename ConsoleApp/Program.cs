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
            bool exitApplication = false;
            do
            {
                ShowMenu();

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
                        string newExpression = Console.ReadLine();
                        break;

                    // 2. Set a variable value
                    case 2:
                        break;

                    // 3. Evaluate tree
                    case 3:
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

                // if we want to give the user time to read the result before showing the menu again:
                Console.WriteLine("Press any key to continue.\n");
                Console.ReadKey();
            }
            while (!exitApplication);
        }

        /// <summary>
        /// Prints menu to the Console.
        /// </summary>
        private static void ShowMenu()
        {
            Console.WriteLine($"\"Main Menu (current expression=\"{0}");
            Console.WriteLine("1. Enter a new expression");
            Console.WriteLine("2. Set a variable value");
            Console.WriteLine("3. Evaluate tree");
            Console.WriteLine("4. Quit");
        }
    }
}
