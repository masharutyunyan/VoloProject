using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;
using BooksCatalogue.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BooksCatalogue.Meneger
{
    public class Meneger
    {
        private BooksCatalogueEntities1 context;
        public Meneger(BooksCatalogueEntities1 context)
        {
            this.context = context;
        }
        public Meneger()
        {

        }
        public static  List<ViewBookModel> GetBoooks()
        {
            BooksCatalogueEntities1 context = new BooksCatalogueEntities1();
            ViewBookModel book = new ViewBookModel();
            List<ViewBookModel> books = new List<ViewBookModel>();
            foreach (var  item in context.Books)
            {
                book.ID = item.ID;
                book.PageCount = Convert.ToInt32(item.PageCount);
                book.PictureName = item.PictureName;
                book.Price = Convert.ToDecimal(item.Price);
                book.BookName = item.BookName;
                book.AuthorName = context.Authors.Find(item.AuthorID).FirstName;
                book.CountryName= context.Countries.Find(item.CountryID).CountryName; 
               // string temp = Convert.ToString(item.PublishDate).ToString("dd/MM/yyyy");
               // book.PublishDate = temp.ToString("dd/MM/yyyy");
                books.Add(book);         
            }
            return books;
        }
        public static async  Task<List<MyAttribute>> GetAttribbutes()
        {
                List<MyAttribute> attributeList = new List<MyAttribute>();
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                MyAttribute attribute = new MyAttribute();
                var temp =  await context.Attributes.ToListAsync();
                foreach (var item in temp)
                { 
                    attribute.ID = item.ID;
                    attribute.AttributesType = item.AttributesType;
                    attribute.AttributName = item.AttributName;
                    attribute.AttributValues = item.AttributValues;
                    attribute.TypeID = item.TypeID;
                    attributeList.Add(attribute);

                }
            }
                return attributeList;
        }
        public static void SaveDB(MyAttribute attribute)
        {
            using (BooksCatalogueEntities1 Db = new BooksCatalogueEntities1())
            {
                Entity.Attribute atrDb = new Entity.Attribute();
                atrDb.AttributesType = attribute.AttributesType;
                atrDb.AttributName = attribute.AttributName;
                atrDb.AttributValues = attribute.AttributValues;
                atrDb.ID = attribute.ID;
                atrDb.TypeID = attribute.TypeID;
                Db.Attributes.Add(atrDb);
                Db.SaveChanges();
            }


        }
    } }
    