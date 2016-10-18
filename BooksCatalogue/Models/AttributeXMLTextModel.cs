using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BooksCatalogue.Models
{
    public class AttributeXMLTextModel
    {
    
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int? MaxCharacterCount { get; set; }
        [Required]
        public int? MinCharacterCount { get; set; }

    }
}