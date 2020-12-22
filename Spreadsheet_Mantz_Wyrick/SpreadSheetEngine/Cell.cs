// <copyright file="Cell.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CptS321
{
    /// <summary>
    /// Abstract Cell class that inherits the INotifyPropertyChanged.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// Protected string member that represents the actual text that's typed in the cell.
        /// </summary>
        protected string text;

        /// <summary>
        /// Protected string member that represents the "evaluated" value of the cell.
        /// It will just be the Text property if the text doesn't start with the '=' character.
        /// Otherwise it will represent the evaluation of the formula that's typed in the cell.
        /// </summary>
        protected string value;

        /// <summary>
        /// Declaring a expression tree field for when a cell wants to evaulate an expression.
        /// </summary>
        private ExpressionTree cellTree;

        /// <summary>
        /// This is a field that allows the cell class to see what
        /// variables are used in the expression that is in the cell.
        /// </summary>
        private List<string> listVariableNames;

        /// <summary>
        /// Setting the background color of the cell to default to white.
        /// </summary>
        private uint bgColor = 0xFFFFFFFF;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// Sets values the getters to return rowIndex and columnIndex.
        /// </summary>
        /// <param name="rowIndex">Int value, row index of cell.</param>
        /// <param name="columnIndex">Int value, cell index of cell.</param>
        public Cell(int rowIndex, int columnIndex)
        {
            this.text = string.Empty;
            this.value = string.Empty;
            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
            this.listVariableNames = new List<string>();
        }

        /// <summary>
        /// Declaring the event.
        /// Delegate that notifies the Cell class that the text string was changed.
        /// Same as:
        /// public event PropertyChangedEventHandler PropertyChanged = delegate { };.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets row index.
        /// Read only method.
        /// Set in the constructor and returned through the get.
        /// </summary>
        public int RowIndex
        {
            get;
        }

        /// <summary>
        /// Gets column index.
        /// Read only method.
        /// Set in the constructor and returned through the get.
        /// </summary>
        public int ColumnIndex
        {
            get;
        }

        /// <summary>
        /// Gets or Sets the text member.
        /// If the text member gets changed, notify the property changed.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text == value)
                {
                    return; // ignore it
                }

                this.text = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("Text"));
            }
        }

        /// <summary>
        /// Gets or Sets the background color of the cell.
        /// If the background text gets changed than notify the perperty changed event.
        /// </summary>
        public uint BGColor
        {
            get
            {
                return this.bgColor;
            }

            set
            {
                if (this.bgColor == value)
                {
                    return; // ignore it
                }

                this.bgColor = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("BGColor"));
            }
        }

        /// <summary>
        /// Gets or Sets value element.
        /// Read only method.
        /// Somehow let the spreadsheet class set the value, but no other class.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }

            protected internal set
            {
                if (this.value != value)
                {
                    this.value = value;
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }

        /// <summary>
        /// Gets the name of the cell ex. "A1".
        /// </summary>
        public string CellName
        {
            get
            {
                double asciiColumn = this.ColumnIndex + 65;
                return ((char)asciiColumn).ToString() + (this.RowIndex + 1).ToString();
            }
        }

        /// <summary>
        /// Gets or Sets the list VariableNames so it can be passed to a cell.
        /// </summary>
        /// <returns>list, names of variables in expression.</returns>
        public List<string> ListVariableNames
        {
            get
            {
                return this.listVariableNames;
            }

            protected internal set
            {
                this.listVariableNames = value;
            }
        }

        /// <summary>
        ///  Cell function that calls uses the expression tree version
        ///  to set a variable in the expression tree.
        /// </summary>
        /// <param name="cell">cell, corresponding cell.</param>
        public void SetCellVariable(Cell cell)
        {
            this.cellTree.SetCellVariableInTree(cell);
        }

        /// <summary>
        ///  Cell function that calls uses the expression tree version
        ///  to unset a variable in the expression tree. So the expression
        ///  does not get evaluated after being undone.
        /// </summary>
        /// <param name="cell">cell, corresponding cell.</param>
        public void UnsetCellVariable(Cell cell)
        {
            this.cellTree.UnsetCellVariable(cell);
        }

        /// <summary>
        /// Creates a new expression tree to be evaluated.
        /// </summary>
        /// <param name="expression">string, a string that can be evaluated by the expression tree.</param>
        public void NewExpression(string expression)
        {
            this.cellTree = new ExpressionTree(expression);
            this.listVariableNames = this.cellTree.GetVariableNames();

            // setting parentCell in the cell's expression tree
            // getting all of the attributes of this specific cell.
            this.cellTree.parentCell = this;
        }

        /// <summary>
        /// This function evlauates a given expression of a cell and returns
        /// as a string instead of a double.
        /// </summary>
        /// <returns>string, result of the evaluated expression.</returns>
        public string EvaluateExpression()
        {
            return this.cellTree.Evaluate().ToString();
        }
    }
}
