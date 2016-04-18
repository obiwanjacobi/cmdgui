using System.Collections.Generic;

namespace CannedBytes.CommandLineGui.Model
{
    abstract class ModelObject
    {
        public IEnumerable<KeyValuePair<string, string>> Properties { get; set; }
    }
}