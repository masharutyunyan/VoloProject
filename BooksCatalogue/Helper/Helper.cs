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
        public static AttributeXMLTextValueModel XmlTextValueDeSerialization(string str)////bazayum grvac xml formatov atributi arjeq@ vercum e  hamapatsaxan obyekti
        {
            int indexfirst = str.IndexOf("<Value>") + 7;
            int indexlast = str.IndexOf("</Value>") - indexfirst;
            string temp = str.Substring(indexfirst, indexlast);
            AttributeXMLTextValueModel XmlValue = new AttributeXMLTextValueModel();
            XmlValue.Value = temp;
            return XmlValue;
        }

        public static AttributeXMLTextModel XmlTextDeSerialization(string str)//bazayum grvac xml formatov atributi anun@ vercum e  hamapatsaxan obyekti
        {
            AttributeXMLTextModel Xmltext = new AttributeXMLTextModel();
            int indexfirst = str.IndexOf("<Name>") + 6;
            int indexlast = str.IndexOf("</Name>") - indexfirst;
            string temp = str.Substring(indexfirst, indexlast);
            Xmltext.Name = temp;
            indexfirst = str.IndexOf("<MaxCharacterCount>") + 19;
            indexlast = str.IndexOf("</MaxCharacterCount>") - indexfirst;
            temp = str.Substring(indexfirst, indexlast);
            Xmltext.MaxCharacterCount = Int32.Parse(temp);
            indexfirst = str.IndexOf("<MinCharacterCount>") + 19;
            indexlast = str.IndexOf("</MinCharacterCount>") - indexfirst;
            temp = str.Substring(indexfirst, indexlast);
            Xmltext.MinCharacterCount = Int32.Parse(temp);
            return Xmltext;
        }

    }
}