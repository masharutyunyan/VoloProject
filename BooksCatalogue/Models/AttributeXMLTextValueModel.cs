using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace BooksCatalogue.Models
{
    [SerializableAttribute]
    [XmlRoot("AttributeXMLTextValueModel")]
    public class AttributeXMLTextValueModel
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public int MaxCharactersCount { get; set; }
        public int minCharactersCount { get; set; }
    public AttributeXMLTextValueModel()
        {
          
        }
        
    }
}