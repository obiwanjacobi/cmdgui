using System.Linq;
using CannedBytes.CommandLineGui.UI;

namespace CannedBytes.CommandLineGui.Commands
{
    class FileNewHandler : CommandHandler
    {
        private GuiDocumentManager _pageMgr;

        public FileNewHandler(GuiDocumentManager pageMgr)
        {
            _pageMgr = pageMgr;

            Command = AppCommands.FileNew;
        }

        protected override bool Execute(object parameter)
        {
            ErrorHandler(() =>
                {
                    var selSchema = new SelectSchema();

                    selSchema.ExecutableNames = from schema in _pageMgr.SchemaManager.GuiSchemas
                                                from exec in schema.ToolConfig.Executables
                                                select new GuiSchemaInfo { ExecutableName = exec.Name, SchemaFilePath = schema.SchemaFilePath, ScreenName = exec.Gui.Name };

                    var window = ToolWindow.Create(selSchema, "Select a Schema");

                    var result = window.ShowDialog();

                    if (result.HasValue && result.Value)
                    {
                        if (selSchema.IsNewSchema)
                        {
                            int index = _pageMgr.NewAll(selSchema.SchemaPath);

                            _pageMgr.ActiveDocument = _pageMgr.Documents[index];
                        }
                        else
                        {
                            var selected = (from schema in _pageMgr.SchemaManager.GuiSchemas
                                            where schema.SchemaFilePath == selSchema.Selected.SchemaFilePath
                                            from exec in schema.ToolConfig.Executables
                                            where exec.Name == selSchema.Selected.ExecutableName
                                            where exec.Gui.Name == selSchema.Selected.ScreenName
                                            select new { Schema = schema, Executable = exec }).FirstOrDefault();

                            _pageMgr.ActiveDocument = _pageMgr.AddDocument(selected.Schema, selected.Executable);
                        }
                    }

                    //var fm = new FileFilterManager();
                    //fm.AddGuiSchemaFilter();
                    //fm.AddAllFilesFilter();

                    //var ofd = ControlFactory.CreateOpenFileDialog(
                    //    "Select a Command-Line Gui definition file.",
                    //    fm.ToString());

                    //if (ofd.ShowDialog(new Win32Window(App.Current.MainWindow)) == System.Windows.Forms.DialogResult.OK)
                    //{
                    //    int index = _pageMgr.NewAll(ofd.FileName);

                    //    _pageMgr.ActiveDocument = _pageMgr.Documents[index];
                    //}
                });

            return base.Execute(parameter);
        }
    }
}