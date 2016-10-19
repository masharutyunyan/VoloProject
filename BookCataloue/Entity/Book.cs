//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.Books_of_Attributes = new HashSet<Books_of_Attributes>();
            this.UserBooks = new HashSet<UserBook>();
            this.Atributes = new List<Attribute>();
            this.AttributeValues = new List<AttributValue>();
            this.SelectAttributeList = new List<SelectListItem>();
            this.SelectAttributeValueList = new List<SelectListItem>();
        }
    
        public int ID { get; set; }
        public string BookName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Description { get; set; }
        public string PictureName { get; set; }
        public Nullable<int> PageCount { get; set; }
        public Nullable<System.DateTime> PublishDate { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> AuthorID { get; set; }
    
        public virtual Author Author { get; set; }
        public virtual Country Country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Books_of_Attributes> Books_of_Attributes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserBook> UserBooks { get; set; }
        public virtual List<Entity.Attribute> Atributes { get; set; }
        public virtual List<Entity.AttributValue> AttributeValues { get; set; }
        public virtual List<SelectListItem> SelectAttributeList { get; set; }
        public virtual List<SelectListItem> SelectAttributeValueList { get; set; }

    }
}
