using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksCatalogue.Models
{
    public class AttributeXMLTextModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? MaxCharacterCount { get; set; }
        public int? MinCharacterCount { get; set; }

    }
}