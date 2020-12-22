// <copyright file="TextChange.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CptS321
{
    /// <summary>
    /// Text change class that inherits from the command interface.
    /// Changes the text of the specific cell.
    /// </summary>
    public class TextChange : ICommand
    {
        /// <summary>
        /// The original text from the cell.
        /// </summary>
        private string originalText;

        /// <summary>
        /// The new text that we are changing the text to.
        /// </summary>
        private string newText;

        /// <summary>
        /// The spreadsheet that the cell is in.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// The specific cell of the spreadsheet that the text is being changed in.
        /// </summary>
        private Cell cell;

        /// <summary>
        /// DataGridViewCellEventArgs to be able to call CellPropertyChanged from the spreadsheet.
        /// </summary>
        private DataGridViewCellEventArgs e;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextChange"/> class.
        /// </summary>
        /// <param name="originalText">original text of the cell.</param>
        /// <param name="newText">new text of the cell.</param>
        /// <param name="spreadsheet">the spreadsheet.</param>
        /// <param name="e">DataGridViewCellEventArgs.</param>
        public TextChange(string originalText, string newText, Spreadsheet spreadsheet, DataGridViewCellEventArgs e)
        {
            this.originalText = originalText;
            this.newText = newText;
            this.spreadsheet = spreadsheet;
            this.e = e;
            this.cell = this.spreadsheet.GetCell(this.e.RowIndex, this.e.ColumnIndex);
        }

        /// <summary>
        /// overriding the interface command class execute function
        /// to execute the text change.
        /// Called by Redo in spreadsheet class.
        /// Need the CellPropertyChanged function so if there is an
        /// expression that is suppose to be evaluated it gets evaluated.
        /// </summary>
        public void Execute()
        {
            this.cell.Text = this.newText;
            this.spreadsheet.CellPropertyChanged(this.cell, this.e);
        }

        /// <summary>
        /// overriding the interface command class unexecute function
        /// this will be used for undo.
        /// Need the CellPropertyChanged function so if there is an
        /// expression that is suppose to be evaluated it gets evaluated properly.
        /// </summary>
        public void Unexecute()
        {
            this.cell.Text = this.originalText;
            this.spreadsheet.CellPropertyChanged(this.cell, this.e);
        }
    }
}
