// <copyright file="HistoryCollection.cs" company="Benjamin Michaelis">
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
    /// Holds the history collection.
    /// </summary>
    public class HistoryCollection
    {
        private readonly IHistoryCommand[] _commands;
        public string Name;

        public HistoryCollection(IHistoryCommand[] commands, string name)
        {
            this._commands = commands;
            this.Name = name;
        }

        public HistoryCollection(List<IHistoryCommand> commands, string name)
        {
            this._commands = commands.ToArray();
            this.Name = name;
        }

        public HistoryCollection(RestoreHistory restoreHistory, string cellTextChange)
        {
        }

        public HistoryCollection Restore(Spreadsheet spreadsheet)
        {
            return new HistoryCollection(this._commands.Select(command => command.Execute(spreadsheet)).ToArray(), this.Name);
        }
    }
}
