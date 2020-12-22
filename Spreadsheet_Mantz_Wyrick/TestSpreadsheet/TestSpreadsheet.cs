// <copyright file="TestSpreadsheet.cs" company="Mantz Wyrick - 11504292">
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
    /// This test class tests the methods in the Spreadsheet class.
    /// </summary>
    [TestFixture]
    public class TestSpreadsheet
    {
        /// <summary>
        /// Instantiating private spreadsheet object to use for testing private methods in Spreadsheet class.
        /// </summary>
        private Spreadsheet testingSpreadsheet = new Spreadsheet(10, 20);

        /// <summary>
        /// Testing GetCell method for a cell that does not exist,
        /// testing to make sure it returns null.
        /// </summary>
        [Test]
        public void TestGetCellNonExistentCell()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            Assert.Throws<System.Exception>(() => newSpreadsheet.GetCell(11, 11));
        }

        /// <summary>
        /// Testing GetCell method for a cell that exists,
        /// testing to make sure it does not return null.
        /// </summary>
        [Test] // when instantiating newSpreadsheet, all indices are equal to null, need to figure out how to set them to not be null.
        public void TestGetCellExistentCell()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            Assert.That(newSpreadsheet.GetCell(1, 1), Is.Not.Null);
        }

        /// <summary>
        /// Testing GetCell method that takes a string name which will be a cell name
        /// Testing to make sure a valid cell does not return null.
        /// </summary>
        [Test]
        public void TestGetCellStringNameValid()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            Assert.That(newSpreadsheet.GetCell("A1"), Is.Not.Null);
        }

        /// <summary>
        /// Testing GetCell method that takes a string name which will be a cell name
        /// Testing to make sure a valid cell returns empty string.
        /// </summary>
        [Test]
        public void TestGetCellStringNameValidEmptyCell()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            Assert.AreEqual(string.Empty, newSpreadsheet.GetCell("A1").Value);
        }

        /// <summary>
        /// Testing GetCell method that takes a string name which will be a cell name
        /// Testing to make sure a valid cell returns the correct value.
        /// </summary>
        [Test]
        public void TestGetCellStringNameValidNonEmptyCell()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            newSpreadsheet.GetCell(0, 0).Text = "1";
            newSpreadsheet.GetCell(0, 0).Value = "1";
            Assert.AreEqual("1", newSpreadsheet.GetCell("A1").Value);
        }

        /// <summary>
        /// Testing GetCell method that takes a string name which will be a cell name
        /// Testing to make sure a throw exception for the invalid name of a cell.
        /// </summary>
        [Test]
        public void TestGetCellStringInvalidName()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            Assert.Throws<System.Exception>(() => newSpreadsheet.GetCell("Z12345"));
        }

        /// <summary>
        /// Testing GetCell method that takes a string name which will be a cell name
        /// Testing to make sure a throw exception for the invalid name of a cell.
        /// </summary>
        [Test]
        public void TestGetCellStringAnotherInvalidName()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 10);
            Assert.Throws<System.Exception>(() => newSpreadsheet.GetCell("Ba"));
        }

        /// <summary>
        /// Testing getter and setter for RowCount, checking if not null.
        /// </summary>
        [Test]
        public void TestGetRowCountIsNotNull()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 20);
            Assert.That(newSpreadsheet.RowCount, Is.Not.Null);
        }

        /// <summary>
        /// Testing getter and setter for RowCount, test actual count.
        /// </summary>
        [Test]
        public void TestGetRowCount()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 20);
            Assert.AreEqual(10, newSpreadsheet.RowCount);
        }

        /// <summary>
        /// Testing getter and setter for ColumnCount, checking if not null.
        /// </summary>
        [Test]
        public void TestGetColumnCountIsNotNull()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 20);
            Assert.That(newSpreadsheet.ColumnCount, Is.Not.Null);
        }

        /// <summary>
        /// Testing getter and setter for ColumnCount, test actual count.
        /// </summary>
        [Test]
        public void TestGetColumnCount()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(10, 20);
            Assert.AreEqual(20, newSpreadsheet.ColumnCount);
        }

        // ----------------------------------------------------------------------------------------------------------------------
        // TESTING PRIVATE MEMBERS
        // ----------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Tests the GetTextFromCell method when the cell has no text in it. Should contain empty string.
        /// </summary>
        [Test]
        public void TestGetTextFromCellNoText()
        {
            MethodInfo methodInfo = this.GetMethod("GetTextFromCell");

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { "A1" });
            Assert.IsNotNull(newObject);
        }

        /// <summary>
        /// Testing the GetTextFromCell when the cell has text.
        /// </summary>
        [Test]
        public void TestGetTextFromCellWithText()
        {
            this.testingSpreadsheet.GetCell(0, 0).Text = "This is a test";
            MethodInfo methodInfo = this.GetMethod("GetTextFromCell");

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { "A1" });
            Assert.AreEqual("This is a test", newObject);
        }

        /// <summary>
        /// Testing the private function SetDictionary for the column with the name "2".
        /// which is not a key so should return null.
        /// </summary>
        [Test]
        public void TestSetDictionaryFor2()
        {
            // do not need to call GetMethod since constructor calls private method SetDictionary method
            // thus constructor instantiates the private cellLocationName dictionary field.
            Dictionary<string, int> testDictionary = this.testingSpreadsheet.GetDictionary;
            try
            {
                Assert.Fail("Key not found", testDictionary["2"]);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("The given key was not present in the dictionary.", ex.Message);
            }
        }

        /// <summary>
        /// Testing the private function SetDictionary for the column with the name "A".
        /// Indirectly testing the SetDictionary method.
        /// </summary>
        [Test]
        public void TestSetDictionaryForA()
        {
            // do not need to call GetMethod since constructor calls private method SetDictionary method
            // thus constructor instantiates the private cellLocationName dictionary field.
            Dictionary<string, int> testDictionary = this.testingSpreadsheet.GetDictionary;
            Assert.AreEqual(0, testDictionary["A"]);
        }

        /// <summary>
        /// Testing the private function SetDictionary for the column with the name "Z".
        /// Indirectly testing the SetDictionary method.
        /// </summary>
        [Test]
        public void TestSetDictionaryForZ()
        {
            // do not need to call GetMethod since constructor calls private method SetDictionary method
            // thus constructor instantiates the private cellLocationName dictionary field.
            Dictionary<string, int> testDictionary = this.testingSpreadsheet.GetDictionary;
            Assert.AreEqual(25, testDictionary["Z"]);
        }

        /// <summary>
        /// Testing the private function CheckCellReference for empty cell A1.
        /// </summary>
        [Test]
        public void TestCheckCellReferenceEmptyCell()
        {
            MethodInfo methodInfo = this.GetMethod("CheckCellReference");

            this.testingSpreadsheet.GetCell("A1").ListVariableNames = new List<string>();

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { this.testingSpreadsheet.GetCell("A1") });
            Assert.AreEqual(true, newObject);
        }

        /// <summary>
        /// Testing the private function CheckCellReference for nonempty cell A1 with a valid reference.
        /// </summary>
        [Test]
        public void TestCheckCellReferenceNonEmptyCellValidReference()
        {
            MethodInfo methodInfo = this.GetMethod("CheckCellReference");

            this.testingSpreadsheet.GetCell("A1").ListVariableNames = new List<string>();
            this.testingSpreadsheet.GetCell("A1").Text = "B1";
            this.testingSpreadsheet.GetCell("A1").ListVariableNames.Add("B1");

            this.testingSpreadsheet.GetCell("B1").ListVariableNames = new List<string>();

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { this.testingSpreadsheet.GetCell("A1") });
            Assert.AreEqual(true, newObject);
        }

        /// <summary>
        /// Testing the pricate function CheckCellReference for nonempty cell A1 with a self reference.
        /// </summary>
        [Test]
        public void TestCheckCellReferenceNonEmptyCellSelfReference()
        {
            MethodInfo methodInfo = this.GetMethod("CheckCellReference");

            this.testingSpreadsheet.GetCell("A1").Text = "A1";
            this.testingSpreadsheet.GetCell("A1").ListVariableNames = new List<string>();
            this.testingSpreadsheet.GetCell("A1").ListVariableNames.Add("A1");

            Assert.Throws<System.Reflection.TargetInvocationException>(() => methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { this.testingSpreadsheet.GetCell("A1") }));
        }

        /// <summary>
        /// Testing the private function CheckCellReference for nonempty cell A1 with a circular reference.
        /// </summary>
        [Test]
        public void TestCheckCellReferenceNonEmptyCellCircularReference()
        {
            MethodInfo methodInfo = this.GetMethod("CheckCellReference");

            this.testingSpreadsheet.GetCell("A1").Text = "B1";
            this.testingSpreadsheet.GetCell("B1").Text = "A1";

            this.testingSpreadsheet.GetCell("A1").ListVariableNames = new List<string>();
            this.testingSpreadsheet.GetCell("A1").ListVariableNames.Add("B1");

            this.testingSpreadsheet.GetCell("B1").ListVariableNames = new List<string>();
            this.testingSpreadsheet.GetCell("B1").ListVariableNames.Add("A1");

            Assert.Throws<System.Reflection.TargetInvocationException>(() => methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { this.testingSpreadsheet.GetCell("A1") }));
        }

        /// <summary>
        /// Testing the private IsCircularReference method with no circular reference.
        /// </summary>
        [Test]
        public void TestIsCircularReferenceOneCellNotCircular()
        {
            MethodInfo methodInfo = this.GetMethod("IsCircularReference");

            this.testingSpreadsheet.GetCell("A1").Text = "B1";
            this.testingSpreadsheet.GetCell("A1").ListVariableNames = new List<string>();
            this.testingSpreadsheet.GetCell("A1").ListVariableNames.Add("B1");

            this.testingSpreadsheet.GetCell("B1").ListVariableNames = new List<string>();

            string cellReference = "B1";
            string cellName = this.testingSpreadsheet.GetCell("A1").CellName;

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { cellReference, cellName });
            Assert.AreEqual(false, newObject);
        }

        /// <summary>
        /// Testing the private IsCircularReference method with a circular reference.
        /// </summary>
        [Test]
        public void TestIsCircularReferenceIsCircular()
        {
            MethodInfo methodInfo = this.GetMethod("IsCircularReference");

            this.testingSpreadsheet.GetCell("A1").Text = "B1";
            this.testingSpreadsheet.GetCell("B1").Text = "A1";

            this.testingSpreadsheet.GetCell("A1").ListVariableNames = new List<string>();
            this.testingSpreadsheet.GetCell("A1").ListVariableNames.Add("B1");

            this.testingSpreadsheet.GetCell("B1").ListVariableNames = new List<string>();
            this.testingSpreadsheet.GetCell("B1").ListVariableNames.Add("A1");

            string cellReference = "B1";
            string cellName = this.testingSpreadsheet.GetCell("A1").CellName;

            var newObject = methodInfo.Invoke(
                this.testingSpreadsheet, // the object on which we are invoking the method
                new object[] { cellReference, cellName });
            Assert.AreEqual(true, newObject);
        }

        /// <summary>
        /// This allows me to call private methods in the spreadSheet class so I can test them.
        /// </summary>
        /// <param name="methodName">string value, name of the private method I want to test.</param>
        /// <returns>MethodInfo.</returns>
        private MethodInfo GetMethod(string methodName)
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
    }
}
