// <copyright file="ExpressionTreeNode.cs" company="Mantz Wyrick - 11504292">
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
    /// Abstract Expression tree node, inherited by ExpressionTree,
    /// ConstantNode, OperatorNode, and VariableNode classes.
    /// </summary>
    internal abstract class ExpressionTreeNode
    {
        /// <summary>
        /// Abstract method that will be overrided by VariableNode,
        /// OperatorNode's subclasses, and ConstantNode.
        /// </summary>
        /// <returns>A double value, that is the value after a expression is evaluated.</returns>
        public abstract double Evaluate();
    }
}
