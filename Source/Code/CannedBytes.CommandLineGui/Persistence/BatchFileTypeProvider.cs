using System.IO;

namespace CannedBytes.CommandLineGui.Persistence
{
    class BatchFileTypeProvider : IFileTypeProvider
    {
        public BatchFileTypeProvider()
        {
            FileFilter = new FileFilter();
            FileFilter.Description = "Batch files (*.bat, *.cmd)";
            FileFilter.Filter = "*.bat;*.cmd";
        }

        public FileFilter FileFilter { get; private set; }

        public void Serialize(GuiDocument guiDocument, Stream output)
        {
            var writer = new StreamWriter(output);

            var builder = new CommandLineBuilder(guiDocument.ToolInfo.Tool);
            var cmdLine = builder.Build(guiDocument.ToolBindingModel);

            writer.WriteLine("REM This file was generated with the Command Line Gui application. (c) 2011 Canned Bytes.");
            writer.WriteLine();
            writer.Write("\"");
            writer.Write(guiDocument.ToolInfo.ToolExecutablePath);
            writer.Write("\" ");
            writer.Write(cmdLine);
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("pause");

            writer.Flush();
        }
    }
}