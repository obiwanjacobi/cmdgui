using System;
using System.IO;
using CannedBytes.CommandLineGui.Schema.Version1;

namespace CannedBytes.CommandLineGui.Schema
{
    class ToolInfo : ObservableObject
    {
        private readonly string _expandedPath;

        public ToolInfo(Executable tool)
        {
            Tool = tool;

            if (!String.IsNullOrEmpty(tool.Location))
            {
                _expandedPath = Environment.ExpandEnvironmentVariables(tool.Location);
                ToolExecutablePath = _expandedPath;
            }
        }

        public ToolInfo(GuiSchema schema, Executable tool)
            : this(tool)
        {
            GuiSchema = schema;
        }

        /// <summary>
        /// Schema information where the tool is defined.
        /// </summary>
        /// <remarks>Can be null for embedded schemas.</remarks>
        public GuiSchema GuiSchema { get; set; }

        private string _toolExecutablePath;

        /// <summary>
        /// A file path to the tool executable.
        /// </summary>
        public string ToolExecutablePath
        {
            get { return _toolExecutablePath; }
            set
            {
                _toolExecutablePath = value;
                OnPropertyChanged("ToolExecutablePath");
                OnPropertyChanged("IsToolExecutablePathOverridden");
            }
        }

        public bool IsToolExecutablePathOverridden
        {
            get
            {
                if (String.IsNullOrEmpty(Tool.Location))
                {
                    return (ToolExecutablePath != Tool.Location);
                }

                return (ToolExecutablePath != _expandedPath);
            }
        }

        public bool IsToolFileMissing
        {
            get { return !File.Exists(ToolExecutablePath); }
        }

        public void RevertToolExecutionPath()
        {
            ToolExecutablePath = _expandedPath;
        }

        /// <summary>
        /// The executable tool definitions.
        /// </summary>
        public Executable Tool { get; set; }
    }
}