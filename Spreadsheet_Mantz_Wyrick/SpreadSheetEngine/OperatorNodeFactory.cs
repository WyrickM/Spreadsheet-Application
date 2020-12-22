// <copyright file="OperatorNodeFactory.cs" company="Mantz Wyrick - 11504292">
// Copyright (c) Mantz Wyrick - 11504292. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CptS321;
using static CptS321.OperatorNode;

namespace SpreadSheetEngine
{
    /// <summary>
    /// OperatorNodeFactory that checks the operators in the expressions then returns
    /// the corresponding Operator object.
    /// </summary>
    internal class OperatorNodeFactory
    {
        /// <summary>
        /// Creating a dictionary that will have the character of the operator as keys and the
        /// corresponding OperatorNode classes as values.
        /// </summary>
        private Dictionary<char, Type> treeOperators = new Dictionary<char, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
        /// Calls TraverseAvailableOperators function.
        /// </summary>
        public OperatorNodeFactory()
        {
            // Instantiate the delegate with a lambda expression
            this.TraverseAvailableOperators((op, type) => this.treeOperators.Add(op, type));
        }

        /// <summary>
        /// Delegate function.
        /// </summary>
        /// <param name="op">character operator.</param>
        /// <param name="type">subclass type of the operator class.</param>
        private delegate void OnOperator(char op, Type type);

        /// <summary>
        /// Gets the dictionary treeOperators so can check if a character is in the operators dictionary.
        /// </summary>
        /// <returns>Dictionary.</returns>
        public Dictionary<char, Type> TreeOperators
        {
            get
            {
                return this.treeOperators;
            }
        }

        /// <summary>
        /// Checks the treeOperators dictionary to see if operatorCharacter is in dictionary,
        /// if in dictionary return corresponding operatornode object,
        /// not found in dictionry return null.
        /// </summary>
        /// <param name="operatorCharacter">char value, that is the operator character from the expression.</param>
        /// <returns>OperatorNode subclass that corresponds to the operator character passed in or null.</returns>
        public OperatorNode CreateNewNode(char operatorCharacter)
        {
            if (this.treeOperators.ContainsKey(operatorCharacter))
            {
                object operatorNodeObject = System.Activator.CreateInstance(this.treeOperators[operatorCharacter]);
                if (operatorNodeObject is OperatorNode)
                {
                    return (OperatorNode)operatorNodeObject;
                }
            }

            throw new Exception("Unhandled operator");
        }

        /// <summary>
        /// Checks if the passed in character is a legal operator.
        /// </summary>
        /// <param name="operatorCharacter">single character.</param>
        /// <returns>true or false.</returns>
        public bool IsOperator(char operatorCharacter)
        {
            if (this.treeOperators.ContainsKey(operatorCharacter) == true)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the precedence of the corresponding operator node.
        /// </summary>
        /// <param name="operatorCharacter">operator character.</param>
        /// <returns>the precedence associated with the operator character.</returns>
        public ushort GetPrecedence(char operatorCharacter)
        {
            ushort precedenceValue = 0;
            if (this.treeOperators.ContainsKey(operatorCharacter))
            {
                Type type = this.treeOperators[operatorCharacter];
                PropertyInfo propertyInfo = type.GetProperty("Precedence");
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue(type);
                    if (propertyValue is ushort)
                    {
                        precedenceValue = (ushort)propertyValue;
                    }
                }
            }

            if (operatorCharacter == '(')
            {
                precedenceValue = 10;
            }

            return precedenceValue;
        }

        /// <summary>
        /// Get the associativity of the corresponding operator node.
        /// </summary>
        /// <param name="operatorCharacter">operator character.</param>
        /// <returns>the associativity of the corresponding operator character.</returns>
        public Associative GetAssociativity(char operatorCharacter)
        {
            Associative associativeCharacteristic = Associative.Left;
            if (this.treeOperators.ContainsKey(operatorCharacter))
            {
                Type type = this.treeOperators[operatorCharacter];
                PropertyInfo propertyInfo = type.GetProperty("Associative");
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue(type);
                    if (propertyValue is Associative)
                    {
                        associativeCharacteristic = (Associative)propertyValue;
                    }
                }
            }

            return associativeCharacteristic;
        }

        /// <summary>
        /// This traverses available operators.
        /// </summary>
        /// <param name="onOperator">an operator.</param>
        private void TraverseAvailableOperators(OnOperator onOperator)
        {
            // get the type declaration of OperatorNode
            Type operatorNodeType = typeof(OperatorNode);

            // Iterate over all loaded assemblies:
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Get all types that inherit from our OperatorNode class using LINQ
                IEnumerable<Type> operatorTypes =
                assembly.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));

                // Iterate over those subclasses of OperatorNode
                foreach (var type in operatorTypes)
                {
                    // for each subclass, retrieve the Operator property
                    PropertyInfo operatorField = type.GetProperty("Operator");
                    if (operatorField != null)
                    {
                        // Get the character of the Operator, if the precedence, associativity, and operator fields are static in the subclasses.
                        object value = operatorField.GetValue(type);

                        // If the property is not static, use the following code instead:
                        // object value = operatorField.GetValue(Activator.CreateInstance(type,
                        // new ConstantNode(0), new ConstantNode(0)));
                        if (value is char)
                        {
                            char operatorSymbol = (char)value;

                            // And invoke the function passed as parameter
                            // with the operator symbol and the operator class
                            onOperator(operatorSymbol, type);
                        }
                    }
                }
            }
        }
    }
}
