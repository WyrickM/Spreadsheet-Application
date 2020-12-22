// <copyright file="ExpressionTree.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadSheetEngine;

namespace CptS321
{
    /// <summary>
    /// ExpressionTree class, gets the user inputted expression and evaulates it.
    /// </summary>
    public class ExpressionTree
    {
        /// <summary>
        /// A cell class object so the tree can reference the cell.
        /// Need this to be able to set variables with the cells' values.
        /// </summary>
        public Cell parentCell;

        /// <summary>
        /// the root of the expression tree.
        /// </summary>
        private readonly ExpressionTreeNode root;

        /// <summary>
        /// dictionary that stores the names and values of the variables in the expression.
        /// </summary>
        private Dictionary<string, double> variables;

        /// <summary>
        /// private field that is a list of strings,
        /// which are all of the names of the variables
        /// in the expression.
        /// </summary>
        private List<string> variableNames;

        /// <summary>
        /// Will be the expression that the user inputs.
        /// </summary>
        private string inputExpression;

        /// <summary>
        /// We want a OperatorNodeFactory field so we can call the methods in the class.
        /// </summary>
        private OperatorNodeFactory operatorNodeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">string expression inputed by the user.</param>
        public ExpressionTree(string expression)
        {
            this.inputExpression = expression;
            this.variables = new Dictionary<string, double>();
            this.variableNames = new List<string>();
            this.operatorNodeFactory = new OperatorNodeFactory();
            this.root = this.BuildExpressionTree();
        }

        /// <summary>
        /// Gets the input expression string.
        /// </summary>
        /// <returns>the input expression the user inputted.</returns>
        public string Expression
        {
            get
            {
                return this.inputExpression;
            }
        }

        /// <summary>
        /// Sets the specific variable(key) with its value to the variables dictionary.
        /// </summary>
        /// <param name="variableName">string, name of the variable.</param>
        /// <param name="variableValue">double, value associated with the variable.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            this.variables.Add(variableName, variableValue);
        }

        /// <summary>
        /// Overriding SetVariables function that takes a string as the second parameter.
        /// Turns the second parameter (string) into a double so it can be put in the
        /// variables dictionary so it can be evaluated properly.
        /// </summary>
        /// <param name="variableName">string, name of the variable.</param>
        /// <param name="variableValue">string, value associated with the variable.</param>
        public void SetVariable(string variableName, string variableValue)
        {
            double valueAsDouble;

            if (double.TryParse(variableValue, out valueAsDouble))
            {
                if (this.variables.ContainsKey(variableName) == false)
                {
                    this.variables.Add(variableName, valueAsDouble);
                }
                else
                {
                    this.variables[variableName] = valueAsDouble;
                }
            }

            // variableValue string could not be parsed into a double
            // thus make variable value = 0
            else
            {
                if (this.variables.ContainsKey(variableName) == false)
                {
                    this.variables.Add(variableName, 0);
                }
                else
                {
                    this.variables[variableName] = 0;
                }
            }
        }

        /// <summary>
        /// Gets the value of the specifc variable.
        /// Using this function only in tests if the variables are being set properly.
        /// </summary>
        /// <param name="variableName">The name of the variable in the variables dictionary.</param>
        /// <returns>0.0 if key not in variables dictionary, otherwise the value associated with the variable.</returns>
        public double GetVariablesValues(string variableName)
        {
            // if (this.variables.ContainsKey(variableName))
            // {
            //    return this.variables[variableName];
            // }

            // using exception handling.
            try
            {
                // try block to see if dictionary contains key == variableName
                return this.variables[variableName];
            }
            catch (KeyNotFoundException)
            {
                // catch, key not found in variables dictionary.
                // aka, value for the variable in the expression was not set.
                return 0.0;
            }
        }

        /// <summary>
        /// Gets the list that contains all of the set variables
        /// in the expression.
        /// Provide easy-to-use interface for finding relevant information about the expression.
        /// </summary>
        /// <returns>list of strings, names of all variables.</returns>
        public List<string> GetVariableNames()
        {
            return this.variableNames;
        }

