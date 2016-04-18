using System.Windows;

namespace CannedBytes.CommandLineGui.Commands
{
    class EditCopyHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public EditCopyHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.EditCopy;
        }

        protected override bool CanExecute(object parameter)
        {
            if (base.CanExecute(parameter))
            {
                return (_pageMgr.ActiveDocument != null);
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

                        Clipboard.SetText(commandLine);

                        MessageBox.Show(App.Current.MainWindow, commandLine, "Clip Board",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });

            return base.Execute(parameter);
        }
    }
}