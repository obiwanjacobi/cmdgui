using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// Indicates how many times an argument can appear in the command line.
    /// </summary>
    [XmlTypeAttribute("multiplicity", Namespace = XmlNamespaces.CommandLineSchema)]
    public enum Multiplicity
    {
        /// <summary>None, One and multiple of the same argument is allowed.</summary>
        ZeroOrMore,
        /// <summary>None and One of the argument is allowed.</summary>
        ZeroOrOne,
        /// <summary>Only One of the argument is allowed.</summary>
        ExactlyOne,
        /// <summary>One or multiple of the same argument is allowed.</summary>
        OneOrMore,
    }
}