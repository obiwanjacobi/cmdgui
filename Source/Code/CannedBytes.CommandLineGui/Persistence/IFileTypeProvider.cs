using System.IO;

namespace CannedBytes.CommandLineGui.Persistence
{
    interface IFileTypeProvider
    {
        FileFilter FileFilter { get; }

        void Serialize(GuiDocument guiDocument, Stream output);
    }
}