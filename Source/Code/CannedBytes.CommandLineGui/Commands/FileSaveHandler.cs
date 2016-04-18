using CannedBytes.CommandLineGui.Persistence;
using CannedBytes.CommandLineGui.UI;

namespace CannedBytes.CommandLineGui.Commands
{
    class FileSaveHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public FileSaveHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.FileSave;
        }

        protected override bool CanExecute(object parameter)
        {
            if (base.CanExecute(parameter))
            {
                return (_pageMgr.ActiveDocument != null &&
                    !string.IsNullOrEmpty(_pageMgr.ActiveDocument.DocumentFilePath));
            }

            return false;
        }

        protected override bool Execute(object parameter)
        {
            ErrorHandler(() =>
                {
                    if (_pageMgr.ActiveDocument != null)
                    {
                        if (string.IsNullOrEmpty(_pageMgr.ActiveDocument.DocumentFilePath))
                        {
                            var fm = new FileFilterManager();
                            fm.AddCommandLineGuiFilter();
                            fm.AddAllFilesFilter();

                            var sfd = ControlFactory.CreateSaveFileDialog("Select a Gui Command Line file name", fm.ToString());

                            if (sfd.ShowDialog(new Win32Window(App.Current.MainWindow)) == System.Windows.Forms.DialogResult.OK)
                            {
                                _pageMgr.ActiveDocument.DocumentFilePath = sfd.FileName;

                                _pageMgr.SaveActive();
                            }
                        }
                        else
                        {
                            _pageMgr.SaveActive();
                        }
                    }
                });

            return base.Execute(parameter);
        }
    }
}