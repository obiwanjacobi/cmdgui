using System.Diagnostics;

namespace CannedBytes.CommandLineGui.Commands
{
    class NavigateUrlHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public NavigateUrlHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.NavigateUrl;
        }

        protected override bool CanExecute(object parameter)
        {
            if (base.CanExecute(parameter))
            {
                if (parameter != null && parameter is string)
                {
                    return !string.IsNullOrEmpty((string)parameter);
                }

                return (_pageMgr.ActiveDocument != null &&
                    !string.IsNullOrEmpty(_pageMgr.ActiveDocument.ToolInfo.Tool.HelpUrl));
            }

            return false;
        }

        protected override bool Execute(object parameter)
        {
            ErrorHandler(() =>
                {
                    string url = null;

                    if (parameter != null && parameter is string)
                    {
                        url = (string)parameter;
                    }
                    else if (_pageMgr.ActiveDocument != null)
                    {
                        url = _pageMgr.ActiveDocument.ToolInfo.Tool.HelpUrl;
                    }

                    if (!string.IsNullOrEmpty(url))
                    {
                        Process.Start(url);
                    }
                });

            return base.Execute(parameter);
        }
    }
}