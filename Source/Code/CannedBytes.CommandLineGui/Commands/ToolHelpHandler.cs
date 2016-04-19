using System;

namespace CannedBytes.CommandLineGui.Commands
{
    class ToolHelpHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public ToolHelpHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.ToolHelp;
        }

        protected override bool CanExecute(object parameter)
        {
            if (base.CanExecute(parameter))
            {
                if (_pageMgr.ActiveDocument != null &&
                    !_pageMgr.ActiveDocument.ToolInfo.IsToolFileMissing)
                {
                    if (parameter != null && parameter is string)
                    {
                        return !String.IsNullOrEmpty((string)parameter);
                    }
                    else
                    {
                        return !String.IsNullOrEmpty(_pageMgr.ActiveDocument.ToolInfo.Tool.HelpCmd);
                    }
                }
            }

            return false;
        }

        protected override bool Execute(object parameter)
        {
            ErrorHandler(() =>
                {
                    string command = null;

                    var guiDoc = _pageMgr.ActiveDocument;

                    if (guiDoc != null)
                    {
                        if (parameter != null && parameter is string)
                        {
                            command = (string)parameter;
                        }
                        else
                        {
                            command = guiDoc.ToolInfo.Tool.HelpCmd;
                        }

                        ToolExecuteHandler.StartConsole(guiDoc.ToolInfo, command);
                    }
                });

            return base.Execute(parameter);
        }
    }
}