using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// A reference to the command line argument definition.
    /// </summary>
    [XmlType("bindingArgument", Namespace = XmlNamespaces.CommandLineSchema)]
    public class BindingArgument
    {
        /// <summary>
        /// The name of the command line <see cref="Argument"/>.
        /// </summary>
        [XmlAttribute("argument")]
        public string Name { get; set; }
    }
}