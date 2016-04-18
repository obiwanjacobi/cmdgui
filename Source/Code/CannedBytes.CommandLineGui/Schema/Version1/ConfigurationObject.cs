using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    public class ConfigurationObject
    {
        [XmlAnyAttributeAttribute()]
        public List<XmlAttribute> Attributes { get; set; }

        private IEnumerable<KeyValuePair<string, string>> _properties;

        public IEnumerable<KeyValuePair<string, string>> PropertyBag
        {
            get
            {
                if (_properties == null && Attributes != null)
                {
                    _properties = from attr in Attributes
                                  select new KeyValuePair<string, string>(attr.Name, attr.Value);
                }

                return _properties;
            }
        }
    }
}