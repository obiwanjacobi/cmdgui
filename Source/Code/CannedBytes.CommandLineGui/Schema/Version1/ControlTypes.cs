using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// An enumeration of all built-in control types.
    /// </summary>
    [XmlType("controlTypes", Namespace = XmlNamespaces.CommandLineSchema)]
    public enum ControlTypes
    {
        /// <summary>A read-only literal display of the value.</summary>
        Literal,
        /// <summary>A floating-point number.</summary>
        Number,
        /// <summary>A free text box.</summary>
        Text,
        /// <summary>An integeral number.</summary>
        Integer,
        /// <summary>A text box and a browse button that opens the Open File Dialog.</summary>
        InputFile,
        /// <summary>A text box and a browse button that opens the Save File Dialog.</summary>
        OutputFile,
        /// <summary>A text box and a browse button that opens the Folder Browser Dialog.</summary>
        Folder,
        /// <summary>A multiline text box.</summary>
        Multiline,
        /// <summary>A list with the specified values as a single select option.</summary>
        Options,
        /// <summary>A check box.</summary>
        Check,
        /// <summary>An editable combo box that is filled when opening an Assembly with the browse button.</summary>
        SelectType,
        /// <summary>A uri text box.</summary>
        Uri,
        /// <summary>Not implemented yet.</summary>
        Custom,
    }
}