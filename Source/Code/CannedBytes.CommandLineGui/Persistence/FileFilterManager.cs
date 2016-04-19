using System;
using System.Collections.Generic;
using System.Text;

namespace CannedBytes.CommandLineGui.Persistence
{
    class FileFilterManager
    {
        public const string Separator = "|";

        public FileFilterManager()
        {
            Filters = new List<FileFilter>();
        }

        public List<FileFilter> Filters { get; private set; }

        public void AddParsed(string filterString)
        {
            if (String.IsNullOrEmpty(filterString)) return;

            var parts = filterString.Split('|');

            foreach (var part in parts)
            {
                Add(part);
            }
        }

        public void AddAssembliesFilter()
        {
            Add("*.dll;*.exe", "Assembly Files (*.dll, *.exe)");
        }

        public void AddCommandLineGuiFilter()
        {
            Filters.Add(CreateCommandLineGuiFilter());
        }

        public void AddGuiSchemaFilter()
        {
            Add("*.gui", "Gui Definition Files (*.gui)");
        }

        public void AddAllFilesFilter()
        {
            Add("*.*", "All Files (*.*)");
        }

        public void Add(string filter)
        {
            Add(filter, null);
        }

        public void Add(string filter, string description)
        {
            var filFilter = new FileFilter();
            filFilter.Description = description;
            filFilter.Filter = filter;

            Filters.Add(filFilter);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var filter in Filters)
            {
                if (builder.Length > 0)
                {
                    builder.Append(Separator);
                }

                builder.Append(filter.ToString());
            }

            return builder.ToString();
        }

        public static FileFilter CreateCommandLineGuiFilter()
        {
            var fileFilter = new FileFilter();
            fileFilter.Description = "Command Line Gui Files (*.clg)";
            fileFilter.Filter = "*.clg";

            return fileFilter;
        }
    }

    class FileFilter
    {
        public string Description { get; set; }

        public string Filter { get; set; }

        public override string ToString()
        {
            var description = Description;

            if (String.IsNullOrEmpty(description))
            {
                int index = Filter.IndexOf('.');

                if (index > 0)
                {
                    description = Filter.Substring(index) + " Files (" + Filter + ")";
                }
                else
                {
                    description = Filter + " Files";
                }
            }

            return string.Format("{0}{1}{2}", description, FileFilterManager.Separator, Filter);
        }
    }
}