// <copyright file="MinusOperatorNode.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace SpreadSheetEngine
{
    /// <summary>
    /// MinusOperatorNode class, inherits from abstract OperatorNode class.
    /// Evaluates the expression of two nodes with subtraction operator.
    /// </summary>
    internal class MinusOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinusOperatorNode"/> class.
        /// </summary>
        public MinusOperatorNode()
        {
        }

        /// <summary>
        /// Gets the MinusOperatorNode operator.
        /// </summary>
        public static char Operator => '-';

        /// <summary>
        /// Gets the precedence of the operator.
        /// </summary>
        public static ushort Precedence => 7;

        /// <summary>
        /// Gets the associativity of the operator.
        /// </summary>
        public static Associative Associativity => Associative.Left;

        /// <summary>
        /// Evaluates the left and right node and then subtracts them to return a double value.
        /// </summary>
        /// <returns>the subtraction of the right node from the left node.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() - this.Right.Evaluate();
        }
    }
}
