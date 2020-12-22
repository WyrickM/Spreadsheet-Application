// <copyright file="TestComplexExpressionTree.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace CptS321
{
    /// <summary>
    /// This class tests more complex expressions for the expression tree. Expressions may include
    /// parentheses and different operators.
    /// </summary>
    [TestFixture]
    public class TestComplexExpressionTree
    {
        /// <summary>
        /// Testing an expression of only parentheses.
        /// </summary>
        [Test]
        public void TestExpressionOnlyParentheses()
        {
            ExpressionTree testTree = new ExpressionTree("()");
            Assert.Throws<System.Exception>(() => testTree.Evaluate());
        }

        /// <summary>
        /// Testing an expression that contains a single value in parentheses.
        /// </summary>
        [Test]
        public void TestExpressionOneValueInParentheses()
        {
            ExpressionTree testTree = new ExpressionTree("(1)");
            Assert.AreEqual(1, testTree.Evaluate());
        }

        /// <summary>
        /// Testing an expression that contains a single unvalued variable in parentheses.
        /// </summary>
        [Test]
        public void TestExpressionOneUnvaluedVariableInParentheses()
        {
            ExpressionTree testTree = new ExpressionTree("(A)");
            Assert.Throws<System.Exception>(() => testTree.Evaluate());
        }

        /// <summary>
        /// Testing an expression that contains a simple addition expression in parentheses.
        /// </summary>
        [Test]
        public void TestExpressionSimpleAdditionInParentheses()
        {
            ExpressionTree testTree = new ExpressionTree("(1+2)");
            Assert.AreEqual(3, testTree.Evaluate());
        }

        /// <summary>
        /// Testing an expression that contains a simple addition inside parentheses and outside.
        /// </summary>
        [Test]
        public void TestExpressionSimpleAdditionInParenthesesAndOut()
        {
            ExpressionTree testTree = new ExpressionTree("(1+2)+1");
            Assert.AreEqual(4, testTree.Evaluate());
        }

        /// <summary>
        /// This expression test the precedence of the parentheses, addition, and multiplication
        /// by having an addtion expression in the parenthises and a multiplication expression outside
        /// the parentheses.
        /// </summary>
        [Test]
        public void TestExpressionSimpleAdditionInParensesMulOut()
        {
            ExpressionTree testTree = new ExpressionTree("(1+2)*2");
            Assert.AreEqual(6, testTree.Evaluate());
        }

        /// <summary>
        /// Testing an expression with multiple parentesis expressions.
        /// </summary>
        [Test]
        public void TestExpressionMultipleParenthesis()
        {
            ExpressionTree testTree = new ExpressionTree("(1+2)*(2+3)");
            Assert.AreEqual(15, testTree.Evaluate());
        }

        /// <summary>
        /// Testing an advanced expression with no parentheses. Need to use precedence and associativity correctly.
        /// </summary>
        [Test]
        public void TestAdvancedExpressionNoParenthesis()
        {
            ExpressionTree testTree = new ExpressionTree("15/3*5+1/13");
            Assert.AreEqual(25.076923076923077, testTree.Evaluate());
        }

        /// <summary>
        /// Testing an advanced expression with parentheses. Need to use precedence and associativity correctly.
        /// </summary>
        [Test]
        public void TestAdvancedExpressionWithParenthesis()
        {
            ExpressionTree testTree = new ExpressionTree("(((15/3)*5)+1)/13");
            Assert.AreEqual(2, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the evaluate function when dividing a positive number by 0.
        /// </summary>
        [Test]
        public void TestDividePositiveBy0()
        {
            ExpressionTree testTree = new ExpressionTree("1/0");
            Assert.IsTrue(double.IsInfinity(testTree.Evaluate()));
        }

        /// <summary>
        /// Testing the evaluate function when dividing a negative number by 0.
        /// </summary>
        [Test]
        public void TestDivideNegativeBy0()
        {
            ExpressionTree testTree = new ExpressionTree("(0-1)/0");
            Assert.IsTrue(double.IsNegativeInfinity(testTree.Evaluate()));
        }

        /// <summary>
        /// Testing what the max value that the evaluate expression can return. Infinity is max value.
        /// </summary>
        [Test]
        public void TestMulInfinityExpression()
        {
            ExpressionTree testTree = new ExpressionTree("1000000000000000000000000*1000000000000000000000000*" +
                "1000000000000000000000000*1000000000000000000000000*1000000000000000000000000" +
                "*1000000000000000000000000*1000000000000000000000000*1000000000000000000000000" +
                "*1000000000000000000000000*1000000000000000000000000*1000000000000000000000000" +
                "*1000000000000000000000000*1000000000000000000000000*1000000000000000000000000" +
                "*1000000000000000000000000*1000000000000000000000000*1000000000000000000000000" +
                "*1000000000000000000000000*1000000000000000000000000*1000000000000000000000000" +
                "*1000000000000000000000000*1000000000000000000000000*1000000000000000000000000" +
                "*1000000000000000000000000*1000000000000000000000000*1000000000000000000000000" +
                "*1000000000000000000000000*1000000000000000000000000*1000000000000000000000000");
            Assert.IsTrue(double.IsInfinity(testTree.Evaluate()));
        }

        /// <summary>
        /// Testing dividing 1 by big number until evaluate returns 0.
        /// </summary>
        [Test]
        public void TestDiv0Expression()
        {
            ExpressionTree testTree = new ExpressionTree("1/1000000000000000000000000/1000000000000000000000000" +
                "/1000000000000000000000000/1000000000000000000000000/1000000000000000000000000" +
                "/1000000000000000000000000/1000000000000000000000000/1000000000000000000000000" +
                "/1000000000000000000000000/1000000000000000000000000/1000000000000000000000000" +
                "/1000000000000000000000000/1000000000000000000000000/1000000000000000000000000" +
                "/1000000000000000000000000/1000000000000000000000000/1000000000000000000000000" +
                "/1000000000000000000000000/1000000000000000000000000/1000000000000000000000000" +
                "/1000000000000000000000000/1000000000000000000000000/1000000000000000000000000");
            Assert.AreEqual(0, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the minimum value returned by evaluate using subtraction.
        /// </summary>
        [Test]
        public void TestNegativeMaxValueSubtraction()
        {
            ExpressionTree testTree = new ExpressionTree("1-100000000000000000000000000000000000" +
                "0000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                "0000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                "0000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                "000000000000000000000000000000000000");
            Assert.AreEqual(-1E+308, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the maximum value returned by evaluate using addition. If add even one more 0 at the end
        /// Evaluate will return 1.0d.
        /// </summary>
        [Test]
        public void TestPositiveMaxValueAddition()
        {
            ExpressionTree testTree = new ExpressionTree("1+100000000000000000000000000000000000" +
                "0000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                "0000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                "0000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                "000000000000000000000000000000000000");
            Assert.AreEqual(1E+308, testTree.Evaluate());
        }

        /// <summary>
        /// Testing if evaluate can handle spaces/whitespaces in an expression.
        /// </summary>
        [Test]
        public void TestSpaceCharacter()
        {
            ExpressionTree testTree = new ExpressionTree("1 + 2");
            Assert.AreEqual(3, testTree.Evaluate());

            testTree = new ExpressionTree(" 4 * 2 ");
            Assert.AreEqual(8, testTree.Evaluate());
        }
    }
}
