using CannedBytes.CommandLineGui.Persistence;
using CannedBytes.CommandLineGui.UI;

namespace CannedBytes.CommandLineGui.Commands
{
    class FileSaveAsHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public FileSaveAsHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.FileSaveAs;
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
                    if (_pageMgr.ActiveDocument != null)
                    {
                        var pm = new PersistenceManager();
                        pm.FileTypeProviders.Add(new CommandLineGuiFileTypeProvider());
                        pm.FileTypeProviders.Add(new BatchFileTypeProvider());

                        var sfd = ControlFactory.CreateSaveFileDialog("Select a Gui Command Line file name", pm.GetFileFilter(false));

                        if (sfd.ShowDialog(new Win32Window(App.Current.MainWindow)) == System.Windows.Forms.DialogResult.OK)
                        {
                            // SaveFileDialog uses a 1-based index for FilterIndex.
                            var provider = pm.FileTypeProviders[sfd.FilterIndex - 1];

                            if (sfd.FilterIndex == 1)
                            {
                                _pageMgr.ActiveDocument.DocumentFilePath = sfd.FileName;
                                _pageMgr.ActiveDocument.IsChanged = false;
                            }

                            GuiDocumentManager.SaveAs(_pageMgr.ActiveDocument, sfd.FileName, provider);
                        }
                    }
                });

            return base.Execute(parameter);
        }
    }
}