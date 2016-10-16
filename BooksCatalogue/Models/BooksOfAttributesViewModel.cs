using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
namespace BooksCatalogue.Models
{
    public class BooksOfAttributesViewModel
    {
        public BooksOfAttributesViewModel()
        {
           
            this.Atributes = new List<Entity.Attribute>();
            this.SelectAttributeList = new List<SelectListItem>();
            this.SelectAttributeValueList = new List<SelectListItem>();
            this.AttributeValues = new List<AttributValue>();
        }
        public int BookID { get; set; }
        public int AttributeID { get; set; }
        public String AttributeName { get; set; }
        public String BookName { get; set; }
        public string AttributeValue { get; set; }
        public List<Entity.Attribute> Atributes { get; set; }
        public List<SelectListItem> SelectAttributeList { get; set; }
        public List<SelectListItem> SelectAttributeValueList { get; set; }

        public List<Entity.AttributValue> AttributeValues { get; set; }

    }
}