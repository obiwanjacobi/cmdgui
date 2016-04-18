using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// A binding definition between a command line argument and the gui control.
    /// </summary>
    [XmlType("binding", Namespace = XmlNamespaces.CommandLineSchema)]
    public class Binding : ConfigurationObject
    {
        /// <summary>
        /// The name of the binding as presented to the user.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Information on what control to use to represent the command line argument to the user.
        /// </summary>
        [XmlElement("control")]
        public BindingControl Control { get; set; }

        /// <summary>
        /// A reference to the command line argument definition.
        /// </summary>
        [XmlElement("bindTo")]
        public BindingArgument Argument { get; set; }

        /// <summary>
        /// An optional list of value for multi-valued arguments or option-value lists.
        /// </summary>
        [XmlElement("value")]
        public BindingValueList Values { get; set; }
    }
}