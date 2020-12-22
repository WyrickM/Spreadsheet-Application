// <copyright file="Form1.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CptS321;

namespace Spreadsheet_Mantz_Wyrick
{
    /// <summary>
    /// Form1 class that inherits from Form.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// spreadsheet object member variable for main form's code.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Loads the spreadsheet with proper amount of rows and columns.
        /// Labels the headers for rows and columns as well.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">EventArgs e.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            if (this.dataGridView1.ColumnCount != 0)
            {
                this.dataGridView1.Columns.Clear();
            }

            for (int letterAlphabet = 65; letterAlphabet <= 90; letterAlphabet++)
            {
                // starting at ascii 65 because "A" = 65, ending at 90 because "Z" = 90
                // using ints so I can increment, just need to convert to Chars and then to strings to put as columns.
                // setting the column name and headerText to the letter of the alphabet ("A"-"Z")
                this.dataGridView1.Columns.Add(((char)letterAlphabet).ToString(), ((char)letterAlphabet).ToString());
            }

            this.dataGridView1.RowCount = 50;
            for (int rowIndex = 0; rowIndex < 50; rowIndex++)
            {
                // creating 50 rows that are numbered 1-50 for the dataGridView1
                this.dataGridView1.Rows[rowIndex].HeaderCell.Value = (rowIndex + 1).ToString();
            }

            // intializing spreadsheet to have 50 rows and 26 columns.
            this.spreadsheet = new Spreadsheet(50, 26);
            this.spreadsheet.PropertyChanged += this.HandleCellPropertyChangedEvent;
            this.undoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// This happens when a cell content is clicked.
        /// No code yet.
        /// </summary>
        /// <param name="sender">Object, sender.</param>
        /// <param name="e">DataGridViewCellEventArgs, e.</param>
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        /// <summary>
        /// This button click fills 50 random cells and columns A and B as specified.
        /// </summary>
        /// <param name="sender">object sender, is the performDemoButton.</param>
        /// <param name="e">EventArgs e, is the button click.</param>
        private void PerfomDemoButton_Click(object sender, EventArgs e)
        {
            this.Random50Cell(sender, e);

            // To see this work properly go to HW6_Branch
            // the expression tree evaluator makes all of column
            // A's cells 0 since B column cells are just text.
            this.ColumnASetToB(sender, e);
        }

        /// <summary>
        /// Subscribing to the spreadsheet's CellPropertyChanged event.
        /// When the cell's Value changes it gets updated in the cell in the DataGridView.
        /// </summary>
        /// <param name="sender">object sender, cell that is changing.</param>
        /// <param name="e">PropertyChangedEventArgs e.</param>
        private void HandleCellPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            if (sender.GetType() == typeof(Spreadsheet))
            {
                string propertyEventName = e.PropertyName;
                string[] cellAttributes = propertyEventName.Split(',');

                int cellRow = Convert.ToInt32(cellAttributes[0]);
                int cellColumn = Convert.ToInt32(cellAttributes[1]);
                string cellValue = cellAttributes[2];

                this.dataGridView1.Rows[cellRow].Cells[cellColumn].Value = cellValue;
            }

            // check if the cell property change is setter for "BGColor"
            else if (sender is "BGColor")
            {
                string propertyEventName = e.PropertyName;
                string[] cellAttributes = propertyEventName.Split(',');

                int cellRow = Convert.ToInt32(cellAttributes[0]);
                int cellColumn = Convert.ToInt32(cellAttributes[1]);
                string cellValue = cellAttributes[2];

                // parsing to get the uint color corresponding.
                if (uint.TryParse(cellValue, out uint colorResult))
                {
                    // guided by assignment prompt.
                    this.dataGridView1.Rows[cellRow].Cells[cellColumn].Style.BackColor = Color.FromArgb(unchecked((int)colorResult));
                }
            }

