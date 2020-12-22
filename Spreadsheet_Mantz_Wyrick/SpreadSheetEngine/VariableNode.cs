// <copyright file="VariableNode.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// VariableNode class, inherits from the abstract ExpressionTreeNode class.
    /// To be able to evaluate only the variables in this class.
    /// </summary>
    internal class VariableNode : ExpressionTreeNode
    {
        /// <summary>
        /// private readonly string inputed by the user, compared to the
        /// keys in the variables dicitonary.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// string and double dictionary that will hold the names(keys) of the variables
        /// and their values(values).
        /// </summary>
        private Dictionary<string, double> variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="name">Name of the variable node.</param>
        /// <param name="variables">Reference to the dictionary of variables
        /// that will be changed by the user when setting the values of variable nodes.</param>
        public VariableNode(string name, ref Dictionary<string, double> variables)
        {
            this.name = name;
            this.variables = variables;
        }

        /// <summary>
        /// Returns the value of the node using the dictionary of variables, if
        /// not found the value will be 0.0.
        /// </summary>
        /// <returns>0.0 or the value assigned to the variable in the variables dictionary.</returns>
        public override double Evaluate()
        {
            // double value;

            //// checking to see if variable(name) is in the variables dictionary.
            // if (this.variables.ContainsKey(this.name))
            // {
            //    // variable(name) in variables dictionary, get the value of the variable set to value.
            //    value = this.variables[this.name];
            // }

            // using exception handling
            try
            {
                // try block to see if dictionary contains key == name
                // variable(name) in variables dictionary, get the value of the variable set to value.
                return this.variables[this.name];
            }
            catch (KeyNotFoundException)
            {
                // catching error when key not found in dictionary
                // throwing error message for other functions to catch
                // to be able to display the message either in the console app
                // or the winforms app in a cell.
                throw new KeyNotFoundException("#VALUE!");
            }
        }
    }
}
