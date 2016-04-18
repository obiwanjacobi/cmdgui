using System.Collections.Generic;
using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// A grouping definition used to organize bindings.
    /// </summary>
    [XmlType("bindingGroup", Namespace = XmlNamespaces.CommandLineSchema)]
    public class BindingGroup : ConfigurationObject
    {
        /// <summary>
        /// The name of the binding group.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// A helpful description about the group of bindings.
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// An optional indication of the control type to use for the group.
        /// </summary>
        [XmlElement("control")]
        public GroupControl Control { get; set; }

        /// <summary>
        /// A list of either <see cref="Binding"/> or <see cref="BindingGroup"/> instances.
        /// </summary>
        [XmlElement("binding", typeof(Binding))]
        [XmlElement("bindingGroup", typeof(BindingGroup))]
        public List<object> BindingGroupsAndBindings { get; set; }
    }
}