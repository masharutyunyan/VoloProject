using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksCatalogue.Models
{
    public class ViewBookModel
    {
        public int ID { get; set; }
        public string BookName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string PictureName { get; set; }
        public int PageCount { get; set; }
        public string PublishDate { get; set; }
        public string CountryName { get; set; }
        public string AuthorName { get; set; }

    }
}