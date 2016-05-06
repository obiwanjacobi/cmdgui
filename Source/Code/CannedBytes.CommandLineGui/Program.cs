using CannedBytes.CommandLineGui.UI;
using System;
using System.IO;
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
            var prgArgs = new ProgramArguments(args);

            if (!prgArgs.IsEmpty && Path.GetExtension(prgArgs.FileName).ToLower() == ".clg")
            {
                var program = new Program();

                try
                {
                    program.ExecuteCommandLineArgs(prgArgs.FileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                return;
            }

            try
            {
                var app = new App();
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
        private void ExecuteCommandLineArgs(string cmdArgFile)
        {
            var pageMgr = new GuiDocumentManager();
            pageMgr.ActiveDocument = pageMgr.Open(cmdArgFile);

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