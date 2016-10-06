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

namespace BooksCatalogue.Helper
{
    public static class Helper 
    {
        private static string FilePath = "~/Picture/";
        public static string SavePicture(HttpPostedFileBase PictureName)
        {
            FileInfo file = new FileInfo(PictureName.FileName);
            string GuIdName = Guid.NewGuid().ToString() + file.Extension;// stugel vor miayn picture tipi filer pahi 
         //   var path = Path.Combine(HttpContext.Server.MapPath(FilePath), GuIdName);
           // PictureName.SaveAs(path);
            return GuIdName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  static void DeletePhoto(string photoFileName)
        {
          //  string fullPath = Request.MapPath(FilePath + photoFileName);
           // if (System.IO.File.Exists(fullPath))
            {
            //    System.IO.File.Delete(fullPath);

            }

        }
        public static IQueryable<Book> Search(string searchText,IQueryable<Book> books)
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
    }
}