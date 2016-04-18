using System.IO;

namespace CannedBytes.CommandLineGui.Persistence
{
    class CommandLineGuiFileTypeProvider : IFileTypeProvider
    {
        public CommandLineGuiFileTypeProvider()
        {
            FileFilter = FileFilterManager.CreateCommandLineGuiFilter();
        }

        public FileFilter FileFilter { get; private set; }

        public void Serialize(GuiDocument guiDocument, Stream output)
        {
            var writer = new FileWriterVersion1(output);

            writer.Write(guiDocument);
        }
    }
}