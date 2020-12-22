// <copyright file="OperatorNode.cs" company="Mantz Wyrick - 11504292">
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
    /// abstract OperatorNode class that inherits from the ExpressionTreeNode class,
    /// will be inherited by all of the specific operators (+,-,*,/) classes.
    /// </summary>
    internal abstract class OperatorNode : ExpressionTreeNode
    {
        /// <summary>
        /// Setting associativity of the operators.
        /// </summary>
        public enum Associative
        {
            /// <summary>
            /// For right associativity.
            /// </summary>
            Right,

            /// <summary>
            /// For left associativity.
            /// </summary>
            Left,
        }

        /// <summary>
        /// Gets or Sets the left node.
        /// </summary>
        public ExpressionTreeNode Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the right node.
        /// </summary>
        public ExpressionTreeNode Right
        {
            get;
            set;
        }
    }
}
