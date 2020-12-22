// <copyright file="GlobalSuppressions.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Want fields to be protected so we can access them through spreadsheet class", Scope = "member", Target = "~F:CptS321.Cell.text")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Want fields to be protected so we can access them through spreadsheet class", Scope = "member", Target = "~F:CptS321.Cell.value")]
[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "need to be able to access parentCell in Cell class", Scope = "member", Target = "~F:CptS321.ExpressionTree.parentCell")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "need to be able to access parentCell in Cell class", Scope = "member", Target = "~F:CptS321.ExpressionTree.parentCell")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "For this function we need to pass the exact cell to change the corresponding variable(s) thus we need the cell class to be able to access the function to pass the exact cell.", Scope = "member", Target = "~M:CptS321.ExpressionTree.SetCellVariableInTree(CptS321.Cell)")]
