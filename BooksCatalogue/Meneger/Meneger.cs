using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;
using BooksCatalogue.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.IO;

namespace BooksCatalogue.Meneger
{
    public class Meneger
    {
       public Meneger()
        {
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
                    attribute.AttributeTextXmlName = Helper.Helper.XmlTextDeSerialization(item.AttributName);
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
                attribute.AttributeTextXmlName = Helper.Helper.XmlTextDeSerialization(atrDb.AttributName);
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
        public static bool IsBooksAttribut(int? bookID ,int AttributID)
        { BooksCatalogueEntities1 context = new BooksCatalogueEntities1();
            var booksAttribute = context.Books_of_Attributes.ToList();
            foreach (var item in context.Books_of_Attributes.ToList() )
            {
                if (item.AttributesID == AttributID && item.BooksID == bookID)
                    return true;
            } 
         return false;
        }
        public static Books_of_Attributes FindBookOfAttribute(int? bookID, int AttributID)
        {
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                var booksAttribute = context.Books_of_Attributes.ToList();
                foreach (var item in context.Books_of_Attributes.ToList())
                {
                    if (item.AttributesID == AttributID && item.BooksID == bookID)
                        return item;
                }
            }
            return null;
        }
        public static void DeleteBooksOfAttribute(int? bookID,int attributeID)
        {

            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                foreach (var item in context.Books_of_Attributes)
                {  
                    if (item.BooksID == bookID && item.AttributesID == attributeID)
                    {
                        context.Books_of_Attributes.Remove(item);
                        break;
                    } 
                 }
                        context.SaveChanges();
            }
            
        }
        public static List<BooksOfAttributesViewModel> GetBooksOfAttributeList(int? bookID)
        {
            List<BooksOfAttributesViewModel> booksOfAttributes = new List<BooksOfAttributesViewModel>();
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                var bookOfAttributeTemp = context.Books_of_Attributes.Where(x => x.BooksID == bookID).ToList();
                foreach (var item in bookOfAttributeTemp)
                {
                    BooksOfAttributesViewModel BooksOfAttribute = new BooksOfAttributesViewModel();
                    BooksOfAttribute.AttributeID = item.AttributesID;
                    BooksOfAttribute.BookID = item.BooksID;
                    BooksOfAttribute.AttributeName = context.Books.Find(item.BooksID).BookName;
                    BooksOfAttribute.AttributeValue = Helper.Helper.XmlTextValueDeSerialization(context.AttributValues.Find(item.AttributesID).AttributValue1).Value;
                    int? id = context.AttributValues.Find(item.AttributesID).AttributID;
                    BooksOfAttribute.AttributeName = Helper.Helper.XmlTextDeSerialization(context.Attributes.Find(id).AttributName).Name;
                    booksOfAttributes.Add(BooksOfAttribute);
                }
               
            }
            return booksOfAttributes;
        }
       public static BooksOfAttributesViewModel GetBooksOfAttribute(int? bookID, int AttributeID)
        {
            BooksOfAttributesViewModel BooksOfAttribute = new BooksOfAttributesViewModel();
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                var bookTemp = context.Books.Find(bookID);
                var AttributeValueTemp = context.AttributValues.Find(AttributeID);
                var AttributeTemp = context.Attributes.Find(AttributeValueTemp.AttributID);
                BooksOfAttribute.BookName = bookTemp.BookName;
                BooksOfAttribute.AttributeID = AttributeID;
                BooksOfAttribute.BookID = bookTemp.ID;
                BooksOfAttribute.AttributeName = Helper.Helper.XmlTextDeSerialization( AttributeTemp.AttributName).Name;
                BooksOfAttribute.AttributeValue = Helper.Helper.XmlTextValueDeSerialization(AttributeValueTemp.AttributValue1).Value;
                BooksOfAttribute.Atributes = context.Attributes.ToList();
                List<SelectListItem> selctedAttributes = new List<SelectListItem>();
                List<AttributeXMLTextModel> attrList = new List<AttributeXMLTextModel>();
                foreach (var item in BooksOfAttribute.Atributes)
                {
                    AttributeXMLTextModel atrxml = new AttributeXMLTextModel();
                    atrxml = Helper.Helper.XmlTextDeSerialization(item.AttributName);
                    atrxml.ID = item.ID;
                    attrList.Add(atrxml);
                }
                attrList.ForEach(x =>
                {
                    selctedAttributes.Add(new SelectListItem
                    { Text = x.Name, Value = x.ID.ToString() });
                });
                BooksOfAttribute.SelectAttributeList = selctedAttributes;
            }
            return BooksOfAttribute;
        }
        public static List<SelectListItem> GetAttributeValueByAttributeID( int AttributeID)
        {
            List<Entity.AttributValue> attrlit = new List<AttributValue>();
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                attrlit = context.AttributValues.Where(x => x.AttributID == AttributeID).ToList();
            }
            //attribut value
            List<SelectListItem> selctedAttributesValue = new List<SelectListItem>();
            List<AttributeXMLTextValueModel> attrValueList = new List<AttributeXMLTextValueModel>();
            foreach (var item in attrlit)
            {
                AttributeXMLTextValueModel atrValueXml = new AttributeXMLTextValueModel();
                atrValueXml = Helper.Helper.XmlTextValueDeSerialization(item.AttributValue1);
                atrValueXml.AttributeValueID = item.ID;
                attrValueList.Add(atrValueXml);
            }
            attrValueList.ForEach(x =>
            {
                selctedAttributesValue.Add(new SelectListItem
                { Text = x.Value, Value = x.AttributeValueID.ToString() });
            });
           

            return selctedAttributesValue;
        }
        public static Book SetInBooksAttributes(int? bookID, int AttributeID)
        {
            Book book = new Book();
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                book = context.Books.Find(bookID);
                book.Atributes = context.Attributes.ToList();
                book.AttributeValues = context.AttributValues.Where(x => x.AttributID == AttributeID).ToList();
            }
                List<SelectListItem> selctedAttributes = new List<SelectListItem>();
                List<AttributeXMLTextModel> attrList = new List<AttributeXMLTextModel>();
                foreach (var item in book.Atributes)
                {
                    AttributeXMLTextModel atrxml = new AttributeXMLTextModel();
                    atrxml = Helper.Helper.XmlTextDeSerialization(item.AttributName);
                    atrxml.ID = item.ID;
                    attrList.Add(atrxml);
                }
                attrList.ForEach(x =>
                {
                    selctedAttributes.Add(new SelectListItem
                    { Text = x.Name, Value = x.ID.ToString() });
                });
                book.SelectAttributeList = selctedAttributes;
            //attribut value
            List<SelectListItem> selctedAttributesValue = new List<SelectListItem>();
            List<AttributeXMLTextValueModel> attrValueList = new List<AttributeXMLTextValueModel>();
            foreach (var item in book.AttributeValues)
            {
                AttributeXMLTextValueModel atrValueXml = new AttributeXMLTextValueModel();
                atrValueXml = Helper.Helper.XmlTextValueDeSerialization(item.AttributValue1);
                atrValueXml.AttributeValueID = item.ID;
                attrValueList.Add(atrValueXml);
            }
            attrValueList.ForEach(x =>
            {
                selctedAttributesValue.Add(new SelectListItem
                { Text = x.Value, Value = x.AttributeValueID.ToString() });
            });
            book.SelectAttributeValueList = selctedAttributesValue;

            return book;
            }
     
       public static bool ValidationAttributeValue(string attrValue, int? id)
        {


           Entity.Attribute attr = new Entity.Attribute();
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                attr = context.Attributes.Find(id);
            }
                AttributeXMLTextModel attrNameTemp = Helper.Helper.XmlTextDeSerialization(attr.AttributName);
            if(attrNameTemp.MaxCharacterCount.ToString().Length > attrValue.Length && attrNameTemp.MinCharacterCount.ToString().Length < attrValue.Length)
            {
                return true;
            }
                return false;
        }
        public static void AddAttributeValue(string value, int id)
        {
            AttributeXMLTextValueModel atrValue = new AttributeXMLTextValueModel();
            atrValue.Value = value;

            System.Xml.Serialization.XmlSerializer atrXml = new System.Xml.Serialization.XmlSerializer(atrValue.GetType());
            StringWriter stringWriter = new StringWriter();
            atrXml.Serialize(stringWriter, atrValue);
            AttributValue dbAtrValue = new AttributValue();
            dbAtrValue.AttributID = id;
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                context.AttributValues.Add(dbAtrValue);
                context.SaveChanges();
            }
      
            }
        public static void EditAttribute(MyAttribute attribute,string Name, int TypeID, int? MaxCharacterCount, int? MinCharacterCount)
        {
            MyAttribute atr = new MyAttribute();
            AttributeXMLTextModel attributeTextXmlName = new AttributeXMLTextModel();
            attributeTextXmlName.MaxCharacterCount = MaxCharacterCount;
            attributeTextXmlName.MinCharacterCount = MinCharacterCount;
            attributeTextXmlName.Name = Name;
            System.Xml.Serialization.XmlSerializer atrXml = new System.Xml.Serialization.XmlSerializer(attributeTextXmlName.GetType());
            StringWriter stringWriter = new StringWriter();
            atrXml.Serialize(stringWriter, attributeTextXmlName);
            atr.AttributName = stringWriter.ToString();
            atr.AttributeTextXmlName = attributeTextXmlName;
            atr.TypeID = attribute.TypeID;
            atr.ID = attribute.ID;
            SaveEdit(atr);
        }
        public static void CreateAttribute(string name, int TypeID, int? maxCharackterCount, int? mincharkcterCount)
        {
            MyAttribute atr = new MyAttribute();
            AttributeXMLTextModel attributeTextXmlName = new AttributeXMLTextModel();
            attributeTextXmlName.MaxCharacterCount = maxCharackterCount;
            attributeTextXmlName.MinCharacterCount = mincharkcterCount;
            attributeTextXmlName.Name = name;
            System.Xml.Serialization.XmlSerializer atrXml = new System.Xml.Serialization.XmlSerializer(attributeTextXmlName.GetType());
            StringWriter stringWriter = new StringWriter();
            atrXml.Serialize(stringWriter, attributeTextXmlName);
            atr.AttributName = stringWriter.ToString();
            atr.AttributeTextXmlName = attributeTextXmlName;
            atr.TypeID = TypeID;
            SaveDB(atr);
        }
        public static void EditAttributeValue(string value, int id)
        {
            AttributValue attributeValue = new AttributValue();
            using (BooksCatalogueEntities1 context = new BooksCatalogueEntities1())
            {
                attributeValue = context.AttributValues.Find(id);
                AttributeXMLTextValueModel atrValue = new AttributeXMLTextValueModel();// texapoxel helper
                atrValue = Helper.Helper.XmlTextValueDeSerialization(attributeValue.AttributValue1);
                atrValue.Value = value;
                System.Xml.Serialization.XmlSerializer atrXml = new System.Xml.Serialization.XmlSerializer(atrValue.GetType());
                StringWriter stringWriter = new StringWriter();
                atrXml.Serialize(stringWriter, atrValue);
                attributeValue.AttributValue1 = stringWriter.ToString();
                context.SaveChanges();
            }
        }
    }
} 
    