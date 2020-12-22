// <copyright file="ConstantNode.cs" company="Mantz Wyrick - 11504292">
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
    /// ConstantNode class, inherits from the abstract ExpressionTreeNode class
    /// to evaluate a constant node.
    /// </summary>
    internal class ConstantNode : ExpressionTreeNode
    {
        /// <summary>
        /// private readonly memember that gets evaluated as is since it is just a constant double.
        /// </summary>
        private readonly double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public ConstantNode(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the private member value.
        /// </summary>
        public double Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Evaluate the constant node.
        /// </summary>
        /// <returns>The constant value of the current node.</returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
