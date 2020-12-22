// <copyright file="ColorChange.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Color change class that inherits from the command interface.
    /// Changes the background colors of the selected cells.
    /// </summary>
    public class ColorChange : ICommand
    {
        /// <summary>
        /// private field that holds the new uint color.
        /// </summary>
        private uint newColor;

        /// <summary>
        /// private list of all the original colors from the selected cells.
        /// </summary>
        private List<uint> originalColor;

        /// <summary>
        /// private list of all of the selected cells that we want to change their
        /// background colors.
        /// </summary>
        private List<Cell> selectedCells;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorChange"/> class.
        /// </summary>
        /// <param name="selectedCells">selected cells by user.</param>
        /// <param name="originalColor">original color of the cells.</param>
        /// <param name="newColor">new color we want to change the selected cells to.</param>
        public ColorChange(List<Cell> selectedCells, List<uint> originalColor, uint newColor)
        {
            this.newColor = newColor;
            this.originalColor = originalColor;
            this.selectedCells = selectedCells;
        }

        /// <summary>
        /// overriding the interface command class execute function
        /// to execute the background color change.
        /// </summary>
        public void Execute()
        {
            foreach (Cell cell in this.selectedCells)
            {
                cell.BGColor = this.newColor;
            }
        }

        /// <summary>
        /// overriding the interface command class unexecute function
        /// this will be used for undo.
        /// </summary>
        public void Unexecute()
        {
            for (int index = 0; index < this.selectedCells.Count; index++)
            {
                this.selectedCells[index].BGColor = this.originalColor[index];
            }
        }
    }
}
