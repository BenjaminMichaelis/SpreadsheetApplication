// <copyright file="UndoRedo.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Logic for undo and redo commands.
    /// </summary>
    public class UndoRedo
    {
        private Stack<HistoryCollection> undoStack = new Stack<HistoryCollection>();
        private Stack<HistoryCollection> redoStack = new Stack<HistoryCollection>();

        /// <summary>
        /// Gets a value indicating whether the redo stack is empty or not.
        /// </summary>
        public bool RedoCount => this.redoStack.Count > 0;

        /// <summary>
        /// Gets a value indicating whether the undo stack is empty or not.
        /// </summary>
        public bool UndoCount => this.undoStack.Count > 0;

        public void AddUndoCommand(HistoryCollection undo)
        {
            this.undoStack.Push(undo);
            this.redoStack.Clear();
        }

        public string CheckUndo => this.UndoCount ? this.undoStack.Peek().Name : string.Empty;

        public string CheckRedo => this.RedoCount ? this.redoStack.Peek().Name : string.Empty;

        /// <summary>
        /// Clear the redo stack.
        /// </summary>
        public void ClearRedo()
        {
            this.redoStack.Clear();
        }

        public void ClearUndo()
        {
            this.undoStack.Clear();
        }

        public void ClearAll()
        {
            this.redoStack.Clear();
            this.undoStack.Clear();
        }

        public void Undo(Spreadsheet sheet)
        {
            HistoryCollection commands = this.undoStack.Pop();
            this.redoStack.Push(commands.Restore(sheet));
        }

        public void Redo(Spreadsheet sheet)
        {
            HistoryCollection commands = this.redoStack.Pop();
            this.undoStack.Push(commands.Restore(sheet));
        }
    }

    public interface IHistoryCommand
    {
        IHistoryCommand Execute(Spreadsheet spreadsheet);
    }
}
