using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// A value definition for a binding.
    /// </summary>
    [XmlType("bindingValue", Namespace = XmlNamespaces.CommandLineSchema)]
    public class BindingValue : ConfigurationObject
    {
        /// <summary>
        /// Optional. When specified it is used as the actual argument value on the command line.
        /// </summary>
        [XmlAttribute("key")]
        public string Key { get; set; }

        /// <summary>
        /// The value that is presented to the user.
        /// </summary>
        /// <remarks>
        /// When <see cref="Key"/> is null or empty this value is also used as an argument value.
        /// </remarks>
        [XmlText]
        public string Value { get; set; }
    }
}