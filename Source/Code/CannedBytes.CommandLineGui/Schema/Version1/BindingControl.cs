using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// A control definition for a binding.
    /// </summary>
    [XmlType("bindingControl", Namespace = XmlNamespaces.CommandLineSchema)]
    public class BindingControl : ConfigurationObject
    {
        /// <summary>
        /// The type of control to use.
        /// </summary>
        [XmlAttribute("type")]
        public ControlTypes ControlType { get; set; }

        /// <summary>
        /// A fully qualified type name of a WPF control.
        /// </summary>
        [XmlAttribute("customType")]
        public string CustomType { get; set; }
    }
}