        /// <summary>
        /// Public Evaluate function that evaluates the expression.
        /// </summary>
        /// <returns>Double value, value that is the result/answer of the evaluated expression.</returns>
        public double Evaluate()
        {
            try
            {
                return this.Evaluate(this.root);
            }
            catch (Exception e)
            {
                // returning a double so should not catch error here,
                // catch error where we are calling this evaluate.
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// I need to unset the variables in the cell so they
        /// are not evaluated after I call undo.
        /// </summary>
        /// <param name="cell">corresponding cell.</param>
        public void UnsetCellVariable(Cell cell)
        {
            cell.PropertyChanged -= this.CellChange;
        }

        /// <summary>
        /// Private Evaluate function to protect the root of the expression tree.
        /// </summary>
        /// <param name="node">The root of the expression tree.</param>
        /// <returns>Double value, value that is the result/answer of the evaluated expression.</returns>
        private double Evaluate(ExpressionTreeNode node)
        {
            double resultOfExpression = 0.0;

            // Build an expression tree with the root as the root node.
            // Building expression tree in constructor. If change root = null in constructor
            // I need the below statement.
            // this.root = this.BuildExpressionTree();

            // Evaluate Expression from root
            if (node != null)
            {
                ExpressionTreeNode evaluateNode = node;
                try
                {
                    resultOfExpression = evaluateNode.Evaluate();
                    return resultOfExpression;
                }
                catch (Exception e)
                {
                    // returning a double so should not catch error here,
                    // catch error where we are calling this evaluate.
                    throw new Exception(e.Message);
                }
            }

            // Check this. Excel sets cell to 0 when B1+C1
            // when B1 and C1 are empty.
            // Also B1+2 = 2 when B1 is empty
            // thus when it comes to the spreadsheet cells if the
            // variable is a cell and the cell has no text then sets
            // that cells value to be 0.

            // If Variable that is not equal to a cell is not set => #Name?
            // If expression has a cell and that cell has a variable that is
            // not set then => #Value!.
            throw new NotSupportedException();
        }

        /// <summary>
        /// Shunting Yard Algorithm.
        /// Converts infix expression to postfix expression. http://math.oxford.emory.edu/site/cs171/shuntingYardAlgorithm/
        /// Converting the user inputted expression to post fix to make stuff easier in the future.
        /// </summary>
        /// <param name="inputExpression">The users inputted string expression.</param>
        /// <returns>List of strings, each constant, variable, and operator string.</returns>
        private List<string> ShuntingYardAlgorithm()
        {
            Stack<char> operatorStack = new Stack<char>();
            List<string> postfixExpressionList = new List<string>();
            int operandStart = -1;
            string varOrConst = string.Empty;
            char expressionChar = ' ';
            bool isWhiteSpace = false;

            for (int index = 0; index < this.inputExpression.Length; index++)
            {
                varOrConst = string.Empty;
                expressionChar = this.inputExpression[index];

                if (this.IsOperatorOrParenthesis(expressionChar))
                {
                    // If the incoming symbol is an operator and we had an operand started
                    // then this is the end of the oprand (to handle variables with multiple characters)
                    if (operandStart != -1 && isWhiteSpace == false)
                    {
                        string operand = this.inputExpression.Substring(operandStart, index - operandStart);
                        postfixExpressionList.Add(operand);
                        operandStart = -1;
                    }

                    // if the incoming symbol is a left parenthesis, push it on the stack
                    if (this.IsParenthesis(expressionChar) == 1)
                    {
                        operatorStack.Push(expressionChar);
                    }

                    // If the incoming symbol is a right parenthesis: discard the right parenthesis,
                    // pop and print the stack symbols until you seee a left parenthesis. Pop left
                    // parenthesis and discard it
                    else if (this.IsParenthesis(expressionChar) == 2)
                    {
                        char poppedChar = operatorStack.Pop();
                        while (this.IsParenthesis(poppedChar) != 1)
                        {
                            postfixExpressionList.Add(poppedChar.ToString());
                            poppedChar = operatorStack.Pop();
                        }
                    }

                    // If the incoming symbol is an operator and the stack is empty or contains a left parenthesis
                    // on top, push the incoming operator onto the stack.
                    else if (this.operatorNodeFactory.IsOperator(expressionChar))
                    {
                        if (operatorStack.Count == 0 || this.IsParenthesis(operatorStack.Peek()) == 1)
                        {
                            operatorStack.Push(expressionChar);
                        }

                        // If the incoming symbol is an operator and has either higher precedence than the operator
                        // on the top of the stack, or has the same precedence as the operator on the top of the stack
                        // and is right associative --> push it on the stack.
                        else if (this.IsHigherPrecedence(expressionChar, operatorStack.Peek())
                            || (this.IsSamePrecedence(expressionChar, operatorStack.Peek()) && this.IsRightAssociative(expressionChar)))
                        {
                            operatorStack.Push(expressionChar);
                        }

                        // If the incoming symbol is an operator and has either lower precedence than the operator
                        // on the top of the stack, or has the same precedence as the operator on the top of the stack
                        // and is left associtive --> continue to pop the stack until this is not true. Then, push the
                        // incoming operator on the stack.
                        else if (this.IsLowerPrecedence(expressionChar, operatorStack.Peek())
                            || (this.IsSamePrecedence(expressionChar, operatorStack.Peek()) && this.IsLeftAssociative(expressionChar)))
                        {
                            do
                            {
                                char poppedChar = operatorStack.Pop();
                                postfixExpressionList.Add(poppedChar.ToString());
                            }
                            while (operatorStack.Count > 0 && (this.IsLowerPrecedence(expressionChar, operatorStack.Peek())
                            || (this.IsSamePrecedence(expressionChar, operatorStack.Peek()) && this.IsLeftAssociative(expressionChar))));

                            operatorStack.Push(expressionChar);
                        }
                    }
                }

                // if character is a whitespace.
                else if (expressionChar == ' ')
                {
                    // do nothing.
                    isWhiteSpace = true;
                    if (operandStart != -1)
                    {
                        string operand = this.inputExpression.Substring(operandStart, index - operandStart);
                        postfixExpressionList.Add(operand);
                        operandStart = -1;
                    }
                }

                // if the character is not an operator/parenthesis then we are starting an operand
                else if (operandStart == -1)
                {
                    operandStart = index;
                    isWhiteSpace = false;
                }
            }

            // if we are at the end of the expression and we have started tracking an operand then finish it
            if (operandStart != -1)
            {
                postfixExpressionList.Add(this.inputExpression.Substring(operandStart, this.inputExpression.Length - operandStart));
                operandStart = -1;
            }

            // At the end of the expression, pop and print all operators on the stack.
            // No parenthesis should remain.
            while (operatorStack.Count > 0)
            {
                postfixExpressionList.Add(operatorStack.Pop().ToString());
            }

            return postfixExpressionList;
        }

        /// <summary>
        /// This method builds the expression tree so it can be evaluated.
        /// Calls the shunting yard algorithm to get post fix list of strings.
        /// </summary>
        /// <returns>Expression tree node. Which is the root of the tree.</returns>
        private ExpressionTreeNode BuildExpressionTree()
        {
            Stack<ExpressionTreeNode> stackOfNodes = new Stack<ExpressionTreeNode>();
            double constantValue = 0;
            ExpressionTreeNode newTreeNode = null;

            List<string> expression = this.ShuntingYardAlgorithm();

            foreach (string item in expression)
            {
                if (item != string.Empty)
                {
                    // check if the current item/string is a constant value.
                    if (double.TryParse(item, out constantValue) == true)
                    {
                        // current string is constant value, convert to constant node.
                        newTreeNode = new ConstantNode(constantValue);
                    }

                    // check if the current item is a variable
                        // checking if item is a digit
                        // checking if item is a operator by checking the dictionary in the OperatorNodeFactory,
                        // if it is an operator it should be the first/only character of the item string.
                    else if (double.TryParse(item, out constantValue) == false &&
                        this.operatorNodeFactory.TreeOperators.ContainsKey(item[0]) == false)
                    {
                        this.variableNames.Add(item);
                        newTreeNode = new VariableNode(item, ref this.variables);
                    }

                    // checking if the current item is a opearator
                    else if (this.operatorNodeFactory.IsOperator(item[0]) == true)
                    {
                        // getting the OperatorNode associated with the operator character from the
                        // OperatorNodeFactory class through the CreateNewNode method which returns
                        // an OperatorNode
                        newTreeNode = this.operatorNodeFactory.CreateNewNode(item[0]);

                        // setting the left node of the operator the the node that is on the top of the stack.
                        ((OperatorNode)newTreeNode).Right = stackOfNodes.Pop();

                        // setting the right node of the operator to the node that is on the top of the stack
                        // (second node from top of stack before this else if)
                        ((OperatorNode)newTreeNode).Left = stackOfNodes.Pop();
                    }

                    stackOfNodes.Push(newTreeNode);
                }
            }

            if (stackOfNodes.Count > 0)
            {
                return stackOfNodes.Pop();
            }

            return null;
        }

        /// <summary>
        /// This function will check if the incoming character is an operator or parenthesis character (left or right).
        /// This is a helper function that cleans up my code.
        /// </summary>
        /// <param name="expressionChar">character from the inputted user expression.</param>
        /// <returns>true or false.</returns>
        private bool IsOperatorOrParenthesis(char expressionChar)
        {
            if (this.operatorNodeFactory.IsOperator(expressionChar) == true)
            {
                return true;
            }
            else if (expressionChar == '(' || expressionChar == ')')
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// This function will check if the incoming character is a left or right parenthesis paranthesis.
        /// This is a helper function that cleans up my code.
        /// </summary>
        /// <param name="expressionChar">character from the inputted user expression.</param>
        /// <returns>1 if left, 2 if right, 0 if niether.</returns>
        private int IsParenthesis(char expressionChar)
        {
            if (expressionChar == '(')
            {
                return 1;
            }
            else if (expressionChar == ')')
            {
                return 2;
            }

            return 0;
        }

        /// <summary>
        /// This is a helper function that gets called by IsHigherPrecedence, IsSamePrecedence,
        /// IsLowerPrecedence, and IsLowerOrSamePrecedence.
        /// Compares the precedence of the left and right operators and returns an integer accordingly.
        /// </summary>
        /// <param name="left">operator character.</param>
        /// <param name="right">different operator character, could be same character symbol though.</param>
        /// <returns>-1 if left greater than right, 1 if left less than right, and 0 if left = right.</returns>
        private int ComparePrecedence(char left, char right)
        {
            uint leftPrecedence = this.operatorNodeFactory.GetPrecedence(left);
            uint rightPrecedence = this.operatorNodeFactory.GetPrecedence(right);

            return (leftPrecedence > rightPrecedence) ? -1 : (leftPrecedence < rightPrecedence) ? 1 : 0;
        }

        /// <summary>
        /// This function compares the outcome of ComparePrecedence with 0.
        /// </summary>
        /// <param name="expressionChar">character from the inputted user expression.</param>
        /// <param name="poppedChar">character from top of stack.</param>
        /// <returns>true if ComparePrecedence is greater than 0.</returns>
        private bool IsHigherPrecedence(char expressionChar, char poppedChar)
        {
            return this.ComparePrecedence(expressionChar, poppedChar) > 0;
        }

        /// <summary>
        /// This function compares the outcome of ComparePrecedence with 0.
        /// </summary>
        /// <param name="expressionChar">character from the inputted user expression.</param>
        /// <param name="poppedChar">character from top of stack.</param>
        /// <returns>true if ComparePrecedence is equal to 0.</returns>
        private bool IsSamePrecedence(char expressionChar, char poppedChar)
        {
            return this.ComparePrecedence(expressionChar, poppedChar) == 0;
        }

        /// <summary>
        /// This function compares the outcome of ComparePrecedence with 0.
        /// </summary>
        /// <param name="expressionChar">character from the inputted user expression.</param>
        /// <param name="poppedChar">character from top of stack.</param>
        /// <returns>true if ComparePrecedence is less than 0.</returns>
        private bool IsLowerPrecedence(char expressionChar, char poppedChar)
        {
            return this.ComparePrecedence(expressionChar, poppedChar) < 0;
        }

        /// <summary>
        /// This function compares the outcome of ComparePrecedence with 0.
        /// </summary>
        /// <param name="expressionChar">character from the inputted user expression.</param>
        /// <param name="poppedChar">character from top of stack.</param>
        /// <returns>true if ComparePrecedence is less than or equal to 0.</returns>
        private bool IsLowerOrSamePrecedence(char expressionChar, char poppedChar)
        {
            return this.ComparePrecedence(expressionChar, poppedChar) <= 0;
        }

        /// <summary>
        /// This function checks the associativity of the operator character.
        /// </summary>
        /// <param name="expressionChar">character from the inputted user expression.</param>
        /// <returns>true if the operator character associativity is right associative.</returns>
        private bool IsRightAssociative(char expressionChar)
        {
            return this.operatorNodeFactory.GetAssociativity(expressionChar) == OperatorNode.Associative.Right;
        }

        /// <summary>
        /// This function checks the associativity of the operator character.
        /// </summary>
        /// <param name="expressionChar">character from the inputted user expression.</param>
        /// <returns>true if the operator character associativity is left associative.</returns>
        private bool IsLeftAssociative(char expressionChar)
        {
            return this.operatorNodeFactory.GetAssociativity(expressionChar) == OperatorNode.Associative.Left;
        }

        /// <summary>
        /// Sets the variable name associated with the cell name
        /// from the spreadsheet. Uses an event to change the cell.
        /// </summary>
        /// <param name="cell">cell that has its value being changed.</param>
        public void SetCellVariableInTree(Cell cell)
        {
            cell.PropertyChanged += this.CellChange;

            if (this.variableNames.Contains(cell.CellName))
            {
                this.SetVariable(cell.CellName, cell.Value);
            }
        }

        /// <summary>
        /// This function is an event that gets called by
        /// the SetCellVariable function. It updates
        /// all of the other cells that are linked to a cell
        /// when that linked cell's value changes.
        /// </summary>
        /// <param name="sender">sender, cell object.</param>
        /// <param name="e">e, event.</param>
        private void CellChange(object sender, EventArgs e)
        {
            Cell copyCell = sender as Cell;

            if (this.variables.ContainsKey(copyCell.CellName))
            {
                this.SetVariable(copyCell.CellName, copyCell.Value);
            }

            if (this.parentCell.ListVariableNames.Contains(copyCell.CellName))
            {
                // Updating the value of the cell by evaluating the new expression
                this.parentCell.Value = this.Evaluate().ToString();
            }
        }
    }
}
