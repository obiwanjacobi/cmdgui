using System.Collections.Generic;

namespace CannedBytes.CommandLineGui.Persistence
{
    class PersistenceManager
    {
        public PersistenceManager()
        {
            FileTypeProviders = new List<IFileTypeProvider>();
        }

        public List<IFileTypeProvider> FileTypeProviders { get; private set; }

        public string GetFileFilter(bool includeAllFiles)
        {
            var fm = new FileFilterManager();

            foreach (var provider in FileTypeProviders)
            {
                fm.Filters.Add(provider.FileFilter);
            }

            if (includeAllFiles)
            {
                fm.AddAllFilesFilter();
            }

            return fm.ToString();
        }
    }
}