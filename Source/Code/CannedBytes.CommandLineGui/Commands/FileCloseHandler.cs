namespace CannedBytes.CommandLineGui.Commands
{
    class FileCloseHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public FileCloseHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.FileClose;
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
                        int index = _pageMgr.Documents.IndexOf(_pageMgr.ActiveDocument);

                        _pageMgr.Documents.Remove(_pageMgr.ActiveDocument);

                        if (index < _pageMgr.Documents.Count)
                        {
                            _pageMgr.ActiveDocument = _pageMgr.Documents[index];
                        }
                        else if (_pageMgr.Documents.Count > 0)
                        {
                            _pageMgr.ActiveDocument = _pageMgr.Documents[_pageMgr.Documents.Count - 1];
                        }
                        else
                        {
                            _pageMgr.ActiveDocument = null;
                        }
                    }
                });

            return base.Execute(parameter);
        }
    }
}