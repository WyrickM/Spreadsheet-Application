// <copyright file="Workbook.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CptS321
{
    /// <summary>
    /// Workbook class that will have file functions:
    /// undo/redo
    /// load/save.
    /// </summary>
    public class Workbook
    {
        /// <summary>
        /// spreadsheet currently in use, will only be used to save
        /// and load the cells of the spreadsheet.
        /// </summary>
        private readonly Cell[,] mySpreadsheet;

        /// <summary>
        /// Undo stack that will keep track of the changes to be undone.
        /// Stack since we want last in first out principle.
        /// </summary>
        private Stack<ICommand> undoStack; // = new Stack<ICommand>();

        /// <summary>
        /// Redo stack that will keep track of the changes to be redone.
        /// Stack since we want last in first out principle.
        /// </summary>
        private Stack<ICommand> redoStack; // = new Stack<ICommand>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Workbook"/> class.
        /// </summary>
        /// <param name="currentspreadsheetCells">passing in the spreadsheet.</param>
        public Workbook(Cell[,] currentspreadsheetCells)
        {
            this.undoStack = new Stack<ICommand>();
            this.redoStack = new Stack<ICommand>();
            this.mySpreadsheet = currentspreadsheetCells;
        }

        /// <summary>
        /// Gets the redo stack.
        /// </summary>
        internal Stack<ICommand> RedoStack
        {
            get
            {
                return this.redoStack;
            }
        }

        /// <summary>
        /// Gets the undo stack.
        /// </summary>
        internal Stack<ICommand> UndoStack
        {
            get
            {
                return this.undoStack;
            }
        }

        /// <summary>
        /// Adds the ICommand to the top of the undo stack.
        /// </summary>
        /// <param name="command">ICommand, most recent change to the spreadsheet.</param>
        public void AddToUndoStack(ICommand command)
        {
            this.undoStack.Push(command);
        }

        /// <summary>
        /// Undo function that performs Unexecute to undo the most
        /// recent change to the spreadsheet. Then pushes that change
        /// to the redo stack.
        /// </summary>
        public void Undo()
        {
            this.undoStack.Peek().Unexecute();
            this.redoStack.Push(this.undoStack.Pop());
        }

        /// <summary>
        /// Redo function that perfoms Execute to redo the most recent
        /// undo action to the spreadsheet. Then pushes that change to the
        /// undo stack.
        /// </summary>
        public void Redo()
        {
            this.redoStack.Peek().Execute();
            this.undoStack.Push(this.redoStack.Pop());
        }

        /// <summary>
        /// Save function that saves the spreadsheet's cells' data in
        /// XML format.
        ///
        /// If cell has not been modified do not need to write/save data
        /// for that cell to the file.
        /// </summary>
        /// <param name="stream">file that I am saving the spreadsheet to.</param>
        public void Save(Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();

            XmlWriter xmlWriter = XmlWriter.Create(stream, settings);

            xmlWriter.WriteStartDocument();

            // everything will be inside the spreadsheet element.
            xmlWriter.WriteStartElement("spreadsheet");

            for (int rowIndex = 0; rowIndex < this.mySpreadsheet.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.mySpreadsheet.GetLength(1); columnIndex++)
                {
                    // get the cell.
                    Cell tempCell = this.mySpreadsheet[rowIndex, columnIndex];

                    // only write cell to xml if cell has been modified.
                    if (tempCell.Text != string.Empty || tempCell.BGColor != 0xFFFFFFFF)
                    {
                        // start with cell element
                        //      <cell>
                        xmlWriter.WriteStartElement("cell");

                        // cell name element that contains the name of the cell
                        //      <name> (name of cell) </name>
                        xmlWriter.WriteStartElement("name");
                        xmlWriter.WriteString(tempCell.CellName);
                        xmlWriter.WriteEndElement(); // ends the name element

                        // cell bgcolor element that contains the color of the cell uint to string
                        //      <bgcolor> (color of cell)  </bgcolor>
                        xmlWriter.WriteStartElement("bgcolor");
                        xmlWriter.WriteString(tempCell.BGColor.ToString());
                        xmlWriter.WriteEndElement(); // ends the bgcolor element

                        // cell text element that contains the text of the cell.
                        //      <text> (text of cell) </text>
                        xmlWriter.WriteStartElement("text");
                        xmlWriter.WriteString(tempCell.Text);
                        xmlWriter.WriteEndElement(); // ends the text element.

                        // end the cell element
                        xmlWriter.WriteEndElement();
                    }
                }
            }

            // added all modified cells of the spreadsheet to the file
            // in xml format. Now end spreadsheet element.
            xmlWriter.WriteEndElement();

            // end the xml document
            xmlWriter.WriteEndDocument();

            // close the xmlWriter
            xmlWriter.Close();
        }

        /// <summary>
        /// Load function that loads the spreadsheet and its cells' data
        /// in XML format.
        ///
        /// May assume that only valid XML files are being loaded.
        /// </summary>
        /// <param name="stream">file that we are loading a spreadsheet from.</param>
        /// <returns>List of loaded cells.</returns>
        public List<Cell> Load(StreamReader stream)
        {
            string tempString = string.Empty;
            Cell tempCell = null;
            List<Cell> listOfLoadedCells = new List<Cell>();

            XmlReaderSettings settings = new XmlReaderSettings();

            XmlReader xmlReader = XmlReader.Create(stream, settings);

            // making the xml reader read the contents inside
            // the spreadsheet element.
            xmlReader.ReadStartElement("spreadsheet");

            // do this loop as long as the name of the element
            // being read is "cell".
            while (xmlReader.Name == "cell" || xmlReader.Name == "name" || xmlReader.Name == "bgcolor" ||
                    xmlReader.Name == "text")
            {
                if (xmlReader.NodeType != XmlNodeType.EndElement && xmlReader.Name == "cell")
                {
                    // start a new read cell element to get the next cell element.
                    xmlReader.ReadStartElement("cell");
                }

                if (xmlReader.Name == "name")
                {
                    // read the data inside the name element of the cell
                    xmlReader.ReadStartElement("name");
                    tempString = xmlReader.ReadContentAsString(); // getting the content of the element.
                    int[] coordinates = this.Coordinates(tempString);
                    tempCell = this.mySpreadsheet[coordinates[1], coordinates[0]]; // getting the cell at the desired coordinates.
                    xmlReader.ReadEndElement();
                }

                if (xmlReader.Name == "bgcolor")
                {
                    // read the data inside the color element
                    xmlReader.ReadStartElement("bgcolor");
                    tempString = xmlReader.ReadContentAsString();
                    uint.TryParse(tempString, out uint colorNumber);
                    tempCell.BGColor = colorNumber;
                    xmlReader.ReadEndElement();
                }

                if (xmlReader.Name == "text")
                {
                    // read the data inside the text element
                    xmlReader.ReadStartElement("text");
                    tempCell.Text = xmlReader.ReadContentAsString();
                    tempCell.Value = tempCell.Text;
                    xmlReader.ReadEndElement();
                }

                if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "cell")
                {
                    // read the end element of the cell element
                    xmlReader.ReadEndElement();
                    listOfLoadedCells.Add(tempCell);
                }

                if (xmlReader.Name != "cell" && xmlReader.Name != "name" && xmlReader.Name != "bgcolor" &&
                    xmlReader.Name != "text" && xmlReader.Name != "spreadsheet")
                {
                    xmlReader.Skip();
                }
            }

            if (xmlReader.Name == "spreadsheet")
            {
                // read the end element of the spreadsheet element
                xmlReader.ReadEndElement();
            }

            this.ClearStacks();
            return listOfLoadedCells;
        }

        /// <summary>
        /// clears the undo and redo stacks
        ///     called by Load.
        /// </summary>
        public void ClearStacks()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
        }

        /// <summary>
        /// This converts a string in the form of "A1" or "A10"
        /// to be converted into row and column coordinates.
        /// </summary>
        /// <param name="name">string name, ex A1.</param>
        /// <returns>int array, column = 0 row = 1.</returns>
        private int[] Coordinates(string name)
        {
            int[] coordinates = new int[2];
            int rowIndex = -1, columnIndex = -1;

            try
            {
                columnIndex = Convert.ToInt32(name[0]) - 65;
                rowIndex = Convert.ToInt32(name.Substring(1)) - 1;

                coordinates[0] = columnIndex;
                coordinates[1] = rowIndex;

                return coordinates;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
