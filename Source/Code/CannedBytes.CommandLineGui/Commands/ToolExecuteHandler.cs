using System;
using System.IO;
using CannedBytes.CommandLineGui.Schema;
using CannedBytes.CommandLineGui.UI;

namespace CannedBytes.CommandLineGui.Commands
{
    class ToolExecuteHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public ToolExecuteHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.ExecuteCommandLine;
        }

        protected override bool CanExecute(object parameter)
        {
            if (base.CanExecute(parameter))
            {
                return (_pageMgr.ActiveDocument != null &&
                    !_pageMgr.ActiveDocument.ToolInfo.IsToolFileMissing);
            }

            return false;
        }

        protected override bool Execute(object parameter)
        {
            ErrorHandler(() =>
                {
                    var guiDoc = _pageMgr.ActiveDocument;

                    if (guiDoc != null)
                    {
                        var builder = new CommandLineBuilder(guiDoc.ToolInfo.Tool);
                        var commandLine = builder.Build(guiDoc.ToolBindingModel);

                        StartConsole(guiDoc.ToolInfo, commandLine.ToString());
                    }
                });

            return base.Execute(parameter);
        }

        internal static void StartConsole(ToolInfo toolInfo, string command)
        {
            if (toolInfo == null) return;
            if (String.IsNullOrEmpty(command)) return;

            if (!File.Exists(toolInfo.ToolExecutablePath))
            {
                throw new FileNotFoundException(
                    string.Format("Tool executable file '{0}' can not be found.", toolInfo.ToolExecutablePath),
                    toolInfo.ToolExecutablePath);
            }

            // create UI
            var textOutput = new ConsoleOutput();
            var toolWnd = ToolWindow.Create(textOutput, "Console");

            var host = new ToolExecutionHost(toolInfo, textOutput);
            host.BeginExecute(command);

            toolWnd.ShowDialog();
        }
    }
}