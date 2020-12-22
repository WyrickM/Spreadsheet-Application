// <copyright file="Program.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressionTreeApp
{
    /// <summary>
    /// Runs the main program for the winforms app.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">args.</param>
        [STAThread]
        private static void Main(string[] args)
        {
            string userInput = string.Empty, expression = "A1+B1+C1", variableName = string.Empty;
            string variableValue = string.Empty;
            double result = 0.0; // variableValue = 0.0;

            CptS321.ExpressionTree expressionTree = new CptS321.ExpressionTree(expression);

            while (userInput != "4")
            {
                Console.WriteLine("Please enter an option from the menu:");
                Console.WriteLine("The current expression is \"{0}\"", expression);
                Console.WriteLine("   1 = Enter a new expression");
                Console.WriteLine("   2 = Set a varaible value");
                Console.WriteLine("   3 = Evaluate tree");
                Console.WriteLine("   4 = Quit");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        Console.Write("Please enter a new expression: ");
                        expression = Console.ReadLine();

                        // add expression to expression tree
                        expressionTree = new CptS321.ExpressionTree(expression);

                        break;

                    case "2":
                        Console.Write("Please enter a variable name: ");
                        variableName = Console.ReadLine();
                        Console.Write("Please enter a variable value: ");

                        // variableValue = Convert.ToDouble(Console.ReadLine());
                        variableValue = Console.ReadLine();

                        // add variable to variables dictionary
                        expressionTree.SetVariable(variableName, variableValue);

                        break;

                    case "3":
                        // result = evaluated expression
                        try
                        {
                            result = expressionTree.Evaluate();
                            Console.WriteLine("{0}", result);
                        }
                        catch (Exception e)
                        {
                            // catching exception if the expression cannot be evaluated
                            // then printing the error message to the screen.
                            Console.WriteLine(e.Message);
                        }

                        break;
                }
            }
        }
    }
}
