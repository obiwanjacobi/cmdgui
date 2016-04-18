using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// A command line Argument definition.
    /// </summary>
    [XmlType("argument", Namespace = XmlNamespaces.CommandLineSchema)]
    public class Argument : ConfigurationObject
    {
        /// <summary>
        /// The name the argument is referenced to within the tool configuration file.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// The formatting of the argument. See also <see cref="M:System.String.Format"/> .
        /// </summary>
        [XmlAttribute("format")]
        public string Format { get; set; }

        /// <summary>
        /// Indicates how many times the argument can appear in a command line.
        /// </summary>
        [XmlAttribute("multiplicity")]
        public Multiplicity Multiplicity { get; set; }

        /// <summary>
        /// A helpful description for a user to understand more about the argument.
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        //[XmlAttribute("dataType")]
        //public string DataType { get; set; }

        /// <summary>
        /// The order in which the argument appears in the command line.
        /// </summary>
        /// <remarks>Duplicate value will be treated as an unordered group.</remarks>
        [XmlAttribute("ordinal")]
        public int Ordnial { get; set; }

        /// <summary>
        /// Nullable indication.
        /// </summary>
        [XmlIgnore]
        public bool OrdinalSpecified { get; set; }
    }
}