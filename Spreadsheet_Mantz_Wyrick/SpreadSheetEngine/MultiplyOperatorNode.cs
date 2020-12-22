// <copyright file="MultiplyOperatorNode.cs" company="Mantz Wyrick - 11504292">
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
    /// MultiplyOperatorNode class, inherits from abstract OperatorNode class.
    /// Evaluates the expression of two nodes with multiplication operator.
    /// </summary>
    internal class MultiplyOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyOperatorNode"/> class.
        /// </summary>
        public MultiplyOperatorNode()
        {
        }

        /// <summary>
        /// Gets the MultiplyOperatorNode operator.
        /// </summary>
        public static char Operator => '*';

        /// <summary>
        /// Gets the precedence of the operator.
        /// </summary>
        public static ushort Precedence => 6;

        /// <summary>
        /// Gets the associativity of the operator.
        /// </summary>
        public static Associative Associativity => Associative.Left;

        /// <summary>
        /// Evaluates the left and right node and then multiply them to return a double value.
        /// </summary>
        /// <returns>the multiplication of the left and right nodes.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() * this.Right.Evaluate();
        }
    }
}
