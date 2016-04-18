using CannedBytes.CommandLineGui.Persistence;
using CannedBytes.CommandLineGui.UI;

namespace CannedBytes.CommandLineGui.Commands
{
    class FileOpenHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public FileOpenHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.FileOpen;
        }

        protected override bool Execute(object parameter)
        {
            ErrorHandler(() =>
                {
                    var fm = new FileFilterManager();
                    fm.AddCommandLineGuiFilter();
                    fm.AddAllFilesFilter();

                    var ofd = ControlFactory.CreateOpenFileDialog("Select a Command Line Gui file", fm.ToString());

                    if (ofd.ShowDialog(new Win32Window(App.Current.MainWindow)) == System.Windows.Forms.DialogResult.OK)
                    {
                        var guiDocument = _pageMgr.Open(ofd.FileName);

                        if (guiDocument != null)
                        {
                            // activate loaded doc.
                            _pageMgr.ActiveDocument = guiDocument;
                        }
                    }
                });

            return base.Execute(parameter);
        }
    }
}