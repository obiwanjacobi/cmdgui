using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// A control definition for a binding group.
    /// </summary>
    [XmlType("groupControl", Namespace = XmlNamespaces.CommandLineSchema)]
    public class GroupControl : ConfigurationObject
    {
        /// <summary>
        /// The type of control.
        /// </summary>
        [XmlAttribute("type")]
        public GroupTypes GroupType { get; set; }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        [XmlAttribute("customType")]
        public string CustomType { get; set; }
    }
}