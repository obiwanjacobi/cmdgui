using System;
using System.Windows;

namespace CannedBytes.CommandLineGui
{
    sealed class Program : ITextOutput
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                var program = new Program();

                try
                {
                    program.ExecuteCommandLineArgs(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return;
                }

                return;
            }

            try
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Fatal Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // currently only .clg files are silently executed.
        private void ExecuteCommandLineArgs(string[] args)
        {
            var pageMgr = new GuiDocumentManager();
            pageMgr.ActiveDocument = pageMgr.Open(args[0]);

            var cmdLineBuilder = new CommandLineBuilder(pageMgr.ActiveDocument.ToolInfo.Tool);
            string commandLine = cmdLineBuilder.Build(pageMgr.ActiveDocument.ToolBindingModel);

            var host = new ToolExecutionHost(pageMgr.ActiveDocument.ToolInfo, this);
            host.BeginExecute(commandLine);
        }

        void ITextOutput.Write(string text, bool isError)
        {
            if (isError)
            {
                Console.Error.WriteLine(text);
            }
            else
            {
                Console.WriteLine(text);
            }
        }
    }
}