namespace CannedBytes.CommandLineGui.Commands
{
    class FileSaveAllHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public FileSaveAllHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.FileSaveAll;
        }

        protected override bool CanExecute(object parameter)
        {
            if (base.CanExecute(parameter))
            {
                return (_pageMgr.Documents.Count > 0);
            }

            return false;
        }

        protected override bool Execute(object parameter)
        {
            ErrorHandler(() =>
                {
                    _pageMgr.SaveAll();
                });

            return base.Execute(parameter);
        }
    }
}