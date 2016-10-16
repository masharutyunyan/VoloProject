using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Web.Mvc;
using Entity;
namespace BooksCatalogue.Models
{
    public class MyAttribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MyAttribute()
        {
            this.AttributValues = new HashSet<AttributValue>();
        }
        public AttributeXMLTextModel AttributeTextXmlName { get; set; }
        public int ID { get; set; }
        public string AttributName { get; set; }
        public Nullable<int> TypeID { get; set; }

        public virtual AttributesType AttributesType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AttributValue> AttributValues { get; set; }
    }
}
