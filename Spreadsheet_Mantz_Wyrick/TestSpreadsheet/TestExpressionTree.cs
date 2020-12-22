// <copyright file="TestExpressionTree.cs" company="Mantz Wyrick - 11504292">
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
    /// This class tests the main methods in the expression tree class.
    /// </summary>
    [TestFixture]
    public class TestExpressionTree
    {
        /// <summary>
        /// private member to test the private expression tree methods.
        /// </summary>
        private ExpressionTree privateTestTree;

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class with an empty expression.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateEmptyExpression()
        {
            ExpressionTree testTree = new ExpressionTree(string.Empty);
            Assert.Throws<System.Exception>(() => testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the addition operator with a simple 2 constant expression.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateSimpleAddExpression()
        {
            ExpressionTree testTree = new ExpressionTree("1+2");
            Assert.AreEqual(3, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the addition operator with a 2 constant expression
        /// with one double constant.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateAddExpressionDouble()
        {
            ExpressionTree testTree = new ExpressionTree("1+2.5");
            Assert.AreEqual(3.5, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the addition operator with a 3 constant expression.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateMoreComplexAddExpression()
        {
            ExpressionTree testTree = new ExpressionTree("1+2+3");
            Assert.AreEqual(6, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the subraction operator with a simple 2 constant expression.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateSimpleSubExpression()
        {
            ExpressionTree testTree = new ExpressionTree("2-1");
            Assert.AreEqual(1, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the subraction operator with a 2 constant expression
        /// with one being a double constant.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateSubExpressionDouble()
        {
            ExpressionTree testTree = new ExpressionTree("2.5-1");
            Assert.AreEqual(1.5, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the subraction operator with a 3 constant expression.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateMoreComplexSubExpression()
        {
            ExpressionTree testTree = new ExpressionTree("6-2-1");
            Assert.AreEqual(3, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the multiplication operator with a simple 2 constant expression.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateSimpleMulExpression()
        {
            ExpressionTree testTree = new ExpressionTree("2*2");
            Assert.AreEqual(4, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the multiplication operator with a 2 constant expression
        /// with one constant being a double.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateMulExpressionDouble()
        {
            ExpressionTree testTree = new ExpressionTree("2.5*2");
            Assert.AreEqual(5, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the multiplication operator with a 3 constant expression.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateMoreComplexMulExpression()
        {
            ExpressionTree testTree = new ExpressionTree("6*2*1");
            Assert.AreEqual(12, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the division operator with a simple 2 constant expression.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateSimpleDivExpression()
        {
            ExpressionTree testTree = new ExpressionTree("2/2");
            Assert.AreEqual(1, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the division operator with a 2 constant expression
        /// with a constant being a double.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateDivExpressionDouble()
        {
            ExpressionTree testTree = new ExpressionTree("2.5/2");
            Assert.AreEqual(1.25, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// specifically the division operator with a 3 constant expression.
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluateMoreComplexDivExpression()
        {
            ExpressionTree testTree = new ExpressionTree("6/2/1");
            Assert.AreEqual(3, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the SetVariable method in the ExpressionTree class,
        /// with a new single variable.
        /// </summary>
        [Test]
        public void TestSetVariableEmptyVariableSetValue()
        {
            ExpressionTree testTree = new ExpressionTree("a");
            testTree.SetVariable("a", 1);
            Assert.AreEqual(1, testTree.GetVariablesValues("a"));
        }

        /// <summary>
        /// Testing the SetVariable method in the ExpressionTree class,
        /// with two new variables.
        /// </summary>
        [Test]
        public void TestSetVariableSet2EmptyVariables()
        {
            ExpressionTree testTree = new ExpressionTree("a+b");
            testTree.SetVariable("a", 1);
            testTree.SetVariable("b", 2);
            Assert.AreEqual(1, testTree.GetVariablesValues("a"));
            Assert.AreEqual(2, testTree.GetVariablesValues("b"));
        }

        /// <summary>
        /// Testing the SetVariable method in the ExpressionTree class,
        /// with a variable name longer than one character.
        /// </summary>
        [Test]
        public void TestSetVariableEmptyComplexVariableSetValue()
        {
            ExpressionTree testTree = new ExpressionTree("Hello+2");
            testTree.SetVariable("Hello", 1);
            Assert.AreEqual(1, testTree.GetVariablesValues("Hello"));
        }

        /// <summary>
        /// Testing the SetVariable method in the ExpressionTree class,
        /// with a variable name longer than one character and the value is a string.
        /// </summary>
        [Test]
        public void TestSetVariableEmptyComplexVariableSetValueValueAsString()
        {
            ExpressionTree testTree = new ExpressionTree("Hello+2");
            testTree.SetVariable("Hello", "1");
            Assert.AreEqual(1, testTree.GetVariablesValues("Hello"));
        }

        /// <summary>
        /// Testing the SetVariable method in the ExpressionTree class,
        /// with a variable name longer than one character and the
        /// value is an illegal string.
        /// </summary>
        [Test]
        public void TestSetVariableEmptyComplexVariableSetValueValueAsIllegalString()
        {
            ExpressionTree testTree = new ExpressionTree("Hello+2");
            testTree.SetVariable("Hello", "A1");
            Assert.AreEqual(0, testTree.GetVariablesValues("Hello"));
        }

        /// <summary>
        /// Testing the SetVariable method in the ExpressionTree class,
        /// with a variable name longer than one character and the
        /// value is an illegal string.
        /// </summary>
        [Test]
        public void TestSetVariableAnotherIllegalStringValue()
        {
            ExpressionTree testTree = new ExpressionTree("Hello+2");
            testTree.SetVariable("Hello", "+");
            Assert.AreEqual(0, testTree.GetVariablesValues("Hello"));
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class,
        /// with only a variable as the expression.
        /// </summary>
        [Test]
        public void TestEvaluateWith1Variable()
        {
            ExpressionTree testTree = new ExpressionTree("Hello");
            testTree.SetVariable("Hello", 1);
            Assert.AreEqual(1, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// with the addition operator and a variable for the expression.
        /// </summary>
        [Test]
        public void TestEvaluateExpressionWith1Variable()
        {
            ExpressionTree testTree = new ExpressionTree("Hello+4");
            testTree.SetVariable("Hello", 1);
            Assert.AreEqual(5, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// with the addition operator and two variables for the expression.
        /// </summary>
        [Test]
        public void TestEvaluateExpressionWith2Variable()
        {
            ExpressionTree testTree = new ExpressionTree("Hello+4+World");
            testTree.SetVariable("Hello", 1);
            testTree.SetVariable("World", 10);
            Assert.AreEqual(15, testTree.Evaluate());
        }

        /// <summary>
        /// Testing the Evaluate method from the ExpressionTree class
        /// with the addition operator and two variables for the expression.
        /// </summary>
        [Test]
        public void TestEvaluateExpressionWith1SetVariable1NonSet()
        {
            ExpressionTree testTree = new ExpressionTree("Hello+4+World");
            testTree.SetVariable("Hello", 1);

            // expression could not be evaluated since World was not set
            // thus KeyNotFoundException resulting in an exception of #Name?
            Assert.Throws<System.Exception>(() => testTree.Evaluate());
        }

        /// <summary>
        /// Testing the private SetVariableNames and public
        /// GetVariableNames methods with an expression of
        /// 3 simple variables.
        /// </summary>
        [Test]
        public void TestSetAndGetVariableNamesList()
        {
            ExpressionTree testTree = new ExpressionTree("A+B+C");

            List<string> testList = testTree.GetVariableNames();
            List<string> expectedList = new List<string>();
            expectedList.Add("A");
            expectedList.Add("B");
            expectedList.Add("C");

            Assert.AreEqual(expectedList, testList);
        }

        /// <summary>
        /// Testing the private SetVariableNames and public
        /// GetVariableNames methods with an expression of
        /// 3 complex variables.
        /// </summary>
        [Test]
        public void TestSetAndGetVariableNamesListComplex()
        {
            ExpressionTree testTree = new ExpressionTree("A1+B2+C3");

            List<string> testList = testTree.GetVariableNames();
            List<string> expectedList = new List<string>();
            expectedList.Add("A1");
            expectedList.Add("B2");
            expectedList.Add("C3");

            Assert.AreEqual(expectedList, testList);
        }

        /// <summary>
        /// Testing the private SetVariableNames and public
        /// GetVariableNames methods with an expression of
        /// 3 long variables.
        /// </summary>
        [Test]
        public void TestSetAndGetVariableNamesListLong()
        {
            ExpressionTree testTree = new ExpressionTree("Hello+Beautiful+People");

            List<string> testList = testTree.GetVariableNames();
            List<string> expectedList = new List<string>();
            expectedList.Add("Hello");
            expectedList.Add("Beautiful");
            expectedList.Add("People");

            Assert.AreEqual(expectedList, testList);
        }

        // ----------------------------------------------------------------------------------------------------------------------
        // TESTING PRIVATE MEMBERS
        // ----------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Testing the private ShuntingYardAlgorithm for a simple expression, making sure output is not equal to null.
        /// </summary>
        [Test]
        public void TestShuntingYardAlgorithmSimpleExpIsNotNull()
        {
            this.privateTestTree = new ExpressionTree("1+2");

            MethodInfo methodInfo = this.GetMethod("ShuntingYardAlgorithm");

            var newObject = methodInfo.Invoke(
                this.privateTestTree,
                new object[] { });
            Assert.IsNotNull(newObject);
        }

        /// <summary>
        /// Testing the private ShuntingYardAlgorithm for a simple expression.
        /// </summary>
        [Test]
        public void TestShuntingYardAlgorithmSimpleExp()
        {
            this.privateTestTree = new ExpressionTree("1+2");

            MethodInfo methodInfo = this.GetMethod("ShuntingYardAlgorithm");

            var newObject = methodInfo.Invoke(
                this.privateTestTree,
                new object[] { });

            List<string> expectedReturn = new List<string> { "1", "2", "+" };

            Assert.AreEqual(expectedReturn, newObject);
        }

        /// <summary>
        /// Testing the private ShuntingYardAlgorithm for a variable expression.
        /// </summary>
        [Test]
        public void TestShuntingYardAlgorithmVariableExp()
        {
            this.privateTestTree = new ExpressionTree("A+B");

            MethodInfo methodInfo = this.GetMethod("ShuntingYardAlgorithm");

            var newObject = methodInfo.Invoke(
                this.privateTestTree,
                new object[] { });

            List<string> expectedReturn = new List<string> { "A", "B", "+" };

            Assert.AreEqual(expectedReturn, newObject);
        }

        /// <summary>
        /// Testing the private ShuntingYardAlgorithm for a empty expression.
        /// </summary>
        [Test]
        public void TestShuntingYardAlgorithmEmptyExp()
        {
            this.privateTestTree = new ExpressionTree(string.Empty);

            MethodInfo methodInfo = this.GetMethod("ShuntingYardAlgorithm");

            var newObject = methodInfo.Invoke(
                this.privateTestTree,
                new object[] { });

            List<string> expectedReturn = new List<string> { };

            Assert.AreEqual(expectedReturn, newObject);
        }

        /// <summary>
        /// Testing the private ShuntingYardAlgorithm for a 3 operator expression.
        /// </summary>
        [Test]
        public void TestShuntingYardAlgorithm3OpExp()
        {
            this.privateTestTree = new ExpressionTree("1+2+3+4");

            MethodInfo methodInfo = this.GetMethod("ShuntingYardAlgorithm");

            var newObject = methodInfo.Invoke(
                this.privateTestTree,
                new object[] { });

            List<string> expectedReturn = new List<string> { "1", "2", "+", "3", "+", "4", "+" };

            Assert.AreEqual(expectedReturn, newObject);
        }

        /// <summary>
        /// Testing the private ShuntingYardAlgorithm for a 3 multiplaction operator expression with constants and variables.
        /// Testing something different from + operator.
        /// </summary>
        [Test]
        public void TestShuntingYardAlgorithm3MulOpExp()
        {
            this.privateTestTree = new ExpressionTree("AB*2*CD*4");

            MethodInfo methodInfo = this.GetMethod("ShuntingYardAlgorithm");

            var newObject = methodInfo.Invoke(
                this.privateTestTree,
                new object[] { });

            List<string> expectedReturn = new List<string> { "AB", "2", "*", "CD", "*", "4", "*" };

            Assert.AreEqual(expectedReturn, newObject);
        }

        /// <summary>
        /// Testing the private ShuntingYardAlgorithm for something similar to Cell names in the spreadsheet app.
        /// </summary>
        [Test]
        public void TestShuntingYardAlgorithmSpreadsheeExp()
        {
            this.privateTestTree = new ExpressionTree("A1-2-B3-4");

            MethodInfo methodInfo = this.GetMethod("ShuntingYardAlgorithm");

            var newObject = methodInfo.Invoke(
                this.privateTestTree,
                new object[] { });

            List<string> expectedReturn = new List<string> { "A1", "2", "-", "B3", "-", "4", "-" };

            Assert.AreEqual(expectedReturn, newObject);
        }

        /// <summary>
        /// This allows me to call private methods in the ExpressionTree class so I can test them.
        /// </summary>
        /// <param name="methodName">string value, name of the private method I want to test.</param>
        /// <returns>MethodInfo.</returns>
        private MethodInfo GetMethod(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Assert.Fail("methodName cannot be null or whitespace");
            }

            var method = this.privateTestTree.GetType()
                .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

            if (method == null)
            {
                Assert.Fail(string.Format("{0} method not found", methodName));
            }

            return method;
        }
    }
}
