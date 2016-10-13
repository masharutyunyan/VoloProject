using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Entity;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;
using BooksCatalogue.Models;

namespace BooksCatalogue.Helper
{
    public static class Helper
    {
        public static IQueryable<Book> Search(string searchText, IQueryable<Book> books)
        {

            var b = books.Where(s => s.BookName.Contains(searchText) || s.Price.ToString().Contains(searchText) || s.Author.FirstName.Contains(searchText));
            return b;
        }
    
    public static IQueryable<Book> Sort(string sortby, IQueryable<Book> sortBooks)
    {
            if (sortby == "title")
                sortBooks = sortBooks.OrderBy(x => x.BookName);
            if (sortby == "price")
                
                sortBooks = sortBooks.OrderBy(x => x.Price);
            if (sortby == "author")
                sortBooks = sortBooks.OrderBy(x => x.Author.FirstName);
            return sortBooks;
        }
       public static AttributeXMLTextValueModel XmlDeSerialization (string str)
        {
            int  indexfirst = str.IndexOf("<Value>") +7;
            int  indexlast = str.IndexOf("</Value>") - indexfirst;
            int  c =  str.Count();
            string s = str.Substring(indexfirst , indexlast);
            AttributeXMLTextValueModel XmlValue = new AttributeXMLTextValueModel();
            XmlValue.Value = s;
            return XmlValue; 
        }

    }
}