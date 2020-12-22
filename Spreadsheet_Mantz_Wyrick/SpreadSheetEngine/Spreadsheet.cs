// <copyright file="Spreadsheet.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Spreadsheet class.
    /// </summary>
    public class Spreadsheet
    {
        /// <summary>
        /// 2D array of cells.
        /// This is the entity that actually creates all the cells in the spreadsheet.
        /// </summary>
        private Cell[,] spreadsheetCells;

        /// <summary>
        /// This is a private dictionary that is used to get the numeric value of the column
        /// that is associated with the letter label of the column.
        /// </summary>
        private Dictionary<string, int> cellLocationName;

        /// <summary>
        /// The spreadsheet's workbook object that contains methods:
        ///     undo/redo
        ///     save/load.
        /// </summary>
        private Workbook spreadsheetWorkbook;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="numberRows">Int value, number of rows.</param>
        /// <param name="numberColumns">Int value, number of columns.</param>
        public Spreadsheet(int numberRows, int numberColumns)
        {
            this.spreadsheetCells = new CellInheritedClass[numberRows, numberColumns]; // instantiating the spreadsheetCells 2D array.
            this.RowCount = numberRows; // setting the number of rows.
            this.ColumnCount = numberColumns; // setting the number of columns.
            this.cellLocationName = new Dictionary<string, int>();
            this.SetDictionary();
            this.InstantiateCells(numberRows, numberColumns);
            this.spreadsheetWorkbook = new Workbook(this.spreadsheetCells);
        }

        /// <summary>
        /// This allows the UI datagridview to change the cells of its spreadsheet.
        /// UI spreadsheet is a spreadsheet class object thus need to be able to notify it.
        /// Forms1.cs has a handler method to handle the changes.
        /// same as "public event PropertyChangedEventHandler PropertyChanged = delegate { };" just lambda notation.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets or Sets the number of rows in the spreadsheet.
        /// The number of rows are set in the constructor.
        /// </summary>
        public int RowCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the number of columns in the spreadsheet.
        /// The number of columns are set in the constructor.
        /// </summary>
        public int ColumnCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a copy of the private dictionay field.
        /// Using this function to test the SetDictionary method.
        /// </summary>
        public Dictionary<string, int> GetDictionary
        {
            get
            {
                return this.cellLocationName;
            }
        }

        /// <summary>
        /// Gets the workbook to be able to clear the undo
        /// and redo stacks.
        /// </summary>
        public Workbook Workbook
        {
            get
            {
                return this.spreadsheetWorkbook;
            }
        }

        /// <summary>
        /// This method updates the spread sheet if the cell changed by updating the Value in the cell.
        /// Calls GetTextFromCell to get the current text/value from the cell.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">EventArgs, e.</param>
        public void CellPropertyChanged(object sender, EventArgs e)
        {
            Cell copyCell = sender as Cell;

            if (copyCell.Text.Length == 0 || copyCell.Text[0] != '=')
            {
                copyCell.Value = copyCell.Text; // Using setter function for value

                //// Have to unsubscribe cells from each other so that they are not updated by the expression tree after already hitting undo
                if (copyCell.ListVariableNames != null && copyCell.ListVariableNames.Count > 0)
                {
                    foreach (string variable in copyCell.ListVariableNames)
                    {
                        copyCell.UnsetCellVariable(this.GetCell(variable));
                    }
                }

                this.PropertyChanged(copyCell, new PropertyChangedEventArgs("Text"));
            }
            else
            {
                try
                {
                    copyCell.NewExpression(copyCell.Text.Substring(1));
                    this.CheckCellReference(copyCell);
                    foreach (string variable in copyCell.ListVariableNames)
                    {
                        copyCell.SetCellVariable(this.GetCell(variable));
                    }

                    copyCell.Value = copyCell.EvaluateExpression().ToString();
                }
                catch (Exception exception)
                {
                    copyCell.Value = exception.Message;
                }
            }

            // need this so the datagridview can modify its spreadsheet. Form1.cs has handler function to handle change.
            this.PropertyChanged(this, new PropertyChangedEventArgs(copyCell.RowIndex.ToString() + ","
                        + copyCell.ColumnIndex.ToString() + "," + copyCell.Value));
        }

        /// <summary>
        /// This method returns the cell at the location specified by parameters.
        /// Returns null if there is no cell.
        /// </summary>
        /// <param name="rowIndex">Int value, row index.</param>
        /// <param name="columnIndex">Int value, column index.</param>
        /// <returns>Cell type, specified cell at the location based on parameters.</returns>
        public Cell GetCell(int rowIndex, int columnIndex)
        {
            if (this.RowCount < rowIndex || this.ColumnCount < columnIndex)
            {
                // if the row index or column index is out of bounds of the spreadsheet 2D array.
                // throw a bad reference message exception.
                throw new Exception();
            }

            // returning the cell that is located at rowIndex columnIndex.
            return this.spreadsheetCells[rowIndex, columnIndex];
        }

        /// <summary>
        /// Gets the cell that is associated with the name of the cell.
        ///
        /// If the cell reference is a bad reference throw an exception.
        /// </summary>
        /// <param name="nameOfCell">string value, name of the cell.</param>
        /// <returns>cell object, the cell associated with the name being passed in.</returns>
        public Cell GetCell(string nameOfCell)
        {
            int rowIndex = 0, columnIndex = 0;
            try
            {
                // nameOfCell format "A1", "A10"
                // it is no longer "=A1", "=A10"
                rowIndex = Convert.ToInt32(nameOfCell.Substring(1)) - 1; // [1].ToString()) - 1; what i originally had before implement expression tree with spreadsheet.

                // getting first character (letter) from the string, converting to string, then looking for that "letter" in the dictionary
                // that keeps track of the column that the letter is in.
                columnIndex = this.cellLocationName[nameOfCell[0].ToString()];
            }
            catch (Exception)
            {
                throw new Exception("!(bad reference)");
            }

            // Takes both of those numbers sets them to rowIndex and columnIndex respectively,
            // then set as parameters for GetCell to get posistion of the cell.
            try
            {
                Cell theCell = this.GetCell(rowIndex, columnIndex);
                return theCell;
            }
            catch (Exception)
            {
                throw new Exception("!(bad reference)");
            }
        }

        /// <summary>
        /// public function that gives the ability to add and execute undo commands
        /// via the spreadsheets workbook object that calls AddToUndoStack.
        /// </summary>
        /// <param name="command">ICommand command that gets pushed to undoStack.</param>
        public void AddUndo(ICommand command)
        {
            this.spreadsheetWorkbook.AddToUndoStack(command);
            this.PropertyChanged(command, new PropertyChangedEventArgs("Undo"));
        }

        /// <summary>
        /// Undo function that performs Unexecute to undo the most
        /// recent change to the spreadsheet. Then pushes that change
        /// to the redo stack.
        ///
        /// Utilizes the spreadsheets workbook object to call the workbook's
        /// Undo method.
        /// </summary>
        public void Undo()
        {
            this.spreadsheetWorkbook.Undo();

            // This checks the undo and redo stacks to see if empty
            // if empty do not enable redo or undo menu strip buttons.
            this.PropertyChanged(this.spreadsheetWorkbook.RedoStack.Peek(), new PropertyChangedEventArgs("Undo"));
        }

        /// <summary>
        /// Redo function that perfoms Execute to redo the most recent
        /// undo action to the spreadsheet. Then pushes that change to the
        /// undo stack.
        ///
        /// Utilizes the spreadsheets workbook object to call the workbook's
        /// Redo method.
        /// </summary>
        public void Redo()
        {
            this.spreadsheetWorkbook.Redo();

            // This checks the undo and redo stacks to see if empty
            // if empty do not enable redo or undo menu strip buttons.
            this.PropertyChanged(this.spreadsheetWorkbook.UndoStack.Peek(), new PropertyChangedEventArgs("Redo"));
        }

        /// <summary>
        /// Gets the count of the undo stack so I can
        /// check to make sure the stack is empty or not.
        ///
        /// Gets the undo stack form the spreadsheet's workbook
        /// object undo stack getter funtion.
        /// </summary>
        /// <returns>int count of the undo stack.</returns>
        public int GetCountUndoStack()
        {
            return this.spreadsheetWorkbook.UndoStack.Count();
        }

        /// <summary>
        /// Gets the count of the redo stack so I can
        /// check to make sure the stack is empty or not.
        ///
        /// Gets the redo stack form the spreadsheet's workbook
        /// object redo stack getter funtion.
        /// </summary>
        /// <returns>int count of the redo stack.</returns>
        public int GetCountRedoStack()
        {
            return this.spreadsheetWorkbook.RedoStack.Count();
        }

        /// <summary>
        /// Get the top element of the stack so I can
        /// customize the undo button depending on the
        /// ICommand return type.
        ///
        /// Gets the top of the undo stack by using the
        /// spreadsheet's workbook object.
        /// </summary>
        /// <returns>ICommand.</returns>
        public ICommand PeekUndoStack()
        {
            if (this.spreadsheetWorkbook.UndoStack.Count > 0)
            {
                return this.spreadsheetWorkbook.UndoStack.Peek();
            }

            return null;
        }

        /// <summary>
        /// Get the top element of the stack so I can
        /// customize the redo button depending on the
        /// ICommand return type.
        ///
        /// Gets the top of the redo stack by using the
        /// spreadsheet's workbook object.
        /// </summary>
        /// <returns>ICommand.</returns>
        public ICommand PeekRedoStack()
        {
            if (this.spreadsheetWorkbook.RedoStack.Count > 0)
            {
                return this.spreadsheetWorkbook.RedoStack.Peek();
            }

            return null;
        }

        /// <summary>
        /// Saves the spreadsheet to a file by calling the
        /// spreadsheet's workbook object.
        /// </summary>
        /// <param name="stream">file saving spreadsheet to.</param>
        public void SaveSpreadsheet(Stream stream)
        {
            this.spreadsheetWorkbook.Save(stream);
        }

        /// <summary>
        /// Loads the spreadsheet from a file by calling the
        /// spreadsheet's workbook object.
        /// </summary>
        /// <param name="stream">file loading spreadsheet from.</param>
        /// <returns>List of loaded cells.</returns>
        public List<Cell> LoadSpreadsheet(StreamReader stream)
        {
            this.ClearSheet(this.RowCount, this.ColumnCount);
            return this.spreadsheetWorkbook.Load(stream);
        }

        /// <summary>
        /// This function checks to see if a cell reference is valid.
        /// </summary>
        /// <param name="cell">cell.</param>
        /// <returns>true if valid reference, throws error depending on error.</returns>
        private bool CheckCellReference(Cell cell)
        {
            foreach (string variable in cell.ListVariableNames)
            {
                if (variable == cell.CellName)
                {
                    throw new Exception("!(self reference)");
                }
                else if (this.IsCircularReference(variable, cell.CellName) == true)
                {
                    throw new Exception("!(circular reference)");
                }
            }

            return true;
        }

        /// <summary>
        /// This function checks to see if there are any circular references.
        /// </summary>
        /// <param name="cellReference">cell referenced in the parent cell.</param>
        /// <param name="cellName">original parent cell.</param>
        /// <returns>true or false.</returns>
        private bool IsCircularReference(string cellReference, string cellName)
        {
            Cell checkCell = this.GetCell(cellReference);

            foreach (string variable in checkCell.ListVariableNames)
            {
                if (variable == cellName)
                {
                    return true;
                }

                // trying to edit a cell referencing a cell that is self referencing
                // before fixing the self referenced cell.
                else if (variable == checkCell.CellName)
                {
                    return false;
                }

                return this.IsCircularReference(variable, cellName);
            }

            return false;
        }

        /// <summary>
        /// This function will be used in CellPropertyChanged method to find the location of a cell.
        /// This function calls GetCell function to get the cell that we want the text from.
        /// Use this because the cell we need to find will be given by its name.
        /// For example, "A1", "C10", or "H49".
        /// </summary>
        /// <param name="nameOfCell">string value, name of the cell.</param>
        /// <returns>string value, the text/value in the cell.</returns>
        private string GetTextFromCell(string nameOfCell)
        {
            Cell cell = this.GetCell(nameOfCell);
            string textFromCell = cell.Text;
            return textFromCell;
        }

        /// <summary>
        /// This function will assign a number association of the columns of.
        /// </summary>
        private void SetDictionary()
        {
            int columnIndex = 0;

            // setting the first column "A" to column index 0.
            // since the main spreadsheet's first column that labels the rows is the header row so it should still start at row 0.
            for (int letterAlphabet = 65; letterAlphabet <= 90; letterAlphabet++, columnIndex++)
            {
                this.cellLocationName.Add(((char)letterAlphabet).ToString(), columnIndex);
            }
        }

        /// <summary>
        /// This method will instantiate all of the cells in the spreadsheet.
        /// </summary>
        /// <param name="numberRows">Int value, total number of rows.</param>
        /// <param name="numberColumns">Int value, total number of columns.</param>
        private void InstantiateCells(int numberRows, int numberColumns)
        {
            for (int rowIndex = 0; rowIndex < numberRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < numberColumns; columnIndex++)
                {
                    // this is intializing all of the spreadsheet objects with specific rows and columns
                    // for this assignment only intializing spreadsheet to be the same as the UI spreadsheet
                    // 50 rows and 26 columns, so the spreadsheet cells can be transfered to the UI cells.
                    this.spreadsheetCells[rowIndex, columnIndex] = new CellInheritedClass(rowIndex, columnIndex);

                    // I don't think I need this, but keeping it to keep consistency with property changes.
                    this.spreadsheetCells[rowIndex, columnIndex].PropertyChanged += this.Spreadsheet_PropertyChanged;
                }
            }
        }

        /// <summary>
        /// Responsible for notifying other cells of the
        /// spreadsheet of the property change.
        /// </summary>
        /// <param name="sender">object sender.</param>
        /// <param name="e">PropertyChangedEventArgs, e.</param>
        private void Spreadsheet_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Cell copyCell = sender as Cell;

            if (e.PropertyName == "Value")
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(copyCell.RowIndex.ToString() + ","
                    + copyCell.ColumnIndex.ToString() + "," + copyCell.Value));
                return;
            }
            else if (e.PropertyName == "BGColor")
            {
                this.PropertyChanged("BGColor", new PropertyChangedEventArgs(copyCell.RowIndex.ToString() + ","
                    + copyCell.ColumnIndex.ToString() + "," + copyCell.BGColor));
                return;
            }
        }

        /// <summary>
        /// This is a function that does the same saving functionality
        /// as the user interaction function Save.
        /// Testing this function is equivalent to testing the UI saving method
        /// since they both call Save in Workbook class.
        /// </summary>
        /// <param name="textWriter">a stream that links to a file.</param>
        private void SaveToFileTest(Stream textWriter)
        {
            if (textWriter.Length > 0)
            {
                // file is not empty thus need to override the whole file, thus need to clear file.
                // clearing the file by making the length of the file = 0.
                textWriter.SetLength(0);
            }

            // Write the text in the texbox to the file
            this.spreadsheetWorkbook.Save(textWriter);

            // Close the stream
            textWriter.Close();
        }

        /// <summary>
        /// This is a function that does the same loading functionality
        /// as the user interaction function Load.
        /// Testing this function is equivalent to testing the UI loading method
        /// since they both call Load in Workbook class.
        /// </summary>
        /// <param name="testFileStream">a stream that links to a file.</param>
        private void LoadFromFileTest(StreamReader testFileStream)
        {
            this.ClearSheet(this.RowCount, this.ColumnCount);
            this.spreadsheetWorkbook.Load(testFileStream);
        }

        /// <summary>
        /// "Clearing" all the spreadsheet data for the tests.
        /// The UI spreadsheet will be cleared in a clear function
        /// in the forms.cs file.
        /// </summary>
        /// <param name="numberRows">Int value, number of rows.</param>
        /// <param name="numberColumns">Int value, number of columns.</param>
        private void ClearSheet(int numberRows, int numberColumns)
        {
            this.spreadsheetCells = new CellInheritedClass[numberRows, numberColumns]; // instantiating the spreadsheetCells 2D array.
            this.RowCount = numberRows; // setting the number of rows.
            this.ColumnCount = numberColumns; // setting the number of columns.
            this.cellLocationName = new Dictionary<string, int>();
            this.SetDictionary();
            this.InstantiateCells(numberRows, numberColumns);
            this.spreadsheetWorkbook = new Workbook(this.spreadsheetCells);
        }

        /// <summary>
        /// Private class that inherits from the abstract Cell class.
        /// Only accesible to the Spreadsheet class since it is in the spreadsheet class as a private class.
        /// </summary>
        private class CellInheritedClass : Cell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CellInheritedClass"/> class.
            /// base: Initializes a new instance of the cell class.
            /// </summary>
            /// <param name="rowIndex">Int value, row index.</param>
            /// <param name="columnIndex">Int value, columnIndex.</param>
            public CellInheritedClass(int rowIndex, int columnIndex)
                : base(rowIndex, columnIndex)
            {
            }
        }
    }
}
