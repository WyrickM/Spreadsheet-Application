// <copyright file="TestWorkbook.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using NUnit.Framework;

namespace CptS321
{
    /// <summary>
    /// this class tests the methods in the.
    /// </summary>
    [TestFixture]
    public class TestWorkbook
    {
        /// <summary>
        /// Instantiating private spreadsheet object to use for testing private
        /// methods in Spreadsheet class.
        /// </summary>
        private Spreadsheet testingSpreadsheet = new Spreadsheet(10, 10);

        /// <summary>
        /// Instantiating private workbook object to use for testing private
        /// methods in Workbook class.
        /// </summary>
        private Workbook testingWorkbook = new Workbook(new Cell[10, 10]);

        /// <summary>
        /// Testing the getter for undo and redo stacks with 0 commands.
        /// </summary>
        [Test]
        public void TestGetUndoRedoStacks0Commands()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();
            Stack<ICommand> testRedoStack = new Stack<ICommand>();

            Assert.AreEqual(testUndoStack, testWorkbook.UndoStack);
            Assert.AreEqual(testRedoStack, testWorkbook.RedoStack);
        }

        /// <summary>
        /// Testing adding a new text change command to empty undo stack.
        /// </summary>
        [Test]
        public void TestAddToEmptyUndoStackTextChange()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            string originalText = "test", newText = "new text";
            ICommand testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            Assert.AreEqual(testUndoStack, testWorkbook.UndoStack);
        }

        /// <summary>
        /// Testing adding a new text change command to nonempty undo stack.
        /// </summary>
        [Test]
        public void TestAddToNonEmptyUndoStackTextChange()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            string originalText = "test", newText = "new text";
            ICommand testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            testE = new DataGridViewCellEventArgs(1, 1);
            originalText = "Null";
            newText = "Empty";

            testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            Assert.AreEqual(testUndoStack, testWorkbook.UndoStack);
        }

        /// <summary>
        /// Testing adding a new color change command to empty undo stack.
        /// </summary>
        [Test]
        public void TestAddToEmptyUndoStackColorChange()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            List<Cell> listTestCells = new List<Cell>();
            listTestCells.Add(testSpreadsheet.GetCell(0, 0));
            listTestCells.Add(testSpreadsheet.GetCell(1, 1));

            List<uint> listTestOGColors = new List<uint>();
            listTestOGColors.Add(4294967295);
            listTestOGColors.Add(4294967295);

            uint newTestColor = 4294901760;

            ICommand testCommand = new ColorChange(listTestCells, listTestOGColors, newTestColor);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            Assert.AreEqual(testUndoStack, testWorkbook.UndoStack);
        }

        /// <summary>
        /// Testing adding a new color change command to nonempty undo stack.
        ///     original color: 4294967295
        ///     new color 1: 4294901760 (Red)
        ///     new color 2: 4278190335 (Blue).
        /// </summary>
        [Test]
        public void TestAddToNonEmptyUndoStackColorChange()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            List<Cell> listTestCells = new List<Cell>();
            listTestCells.Add(testSpreadsheet.GetCell(0, 0));
            listTestCells.Add(testSpreadsheet.GetCell(1, 1));

            List<uint> listTestOGColors = new List<uint>();
            listTestOGColors.Add(4294967295);
            listTestOGColors.Add(4294967295);

            uint newTestColor = 4294901760;

            ICommand testCommand = new ColorChange(listTestCells, listTestOGColors, newTestColor);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            listTestCells.Clear();
            listTestCells.Add(testSpreadsheet.GetCell(9, 9));
            listTestCells.Add(testSpreadsheet.GetCell(8, 8));

            newTestColor = 4278190335;

            testCommand = new ColorChange(listTestCells, listTestOGColors, newTestColor);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            Assert.AreEqual(testUndoStack, testWorkbook.UndoStack);
        }

        /// <summary>
        /// Testing adding a new color change command to nonempty undo stack
        /// who's top is a text change top.
        ///     original color: 4294967295 (White)
        ///     new color 1: 4294901760 (Red).
        /// </summary>
        [Test]
        public void TestAddToNonEmptyUndoStackDifferentChange()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            string originalText = "test", newText = "new text";
            ICommand testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            List<Cell> listTestCells = new List<Cell>();
            listTestCells.Add(testSpreadsheet.GetCell(0, 0));
            listTestCells.Add(testSpreadsheet.GetCell(1, 1));

            List<uint> listTestOGColors = new List<uint>();
            listTestOGColors.Add(4294967295);
            listTestOGColors.Add(4294967295);

            uint newTestColor = 4294901760;

            testCommand = new ColorChange(listTestCells, listTestOGColors, newTestColor);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            Assert.AreEqual(testUndoStack, testWorkbook.UndoStack);
        }

        /// <summary>
        /// Testing undo a new text change command to undo stack with count of 1.
        /// </summary>
        [Test]
        public void TestUndoChangeStackCount1()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();
            Stack<ICommand> testRedoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            string originalText = "test", newText = "new text";
            ICommand testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            testWorkbook.Undo();
            testRedoStack.Push(testUndoStack.Pop());

            Assert.AreEqual(testUndoStack, testWorkbook.UndoStack);
            Assert.AreEqual(testRedoStack, testWorkbook.RedoStack);
        }

        /// <summary>
        /// Testing undo a new text change command to undo stack with count of 0.
        /// </summary>
        [Test]
        public void TestUndoChangeStackCount0()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            string originalText = "test", newText = "new text";
            ICommand testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            Assert.Throws<System.InvalidOperationException>(() => testWorkbook.Undo());
        }

        /// <summary>
        /// Testing undo a new text change command to undo stack with count of 2.
        /// </summary>
        [Test]
        public void TestUndoChangeStackCount2()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();
            Stack<ICommand> testRedoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            string originalText = "test", newText = "new text";
            ICommand testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            testE = new DataGridViewCellEventArgs(1, 1);
            originalText = "Null";
            newText = "Empty";

            testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            testWorkbook.Undo();
            testRedoStack.Push(testUndoStack.Pop());

            Assert.AreEqual(testUndoStack, testWorkbook.UndoStack);
            Assert.AreEqual(testRedoStack, testWorkbook.RedoStack);
        }

        /// <summary>
        /// Testing redo a text change command to undo stack with count of 0.
        /// </summary>
        [Test]
        public void TestRedoChangeStackCount0()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();
            Stack<ICommand> testRedoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            string originalText = "test", newText = "new text";
            ICommand testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            Assert.Throws<System.InvalidOperationException>(() => testWorkbook.Redo());
        }

        /// <summary>
        /// Testing redo a text change command to redo stack with count of 1.
        /// </summary>
        [Test]
        public void TestRedoChangeStackCount1()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();
            Stack<ICommand> testRedoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            string originalText = "test", newText = "new text";
            ICommand testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            testWorkbook.Undo();
            testRedoStack.Push(testUndoStack.Pop());

            testWorkbook.Redo();
            testUndoStack.Push(testRedoStack.Pop());

            Assert.AreEqual(testUndoStack, testWorkbook.UndoStack);
            Assert.AreEqual(testRedoStack, testWorkbook.RedoStack);
        }

        /// <summary>
        /// Testing redo a text change command to redo stack with count of 2.
        /// </summary>
        [Test]
        public void TestRedoChangeStackCount2()
        {
            Cell[,] spreadsheetCells = new Cell[10, 10];

            Workbook testWorkbook = new Workbook(spreadsheetCells);

            Stack<ICommand> testUndoStack = new Stack<ICommand>();
            Stack<ICommand> testRedoStack = new Stack<ICommand>();

            DataGridViewCellEventArgs testE = new DataGridViewCellEventArgs(0, 0);
            Spreadsheet testSpreadsheet = new Spreadsheet(10, 10);

            string originalText = "test", newText = "new text";
            ICommand testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            testE = new DataGridViewCellEventArgs(1, 1);
            originalText = "Null";
            newText = "Empty";

            testCommand = new TextChange(originalText, newText, testSpreadsheet, testE);

            testWorkbook.AddToUndoStack(testCommand);
            testUndoStack.Push(testCommand);

            testWorkbook.Undo();
            testRedoStack.Push(testUndoStack.Pop());

            testWorkbook.Undo();
            testRedoStack.Push(testUndoStack.Pop());

            testWorkbook.Redo();
            testUndoStack.Push(testRedoStack.Pop());

            Assert.AreEqual(testUndoStack, testWorkbook.UndoStack);
            Assert.AreEqual(testRedoStack, testWorkbook.RedoStack);
        }

        // ----------------------------------------------------------------------------------------------------------------------
        // TESTING PRIVATE MEMBERS
        // ----------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Testing the saving and loading functionality function.
        /// Saving no data to an empty file.
        ///         will not save any file since no data.
        /// Loading the empty spreadsheet to a spreadsheet that has data.
        /// </summary>
        [Test]
        public void TestSaveAndLoadEmptySpreadsheetToFile()
        {
            string filePath = "TestFile";

            // I need to create the xml stream similar to the one in save
            // where I am creating an xml document
            FileStream testFileWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            MethodInfo methodInfo = this.GetMethodSpreadsheet("SaveToFileTest");

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { testFileWrite });

            // close file
            testFileWrite.Close();

            // open a new stream reader file.
            FileStream testFileRead = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader testStreamReader = new StreamReader(testFileRead);

            Spreadsheet expectedSpreadsheet = new Spreadsheet(10, 10);

            methodInfo = this.GetMethodSpreadsheet("LoadFromFileTest");

            Assert.IsNull(methodInfo.Invoke(this.testingSpreadsheet, new object[] { testStreamReader }));

            testStreamReader.Close();
            testFileRead.Close();

            // need to dispose of these files so other tests can use the TestFile file
            testFileRead.Dispose();
            testFileWrite.Dispose();
            testStreamReader.Dispose();
            testFileWrite = null;
            testFileRead = null;
            testStreamReader = null;
        }

        /// <summary>
        /// Testing the saving and loading functionality function.
        /// Saving data to an empty file.
        /// Loading the nonempty spreadsheet to a spreadsheet that has no data.
        /// </summary>
        [Test]
        public void TestSaveAndLoadSpreadsheetToFile()
        {
            string filePath = "TestFile";
            FileStream testFileWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            // making spreadsheet not empty.
            this.testingSpreadsheet.GetCell(0, 0).Text = "This is a test";

            MethodInfo methodInfo = this.GetMethodSpreadsheet("SaveToFileTest");

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { testFileWrite });

            // close file
            testFileWrite.Close();

            // open a new stream reader file.
            FileStream testFileRead = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader testStreamReader = new StreamReader(testFileRead);

            Spreadsheet expectedSpreadsheet = new Spreadsheet(10, 10);

            // expected text in spreadsheet at end of load.
            expectedSpreadsheet.GetCell(0, 0).Text = "This is a test";

            // no changing spreadsheet back to empty spreadsheet to see if
            // spreadsheet overrides the empty spreadsheet.
            this.testingSpreadsheet.GetCell(0, 0).Text = string.Empty;

            methodInfo = this.GetMethodSpreadsheet("LoadFromFileTest");
            newObject = methodInfo.Invoke(this.testingSpreadsheet, new object[] { testStreamReader });

            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 0).Text, this.testingSpreadsheet.GetCell(0, 0).Text);

            testStreamReader.Close();
            testFileRead.Close();

            // need to dispose of these files so other tests can use the TestFile file
            testFileRead.Dispose();
            testFileWrite.Dispose();
            testStreamReader.Dispose();
            testFileWrite = null;
            testFileRead = null;
            testStreamReader = null;
        }

        /// <summary>
        /// Testing the saving and loading functionality function.
        /// Saving data to a nonempty file.
        /// Loading the nonempty spreadsheet to a spreadsheet that has no data.
        /// </summary>
        [Test]
        public void TestSaveAndLoadSpreadsheetToNonEmptyFile()
        {
            // First save to be able to save to a nonempty file.
            // making spreadsheet not empty.
            string filePath = "TestFile";
            FileStream testFileWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            this.testingSpreadsheet.GetCell(0, 0).Text = "This is a test";

            MethodInfo methodInfo = this.GetMethodSpreadsheet("SaveToFileTest");

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { testFileWrite });

            // close file
            testFileWrite.Close();

            // Saving second spreadsheet to override the first saved spreadsheet.
            // making spreadsheet not empty.
            FileStream testFileWrite2 = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            this.testingSpreadsheet.GetCell(0, 0).Text = "Changing the name of the game.";

            methodInfo = this.GetMethodSpreadsheet("SaveToFileTest");

            newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { testFileWrite2 });

            // close file
            testFileWrite2.Close();

            // open a new stream reader file.
            FileStream testFileRead = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader testStreamReader = new StreamReader(testFileRead);

            Spreadsheet expectedSpreadsheet = new Spreadsheet(10, 10);

            // expected text in spreadsheet at end of load.
            expectedSpreadsheet.GetCell(0, 0).Text = "Changing the name of the game.";

            // no changing spreadsheet back to empty spreadsheet to see if
            // spreadsheet overrides the empty spreadsheet.
            this.testingSpreadsheet.GetCell(0, 0).Text = "This is a test";

            methodInfo = this.GetMethodSpreadsheet("LoadFromFileTest");
            newObject = methodInfo.Invoke(this.testingSpreadsheet, new object[] { testStreamReader });

            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 0).Text, this.testingSpreadsheet.GetCell(0, 0).Text);

            testStreamReader.Close();
            testFileRead.Close();

            // need to dispose of these files so other tests can use the TestFile file
            testFileRead.Dispose();
            testFileWrite.Dispose();
            testStreamReader.Dispose();
            testFileWrite = null;
            testFileRead = null;
            testStreamReader = null;
        }

        /// <summary>
        /// Testing the saving and loading functionality function.
        /// Saving no data to a nonempty file.
        ///     not modifying any cells in spreadsheet so xml will not save spreadsheet.
        /// Loading the empty spreadsheet to a spreadsheet that has data.
        /// </summary>
        [Test]
        public void TestSaveAndLoadEmptySpreadsheetToNonEmptyFile()
        {
            // First save to be able to save to a nonempty file.
            // making spreadsheet not empty.
            string filePath = "TestFile";
            FileStream testFileWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            this.testingSpreadsheet.GetCell(0, 0).Text = "This is a test";

            MethodInfo methodInfo = this.GetMethodSpreadsheet("SaveToFileTest");

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { testFileWrite });

            // close file
            testFileWrite.Close();

            // Saving second spreadsheet to override the first saved spreadsheet.
            // making spreadsheet not empty.
            FileStream testFileWrite2 = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            // since not modifiying any cells in this spreadsheet will not be saved.
            this.testingSpreadsheet.GetCell(0, 0).Text = string.Empty;

            methodInfo = this.GetMethodSpreadsheet("SaveToFileTest");

            newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { testFileWrite2 });

            // close file
            testFileWrite2.Close();

            // open a new stream reader file.
            FileStream testFileRead = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader testStreamReader = new StreamReader(testFileRead);

            // change spreadsheet back to non empty spreadsheet to see if a empty spreadsheet overrides it.
            this.testingSpreadsheet.GetCell(0, 0).Text = "This is a test";

            Spreadsheet expectedSpreadsheet = new Spreadsheet(10, 10);

            // expected text in spreadsheet at end of load.
            expectedSpreadsheet.GetCell(0, 0).Text = string.Empty;

            methodInfo = this.GetMethodSpreadsheet("LoadFromFileTest");

            Assert.IsNull(methodInfo.Invoke(this.testingSpreadsheet, new object[] { testStreamReader }));
            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 0).Text, this.testingSpreadsheet.GetCell(0, 0).Text);

            testStreamReader.Close();
            testFileRead.Close();

            // need to dispose of these files so other tests can use the TestFile file
            testFileRead.Dispose();
            testFileWrite.Dispose();
            testStreamReader.Dispose();
            testFileWrite = null;
            testFileRead = null;
            testStreamReader = null;
        }

        /// <summary>
        /// Testing the saving and loading functionality function.
        /// Saving color change data to an empty file.
        /// Loading the color change spreadsheet to a spreadsheet that has no data.
        /// </summary>
        [Test]
        public void TestSaveAndLoadColorSpreadsheetToFile()
        {
            string filePath = "TestFile";
            FileStream testFileWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            // making spreadsheet not empty.
            this.testingSpreadsheet.GetCell(0, 0).Text = "This is a test";
            this.testingSpreadsheet.GetCell(0, 0).BGColor = 4294901760;

            MethodInfo methodInfo = this.GetMethodSpreadsheet("SaveToFileTest");

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { testFileWrite });

            // close file
            testFileWrite.Close();

            // open a new stream reader file.
            FileStream testFileRead = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader testStreamReader = new StreamReader(testFileRead);

            Spreadsheet expectedSpreadsheet = new Spreadsheet(10, 10);

            // expected text in spreadsheet at end of load.
            expectedSpreadsheet.GetCell(0, 0).Text = "This is a test";
            expectedSpreadsheet.GetCell(0, 0).BGColor = 4294901760; // red.

            // now changing spreadsheet back to empty spreadsheet to see if
            // spreadsheet overrides the empty spreadsheet.
            this.testingSpreadsheet.GetCell(0, 0).Text = string.Empty;
            this.testingSpreadsheet.GetCell(0, 0).BGColor = 4294967295; // white.

            methodInfo = this.GetMethodSpreadsheet("LoadFromFileTest");
            newObject = methodInfo.Invoke(this.testingSpreadsheet, new object[] { testStreamReader });

            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 0).Text, this.testingSpreadsheet.GetCell(0, 0).Text);
            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 0).BGColor, this.testingSpreadsheet.GetCell(0, 0).BGColor);

            testStreamReader.Close();
            testFileRead.Close();

            // need to dispose of these files so other tests can use the TestFile file
            testFileRead.Dispose();
            testFileWrite.Dispose();
            testStreamReader.Dispose();
            testFileWrite = null;
            testFileRead = null;
            testStreamReader = null;
        }

        /// <summary>
        /// Testing the loading functionality function.
        /// Loading an xml file that has undesired attributes/elements.
        /// These elements should be ignored.
        /// </summary>
        [Test]
        public void TestLoadUndesiredSpreadsheetToFile()
        {
            string filePath = "TestFile";
            FileStream testFileWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            XmlWriterSettings settings = new XmlWriterSettings();

            XmlWriter xmlWriter = XmlWriter.Create(testFileWrite, settings);

            xmlWriter.WriteStartDocument();

            // everything will be inside the spreadsheet element.
            xmlWriter.WriteStartElement("spreadsheet");

            // start with cell element
            //      <cell>
            xmlWriter.WriteStartElement("cell");

            // undesired elmement.
            xmlWriter.WriteStartElement("unusedattr");
            xmlWriter.WriteString("abc");
            xmlWriter.WriteEndElement();

            // cell name element that contains the name of the cell
            //      <name> (name of cell) </name>
            xmlWriter.WriteStartElement("name");
            xmlWriter.WriteString("B1");
            xmlWriter.WriteEndElement(); // ends the name element

            // cell text element that contains the text of the cell.
            //      <text> (text of cell) </text>
            xmlWriter.WriteStartElement("text");
            xmlWriter.WriteString("=A1+6");
            xmlWriter.WriteEndElement(); // ends the text element.

            // undesired elmement.
            xmlWriter.WriteStartElement("some_tag_you_didnt_write");
            xmlWriter.WriteString("blah");
            xmlWriter.WriteEndElement();

            // cell bgcolor element that contains the color of the cell uint to string
            //      <bgcolor> (color of cell)  </bgcolor>
            uint color = 4294901760;
            xmlWriter.WriteStartElement("bgcolor");
            xmlWriter.WriteString(color.ToString());
            xmlWriter.WriteEndElement(); // ends the bgcolor element

            // undesired elmement.
            xmlWriter.WriteStartElement("another_unused_tag");
            xmlWriter.WriteString("data");
            xmlWriter.WriteEndElement();

            // end the cell element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            // end the xml document
            xmlWriter.WriteEndDocument();

            // close the xmlWriter
            xmlWriter.Close();

            // close file
            testFileWrite.Close();

            // open a new stream reader file.
            FileStream testFileRead = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader testStreamReader = new StreamReader(testFileRead);

            Spreadsheet expectedSpreadsheet = new Spreadsheet(10, 10);

            // expected text in spreadsheet at end of load.
            expectedSpreadsheet.GetCell(0, 1).Text = "=A1+6";
            expectedSpreadsheet.GetCell(0, 1).BGColor = 4294901760; // red.

            // now changing spreadsheet back to empty spreadsheet to see if
            // spreadsheet overrides the empty spreadsheet.
            this.testingSpreadsheet.GetCell(0, 1).Text = string.Empty;
            this.testingSpreadsheet.GetCell(0, 1).BGColor = 4294967295; // white.

            MethodInfo methodInfo = this.GetMethodSpreadsheet("LoadFromFileTest");
            var newObject = methodInfo.Invoke(this.testingSpreadsheet, new object[] { testStreamReader });

            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 1).Text, this.testingSpreadsheet.GetCell(0, 1).Text);
            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 1).BGColor, this.testingSpreadsheet.GetCell(0, 1).BGColor);

            testStreamReader.Close();
            testFileRead.Close();

            // need to dispose of these files so other tests can use the TestFile file
            testFileRead.Dispose();
            testFileWrite.Dispose();
            testStreamReader.Dispose();
            testFileWrite = null;
            testFileRead = null;
            testStreamReader = null;
        }

        /// <summary>
        /// Testing the loading functionality function.
        /// Loading an xml file that has undesired attributes/elements
        /// in multiple cells.
        /// These elements should be ignored.
        /// </summary>
        [Test]
        public void TestLoadUndesiredCellsSpreadsheetToFile()
        {
            string filePath = "TestFile";
            FileStream testFileWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            XmlWriterSettings settings = new XmlWriterSettings();

            XmlWriter xmlWriter = XmlWriter.Create(testFileWrite, settings);

            xmlWriter.WriteStartDocument();

            // everything will be inside the spreadsheet element.
            xmlWriter.WriteStartElement("spreadsheet");

            // start with cell element
            //      <cell>
            xmlWriter.WriteStartElement("cell");

            // undesired elmement.
            xmlWriter.WriteStartElement("unusedattr");
            xmlWriter.WriteString("abc");
            xmlWriter.WriteEndElement();

            // cell name element that contains the name of the cell
            //      <name> (name of cell) </name>
            xmlWriter.WriteStartElement("name");
            xmlWriter.WriteString("B1");
            xmlWriter.WriteEndElement(); // ends the name element

            // cell text element that contains the text of the cell.
            //      <text> (text of cell) </text>
            xmlWriter.WriteStartElement("text");
            xmlWriter.WriteString("=A1+6");
            xmlWriter.WriteEndElement(); // ends the text element.

            // undesired elmement.
            xmlWriter.WriteStartElement("some_tag_you_didnt_write");
            xmlWriter.WriteString("blah");
            xmlWriter.WriteEndElement();

            // cell bgcolor element that contains the color of the cell uint to string
            //      <bgcolor> (color of cell)  </bgcolor>
            uint color = 4294901760;
            xmlWriter.WriteStartElement("bgcolor");
            xmlWriter.WriteString(color.ToString());
            xmlWriter.WriteEndElement(); // ends the bgcolor element

            // undesired elmement.
            xmlWriter.WriteStartElement("another_unused_tag");
            xmlWriter.WriteString("data");
            xmlWriter.WriteEndElement();

            // end the cell element
            xmlWriter.WriteEndElement();

            // start with cell element
            //      <cell>
            xmlWriter.WriteStartElement("cell");

            // undesired elmement.
            xmlWriter.WriteStartElement("unusedattr");
            xmlWriter.WriteString("abc");
            xmlWriter.WriteEndElement();

            // cell name element that contains the name of the cell
            //      <name> (name of cell) </name>
            xmlWriter.WriteStartElement("name");
            xmlWriter.WriteString("A1");
            xmlWriter.WriteEndElement(); // ends the name element

            // cell text element that contains the text of the cell.
            //      <text> (text of cell) </text>
            xmlWriter.WriteStartElement("text");
            xmlWriter.WriteString("=A1+6");
            xmlWriter.WriteEndElement(); // ends the text element.

            // undesired elmement.
            xmlWriter.WriteStartElement("some_tag_you_didnt_write");
            xmlWriter.WriteString("blah");
            xmlWriter.WriteEndElement();

            // cell bgcolor element that contains the color of the cell uint to string
            //      <bgcolor> (color of cell)  </bgcolor>
            color = 4294901760;
            xmlWriter.WriteStartElement("bgcolor");
            xmlWriter.WriteString(color.ToString());
            xmlWriter.WriteEndElement(); // ends the bgcolor element

            // undesired elmement.
            xmlWriter.WriteStartElement("another_unused_tag");
            xmlWriter.WriteString("data");
            xmlWriter.WriteEndElement();

            // end the cell element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            // end the xml document
            xmlWriter.WriteEndDocument();

            // close the xmlWriter
            xmlWriter.Close();

            // close file
            testFileWrite.Close();

            // open a new stream reader file.
            FileStream testFileRead = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader testStreamReader = new StreamReader(testFileRead);

            Spreadsheet expectedSpreadsheet = new Spreadsheet(10, 10);

            // expected text in cell B1 at end of load.
            expectedSpreadsheet.GetCell(0, 1).Text = "=A1+6";
            expectedSpreadsheet.GetCell(0, 1).BGColor = 4294901760; // red.

            // expected text and color in cell A1
            expectedSpreadsheet.GetCell(0, 0).Text = "=A1+6";
            expectedSpreadsheet.GetCell(0, 0).BGColor = 4294901760; // red.

            // now changing spreadsheet back to empty spreadsheet to see if
            // spreadsheet overrides the empty spreadsheet.
            this.testingSpreadsheet.GetCell(0, 1).Text = string.Empty;
            this.testingSpreadsheet.GetCell(0, 1).BGColor = 4294967295; // white.
            this.testingSpreadsheet.GetCell(0, 0).Text = string.Empty;
            this.testingSpreadsheet.GetCell(0, 0).BGColor = 4294967295; // white.

            MethodInfo methodInfo = this.GetMethodSpreadsheet("LoadFromFileTest");
            var newObject = methodInfo.Invoke(this.testingSpreadsheet, new object[] { testStreamReader });

            // checking A1 cell
            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 0).Text, this.testingSpreadsheet.GetCell(0, 0).Text);
            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 0).BGColor, this.testingSpreadsheet.GetCell(0, 0).BGColor);

            // checking B1 cell
            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 1).Text, this.testingSpreadsheet.GetCell(0, 1).Text);
            Assert.AreEqual(expectedSpreadsheet.GetCell(0, 1).BGColor, this.testingSpreadsheet.GetCell(0, 1).BGColor);

            testStreamReader.Close();
            testFileRead.Close();

            // need to dispose of these files so other tests can use the TestFile file
            testFileRead.Dispose();
            testFileWrite.Dispose();
            testStreamReader.Dispose();
            testFileWrite = null;
            testFileRead = null;
            testStreamReader = null;
        }

        /// <summary>
        /// Testing coordinates method with empty string.
        /// </summary>
        [Test]
        public void TestCoordinatesEmptyString()
        {
            string testString = string.Empty;

            MethodInfo methodInfo = this.GetMethodWorkbook("Coordinates");

            int[] expectedCoordinates = new int[2];

            Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            methodInfo.Invoke(
                this.testingWorkbook, // the object on which we are invoking the method
                new object[] { testString }));
        }

        /// <summary>
        /// Testing coordinates method with string A1.
        /// </summary>
        [Test]
        public void TestCoordinatesA1()
        {
            string testString = "A1";

            MethodInfo methodInfo = this.GetMethodWorkbook("Coordinates");

            var newObject = methodInfo.Invoke(
                this.testingWorkbook, // the object on which we are invoking the method
                new object[] { testString });

            int[] expectedCoordinates = new int[2];
            expectedCoordinates[0] = 0;
            expectedCoordinates[1] = 0;

            Assert.AreEqual(expectedCoordinates, newObject);
        }

        /// <summary>
        /// Testing coordinates method with string B10.
        /// </summary>
        [Test]
        public void TestCoordinatesB10()
        {
            string testString = "B10";

            MethodInfo methodInfo = this.GetMethodWorkbook("Coordinates");

            var newObject = methodInfo.Invoke(
                this.testingWorkbook, // the object on which we are invoking the method
                new object[] { testString });

            int[] expectedCoordinates = new int[2];
            expectedCoordinates[0] = 1;
            expectedCoordinates[1] = 9;

            Assert.AreEqual(expectedCoordinates, newObject);
        }

        /// <summary>
        /// Testing coordinates method with string 110.
        /// </summary>
        [Test]
        public void TestCoordinates110()
        {
            string testString = "110";

            MethodInfo methodInfo = this.GetMethodWorkbook("Coordinates");

            var newObject = methodInfo.Invoke(
                this.testingWorkbook, // the object on which we are invoking the method
                new object[] { testString });

            int[] expectedCoordinates = new int[2];
            expectedCoordinates[0] = -16;
            expectedCoordinates[1] = 9;

            Assert.AreEqual(expectedCoordinates, newObject);
        }

        /// <summary>
        /// Testing coordinates method with string A100.
        /// </summary>
        [Test]
        public void TestCoordinatesA100()
        {
            string testString = "A100";

            MethodInfo methodInfo = this.GetMethodWorkbook("Coordinates");

            var newObject = methodInfo.Invoke(
                this.testingWorkbook, // the object on which we are invoking the method
                new object[] { testString });

            int[] expectedCoordinates = new int[2];
            expectedCoordinates[0] = 0;
            expectedCoordinates[1] = 99;

            Assert.AreEqual(expectedCoordinates, newObject);
        }

        /// <summary>
        /// This allows me to call private methods in the spreadSheet class so I can test them.
        /// </summary>
        /// <param name="methodName">string value, name of the private method I want to test.</param>
        /// <returns>MethodInfo.</returns>
        private MethodInfo GetMethodSpreadsheet(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Assert.Fail("methodName cannot be null or whitespace");
            }

            var method = this.testingSpreadsheet.GetType()
                .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

            if (method == null)
            {
                Assert.Fail(string.Format("{0} method not found", methodName));
            }

            return method;
        }

        /// <summary>
        /// This allows me to call private methods in the workbook class so I can test them.
        /// </summary>
        /// <param name="methodName">string value, name of the private method I want to test.</param>
        /// <returns>MethodInfo.</returns>
        private MethodInfo GetMethodWorkbook(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Assert.Fail("methodName cannot be null or whitespace");
            }

            var method = this.testingWorkbook.GetType()
                .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

            if (method == null)
            {
                Assert.Fail(string.Format("{0} method not found", methodName));
            }

            return method;
        }
    }
}
