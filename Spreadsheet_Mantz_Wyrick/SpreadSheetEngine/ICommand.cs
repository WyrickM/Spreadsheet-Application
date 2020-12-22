// <copyright file="ICommand.cs" company="Mantz Wyrick - 11504292">
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
    /// ICommand interface that gets inherited by many other classes.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Execute function that gets overriden by many functions.
        /// </summary>
        void Execute();

        /// <summary>
        /// Unexecute function that gets overrident by many funcitons.
        /// </summary>
        void Unexecute();
    }
}
