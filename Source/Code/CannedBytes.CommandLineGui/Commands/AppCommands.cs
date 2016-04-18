using System.Windows.Input;

namespace CannedBytes.CommandLineGui.Commands
{
    static class AppCommands
    {
        public static RoutedUICommand ToolHelp = ApplicationCommands.Help;
        public static RoutedUICommand FileNew = ApplicationCommands.New;
        public static RoutedUICommand FileOpen = ApplicationCommands.Open;
        public static RoutedUICommand FileSave = ApplicationCommands.Save;
        public static RoutedUICommand FileSaveAs = ApplicationCommands.SaveAs;
        public static RoutedUICommand FileClose = ApplicationCommands.Close;
        public static RoutedCommand EditCopy = ApplicationCommands.Copy;
        public static RoutedCommand FileSaveAll = new RoutedCommand("Save All", typeof(AppCommands));
        public static RoutedCommand Modified = new RoutedCommand("Modified", typeof(AppCommands));
        public static RoutedCommand NavigateUrl = new RoutedCommand("NavigateUrl", typeof(AppCommands));
        public static RoutedCommand ExecuteCommandLine = new RoutedCommand("ExecuteCommandLine", typeof(AppCommands));
    }
}