            // check if the sender is an ICommand
            else if (sender is ICommand)
            {
                if (this.spreadsheet.GetCountUndoStack() > 0)
                {
                    this.undoToolStripMenuItem.Enabled = true;
                }

                if (this.spreadsheet.GetCountRedoStack() > 0)
                {
                    this.redoToolStripMenuItem.Enabled = true;
                }

                if (this.spreadsheet.GetCountUndoStack() <= 0)
                {
                    this.undoToolStripMenuItem.Enabled = false;
                }

                if (this.spreadsheet.GetCountRedoStack() <= 0)
                {
                    this.redoToolStripMenuItem.Enabled = false;
                }
            }
        }

        /// <summary>
        /// This method generates 50 random cells that will have the text "Hello World" written in them.
        /// </summary>
        /// /// <param name="sender">Object sender.</param>
        /// <param name="e">EventArgs, e.</param>
        private void Random50Cell(object sender, EventArgs e)
        {
            Random randomVariable = new Random();
            List<int> randomRowList = new List<int>();
            List<int> randomColumnList = new List<int>();
            for (int i = 1; i <= 50;)
            {
                int randomRow = randomVariable.Next(0, 49);
                int randomColumn = randomVariable.Next(0, 25);
                if (!randomRowList.Contains(randomRow) || !randomColumnList.Contains(randomColumn))
                {
                    // add "Hello World" message to cell.
                    this.spreadsheet.GetCell(randomRow, randomColumn).Text = "Hello World";
                    this.spreadsheet.CellPropertyChanged(this.spreadsheet.GetCell(randomRow, randomColumn), e);

                    randomRowList.Add(randomRow);
                    randomColumnList.Add(randomColumn);
                    i++;
                }
            }
        }

        /// <summary>
        /// This method sets the B column and the sets the A column to be equal to the correspond cells of B.
        /// Set every cell in column B to "This is cell B#" where # is the number of the row.
        /// Then set every cell in column A to "=B#" where # is the number of the row.
        /// CellPropertyChanged then converts the text of all the cells in A to the same as the cells in B,
        /// because of the "=" being the first character condition in CellPropertyChanged method.
        /// </summary>
        /// <param name="sender">object sender.</param>
        /// <param name="e">EventArgs e.</param>
        private void ColumnASetToB(object sender, EventArgs e)
        {
            int columnBIndex = 1, columnAIndex = 0;
            for (int rowIndex = 0, rowLabel = 1; rowIndex < 50; rowIndex++, rowLabel++)
            {
                // first set the text of every cell in column B to "This is cell B#" where # is the number of the row.
                this.spreadsheet.GetCell(rowIndex, columnBIndex).Text = "This is cell B" + rowLabel;
                this.spreadsheet.CellPropertyChanged(this.spreadsheet.GetCell(rowIndex, columnBIndex), e);
            }

            for (int rowIndex = 0, rowLabel = 1; rowIndex < 50; rowIndex++, rowLabel++)
            {
                // then set the text of every cell in column B to "=B#" where # is the number of the row.
                this.spreadsheet.GetCell(rowIndex, columnAIndex).Text = "=B" + rowLabel;

                // since the text of every cell in column A starts with "=", go into CellPropertyChenged method
                // and set the text to be equal to the text in B# where # is the number of the column.
                this.spreadsheet.CellPropertyChanged(this.spreadsheet.GetCell(rowIndex, columnAIndex), e);
            }
        }

        /// <summary>
        /// Event when the user starts editing a cell.
        /// Cell value should change to the Text property of the cell.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">cell that is being changed.</param>
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // changing the Value object of the changing cell to text
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }

        /// <summary>
        /// Event when the user finishes editing a cell.
        /// Cell must go back to Value of the cell.
        /// Thus need to call CellPropertyChanged from spreadsheet to
        /// check if the text starts with '=', which will determine if
        /// the value will be the same or different from text of the cell.
        /// </summary>
        /// <param name="sender">dataGridView.</param>
        /// <param name="e">cell that is being changed.</param>
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGrid = sender as DataGridView;
            Cell copyOfCell = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex); // get the exact cell we want.

            // Check to see if the value for that cell is null
            if (dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
            {
                copyOfCell.Text = string.Empty;
            }

            // Value is not null thus set the text of the spreadsheet to the value dataGridView that was changed in CellBeginEdit.
            // Then call CellPropertyChanged to change Value of that cell in the spreadsheet to either the appropriate value or text.
            else
            {
                string originalText = copyOfCell.Text;
                string originalValue = copyOfCell.Value;

                copyOfCell.Text = dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                this.spreadsheet.CellPropertyChanged(copyOfCell, e);

                TextChange changedText = new TextChange(originalText, copyOfCell.Text, this.spreadsheet, e);
                this.spreadsheet.AddUndo(changedText);
            }
        }

        /// <summary>
        /// Function when the user wants to change the background color of the cell from the menu strip.
        /// Changes the background color of all the highlighted cells.
        /// </summary>
        /// <param name="sender">sender, change background color menu strip click.</param>
        /// <param name="e">menu click.</param>
        private void ChangeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Cell> selectedCells = new List<Cell>();
            List<uint> originalColors = new List<uint>();
            ColorDialog selectNewColor = new ColorDialog();

            selectNewColor.AllowFullOpen = true;
            selectNewColor.ShowHelp = true;

            if (selectNewColor.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewTextBoxCell cell in this.dataGridView1.SelectedCells)
                {
                    selectedCells.Add(this.spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex));
                    originalColors.Add(this.spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex).BGColor);
                    this.spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex).BGColor = this.ColorToUint(selectNewColor.Color);
                }

                ColorChange colorChange = new ColorChange(selectedCells, originalColors, this.ColorToUint(selectNewColor.Color));
                this.spreadsheet.AddUndo(colorChange);
            }
        }

        /// <summary>
        /// This is a helper function that changes the selected color from the UI
        /// to a uint so we can pass the uint as a parameter for other functions.
        /// </summary>
        /// <param name="color">color selected by the user for the background of the cell(s).</param>
        /// <returns>unit that corresponds with the selected color.</returns>
        private uint ColorToUint(Color color)
        {
            return (uint)(((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B) & 0xffffffffL);
        }

        /// <summary>
        /// Undo menu item click, undo any text or background color change.
        /// Calls the spreadsheet undo function.
        /// </summary>
        /// <param name="sender">sender, menu click.</param>
        /// <param name="e">e.</param>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.spreadsheet.Undo();
        }

        /// <summary>
        /// Redo menu item click, redo any undo changes made.
        /// Calls the spreadsheet redo function.
        /// </summary>
        /// <param name="sender">sender, menu click.</param>
        /// <param name="e">e.</param>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.spreadsheet.Redo();
        }

        /// <summary>
        /// Depending on the undoStack and redoStack tops the menu undo and redo
        /// items vary.
        /// </summary>
        /// <param name="sender">sender, menu click.</param>
        /// <param name="e">e.</param>
        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.spreadsheet.GetCountUndoStack() == 0)
            {
                this.undoToolStripMenuItem.Text = "Undo";
                this.undoToolStripMenuItem.Enabled = false;
                this.MenuOptionsForRedo();
            }
            else if (this.spreadsheet.PeekUndoStack() is ColorChange)
            {
                this.undoToolStripMenuItem.Text = "Undo background color change";
                this.MenuOptionsForRedo();
            }
            else if (this.spreadsheet.PeekUndoStack() is TextChange)
            {
                this.undoToolStripMenuItem.Text = "Undo text change";
                this.MenuOptionsForRedo();
            }
        }

        /// <summary>
        /// Helper function that just has all of the possible redo options.
        /// </summary>
        private void MenuOptionsForRedo()
        {
            if (this.spreadsheet.PeekRedoStack() == null)
            {
                this.redoToolStripMenuItem.Text = "Redo";
            }
            else if (this.spreadsheet.PeekRedoStack() is ColorChange)
            {
                this.redoToolStripMenuItem.Text = "Redo background color change";
            }
            else if (this.spreadsheet.PeekRedoStack() is TextChange)
            {
                this.redoToolStripMenuItem.Text = "Redo text change";
            }
        }

        /// <summary>
        /// Load spreadsheet menu item click.
        /// Will load a spreadsheet to the winforms spreadsheet console.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void Load_Spreadsheet_MenuItem_Click(object sender, EventArgs e)
        {
            List<Cell> listOfLoadedCells = new List<Cell>();

            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "XML File (*xml)|*.xml";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of the specified file
                    filePath = openFileDialog1.FileName;
                    var fileStream = openFileDialog1.OpenFile();
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        // clears the datagrid
                        this.ClearSheet();

                        listOfLoadedCells = this.spreadsheet.LoadSpreadsheet(streamReader);
                    }

                    fileStream.Close();

                    foreach (Cell cell in listOfLoadedCells)
                    {
                        DataGridViewCellEventArgs dataGridArg = new DataGridViewCellEventArgs(cell.ColumnIndex, cell.RowIndex);
                        this.DataGridView1_CellEndEdit(this.dataGridView1, dataGridArg);
                    }

                    // clears the undo and redo stacks.
                    this.spreadsheet.Workbook.ClearStacks();
                }
            }
        }

        /// <summary>
        /// Save spreadsheet menu item click.
        /// Will save a spreadsheet from the winforms spreadsheet console.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void Save_Spreadsheet_MenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "XML File (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    this.spreadsheet.SaveSpreadsheet(myStream);
                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// clears the spreadsheet so that a new one can be loaded.
        /// </summary>
        private void ClearSheet()
        {
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.Refresh();

            this.Form1_Load(this, new EventArgs());
        }
    }
}
