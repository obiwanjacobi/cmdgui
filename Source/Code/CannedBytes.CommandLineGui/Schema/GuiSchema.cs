using System.Linq;
using CannedBytes.CommandLineGui.Schema.Version1;

namespace CannedBytes.CommandLineGui.Schema
{
    class GuiSchema
    {
        /// <summary>
        /// A file path to the schema file that was used to load the tool definitions.
        /// </summary>
        public string SchemaFilePath { get; set; }

        /// <summary>
        /// The root of the tool config file.
        /// </summary>
        public CommandLineGuiConfig ToolConfig { get; set; }

        public Executable FindExecutable(string toolName)
        {
            return (from exec in ToolConfig.Executables
                    where exec.Name == toolName
                    select exec).FirstOrDefault();
        }
    }
}