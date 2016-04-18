using System.Windows;
using CannedBytes.CommandLineGui.Commands;

namespace CannedBytes.CommandLineGui.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    partial class MainWindow : Window
    {
        private readonly GuiDocumentManager _pageMgr = new GuiDocumentManager();

        public MainWindow()
        {
            InitializeComponent();

            // register command handlers (via CommandBinding)
            this.CommandBindings.Add(new FileNewHandler(_pageMgr).ToCommandBinding());
            this.CommandBindings.Add(new FileOpenHandler(_pageMgr).ToCommandBinding());
            this.CommandBindings.Add(new FileSaveHandler(_pageMgr).ToCommandBinding());
            this.CommandBindings.Add(new FileSaveAsHandler(_pageMgr).ToCommandBinding());
            this.CommandBindings.Add(new FileSaveAllHandler(_pageMgr).ToCommandBinding());
            this.CommandBindings.Add(new FileCloseHandler(_pageMgr).ToCommandBinding());
            this.CommandBindings.Add(new EditCopyHandler(_pageMgr).ToCommandBinding());
            this.CommandBindings.Add(new EditModifiedHandler(_pageMgr).ToCommandBinding());
            this.CommandBindings.Add(new ToolExecuteHandler(_pageMgr).ToCommandBinding());
            this.CommandBindings.Add(new ToolHelpHandler(_pageMgr).ToCommandBinding());
            this.CommandBindings.Add(new NavigateUrlHandler(_pageMgr).ToCommandBinding());

            DataContext = _pageMgr;
        }
    }
}