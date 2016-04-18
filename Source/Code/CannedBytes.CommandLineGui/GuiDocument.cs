using CannedBytes.CommandLineGui.Model;
using CannedBytes.CommandLineGui.Schema;

namespace CannedBytes.CommandLineGui
{
    /// <summary>
    /// Represents one tool.
    /// </summary>
    sealed class GuiDocument : ObservableObject
    {
        private bool _isChanged;

        /// <summary>
        /// Indication if the document has changed (is dirty).
        /// </summary>
        public bool IsChanged
        {
            get { return _isChanged; }
            set { _isChanged = value; OnPropertyChanged("IsChanged"); }
        }

        private string _documentFilePath;

        /// <summary>
        /// A file path to the document file that stores the command line created.
        /// </summary>
        public string DocumentFilePath
        {
            get { return _documentFilePath; }
            set { _documentFilePath = value; OnPropertyChanged("DocumentFilePath"); }
        }

        public HelpInfo ToolHelpInfo { get; set; }

        /// <summary>
        /// A value indicating if the tool schema is embedded in (saved with) the document file.
        /// </summary>
        public bool EmbedGuiSchema { get; set; }

        /// <summary>
        /// Gui Schema (Configuration) information.
        /// </summary>
        public GuiSchema GuiSchema { get; set; }

        /// <summary>
        /// Tool executable information.
        /// </summary>
        public ToolInfo ToolInfo { get; set; }

        /// <summary>
        /// The binding model for this tool.
        /// </summary>
        public GroupBindingModel ToolBindingModel { get; set; }
    }
}