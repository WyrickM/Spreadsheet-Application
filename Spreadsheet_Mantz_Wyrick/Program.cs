// <copyright file="Program.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spreadsheet_Mantz_Wyrick;

namespace Spreadsheet_Mantz_Wyrick
{
    /// <summary>
    /// Runs the main program for the winforms app.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">string[] args.</param>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // Spreadsheet_Mantz_Wyrick properties --> application --> output type --> changed from windows app to console app.
        }
    }
}
