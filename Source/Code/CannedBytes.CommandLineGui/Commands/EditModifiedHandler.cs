namespace CannedBytes.CommandLineGui.Commands
{
    class EditModifiedHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public EditModifiedHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.Modified;
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
            if (_pageMgr.ActiveDocument != null)
            {
                _pageMgr.ActiveDocument.IsChanged = true;
            }

            return base.Execute(parameter);
        }
    }
}