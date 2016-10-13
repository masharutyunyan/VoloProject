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
                books.Add(book);         
            }
            return books;
        }
        public static   List<MyAttribute> GetAttributes()
        {
                List<MyAttribute> attributeList = new List<MyAttribute>();
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                var temp =  context.Attributes.ToList();
                foreach (var item in temp)// grel selectov
                { 
                MyAttribute attribute = new MyAttribute();
                    attribute.ID = item.ID;
                    attribute.AttributesType = item.AttributesType;
                    attribute.AttributName = item.AttributName;
                    attribute.AttributValues = item.AttributValues;
                    attribute.TypeID = item.TypeID;

                    //foreach (var itemAttrValue in context.AttributValues)
                    //{
                    //    if (itemAttrValue.AttributID != null)
                    //    {
                    //        AttributValue attrValue = new AttributValue();
                    //        if (itemAttrValue.AttributID == item.ID)
                    //            attrValue = itemAttrValue;
                    //            attribute.AttributValues.Add(attrValue);
                    //    }
                    //}
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
                 //atrDb = Db.Attributes.Find(attribute.ID);
                atrDb.AttributesType = attribute.AttributesType;
                atrDb.AttributName = attribute.AttributName;
                atrDb.AttributValues = attribute.AttributValues;
                atrDb.ID = attribute.ID;
                atrDb.TypeID = attribute.TypeID;
                Db.Attributes.Add(atrDb);
                Db.SaveChanges();
            }


        }
        public static void SaveEdit(MyAttribute attribute)
        {
            using (BooksCatalogueEntities1 Db = new BooksCatalogueEntities1())
            {
                Entity.Attribute atrDb = Db.Attributes.Find(attribute.ID);
                atrDb.AttributesType = attribute.AttributesType;
                atrDb.AttributName = attribute.AttributName;
                atrDb.AttributValues = attribute.AttributValues;
                atrDb.ID = attribute.ID;
                atrDb.TypeID = attribute.TypeID;
                Db.SaveChanges();
            }


        }
        public static MyAttribute Find(int? id)
        {
            MyAttribute attribute = new MyAttribute();
            using(BooksCatalogueEntities1 context = new BooksCatalogueEntities1()) 
                {
                var atrDb = context.Attributes.Find(id);
                attribute.AttributesType = atrDb.AttributesType;
                attribute.AttributName = atrDb.AttributName;
                attribute.AttributValues = atrDb.AttributValues;
                attribute.ID = atrDb.ID;
                attribute.TypeID = atrDb.TypeID;
               

            }

            return attribute;
        }
        public static void Remove(MyAttribute attribute)
        {
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                var atrDb = context.Attributes.Find(attribute.ID);
                context.Attributes.Remove(atrDb);
                context.SaveChanges();

            }
        }
       
    } }
    