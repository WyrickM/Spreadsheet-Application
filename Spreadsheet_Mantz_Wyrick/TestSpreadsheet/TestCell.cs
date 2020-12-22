// <copyright file="TestCell.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace CptS321
{
    /// <summary>
    /// This test class tests the methods in the abstract Cell class that get inherited by Spreadsheet class.
    /// </summary>
    [TestFixture]
    public class TestCell
    {
        /// <summary>
        /// Testing the Text getter method for a cell that has no text in it.
        /// </summary>
        [Test]
        public void TestTextGetNoText()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(1, 1);

            Assert.AreEqual(
                string.Empty, // expecting return value
                newSpreadsheet.GetCell(0, 0).Text);
        }

        /// <summary>
        /// Testing the Text getter method for a cell that has text in it.
        /// </summary>
        [Test]
        public void TestTextGetWithText()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(1, 1);
            newSpreadsheet.GetCell(0, 0).Text = "This is a test";
            Assert.AreEqual("This is a test", newSpreadsheet.GetCell(0, 0).Text);
        }

        /// <summary>
        /// Testing the Text setter method for a cell that has no text in it.
        /// </summary>
        [Test]
        public void TestTextSetTextWhenNoText()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(1, 1);
            newSpreadsheet.GetCell(0, 0).Text = "This is a test"; // setting text to cell that has no text
            Assert.AreEqual("This is a test", newSpreadsheet.GetCell(0, 0).Text);
        }

        /// <summary>
        /// Testing the Text setter method for a cell that has preexisting text in it.
        /// </summary>
        [Test]
        public void TestTextSetWithTextExisting()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(1, 1);
            newSpreadsheet.GetCell(0, 0).Text = "This is a test"; // setting text to cell that has no text
            newSpreadsheet.GetCell(0, 0).Text = "Check override!"; // setting text to cell that has text
            Assert.AreEqual("Check override!", newSpreadsheet.GetCell(0, 0).Text);
        }

        /// <summary>
        /// Testing the Text setter method for a cell that has preexisting text in it, clearing the cell.
        /// </summary>
        [Test]
        public void TestTextSetNothingWithTextExisting()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(1, 1);
            newSpreadsheet.GetCell(0, 0).Text = "This is a test"; // setting text to cell that has no text
            newSpreadsheet.GetCell(0, 0).Text = string.Empty; // setting text to cell that has text
            Assert.AreEqual(string.Empty, newSpreadsheet.GetCell(0, 0).Text);
        }

        /// <summary>
        /// Testing the Get and Set method of RowIndex to make sure it is Not.Null.
        /// </summary>
        [Test]
        public void TestGetRowIndexNotNull()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            Cell testCell = newSpreadsheet.GetCell(3, 5);
            Assert.That(testCell.RowIndex, Is.Not.Null);
        }

        /// <summary>
        /// Testing the RowIndex getter method for a cell.
        /// </summary>
        [Test]
        public void TestGetRowIndex()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            Cell testCell = newSpreadsheet.GetCell(3, 5);
            Assert.AreEqual(3, testCell.RowIndex);
        }

        /// <summary>
        /// Testing the Get and Set method of ColumnIndex to make sure it is Not.Null.
        /// </summary>
        [Test]
        public void TestGetColumnIndexNotNull()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            Cell testCell = newSpreadsheet.GetCell(3, 5);
            Assert.That(testCell.ColumnIndex, Is.Not.Null);
        }

        /// <summary>
        /// Testing the RowColumn getter method for a cell.
        /// </summary>
        [Test]
        public void TestGetColumnIndex()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            Cell testCell = newSpreadsheet.GetCell(3, 5);
            Assert.AreEqual(5, testCell.ColumnIndex);
        }
    }
}
