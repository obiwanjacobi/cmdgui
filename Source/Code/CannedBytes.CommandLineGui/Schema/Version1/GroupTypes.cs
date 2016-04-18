using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// A enumeration for the types of group binding controls.
    /// </summary>
    [XmlTypeAttribute("groupTypes", Namespace = XmlNamespaces.CommandLineSchema)]
    public enum GroupTypes
    {
        /// <summary>
        /// Just displays the group of bindings inside a container control (Tab or GroupBox).
        /// </summary>
        Display,
        /// <summary>
        /// Displays the bindings inside the group binding as an option list.
        /// </summary>
        Options,
        /// <summary>
        /// Not implemented.
        /// </summary>
        Custom,
    }
}