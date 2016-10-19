using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using System.Threading.Tasks;
using System.Data.Entity;
using BooksCatalogue.Models;
using System.Net;
using System.IO;
using System.Xml;

namespace BooksCatalogue.Controllers
{
    public class AttributeController : Controller
    {
        public  static MyAttribute attribute;
        public static Error error;

        private BooksCatalogueEntities1 context = new BooksCatalogueEntities1();
        // GET: Attribute
        public  ActionResult Index()
        {
            return View( Meneger.Meneger.GetAttributes());
        }
        public ActionResult Create()
        {
            ViewBag.TypeID= new SelectList(context.AttributesTypes, "ID", "AttributeType");
            return View();
        }

        [HttpPost]
         [ValidateAntiForgeryToken]
        public  ActionResult Create(string name, int TypeID, int? maxCharackterCount, int? mincharkcterCount)
        {
            if(name == null || maxCharackterCount == null || mincharkcterCount == null)
            {
                Error errortemp = new Error();
                errortemp.Name = "emptyFild";
                errortemp.Messag = "inpute error or Type error. do not fill in all fields? Or the wrong types of fields filled in/MaxCharackterCount and MincharkcterCount must be int type /";
                error = errortemp;
                return RedirectToAction("Error");
            }
          
            if (maxCharackterCount < mincharkcterCount)
            {
                Error errortemp = new Error();
                errortemp.Name = "CreateAttribute";
                errortemp.Messag = "inpute error. maxCharackterCount(Max) < mincharkcterCount(Min)";
                error = errortemp;
                return RedirectToAction("Error");
            }
            if (ModelState.IsValid)
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
                Meneger.Meneger.SaveDB(atr);
                return RedirectToAction("Index");
            }
            ViewBag.TypeID = new SelectList(context.AttributesTypes, "ID", "AttributeType", attribute.TypeID);
            return View(attribute);

        }
        public  ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var attribute = Meneger.Meneger.Find(id);
            attribute.AttributeTextXmlName = Helper.Helper.XmlTextDeSerialization(attribute.AttributName);
            if (attribute == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeID = new SelectList(context.AttributesTypes, "ID", "AttributeType", attribute.TypeID);
            return View(attribute);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AttributName,TypeID")] MyAttribute attribute, string Name, int TypeID, int? MaxCharacterCount, int? MinCharacterCount)
        {
            if (ModelState.IsValid)
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
                Meneger.Meneger.SaveEdit(atr);
                return RedirectToAction("Index");
            }
            ViewBag.TypeID = new SelectList(context.AttributesTypes, "ID", "AttributeType", attribute.TypeID);
            return View(attribute);
        }
        //Get popoxum e attributi arjeq@
        public ActionResult EditAttributeValue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributeXMLTextValueModel attribute = Helper.Helper.XmlTextValueDeSerialization(context.AttributValues.Find(id).AttributValue1);
            if (attribute == null)
            {
                return HttpNotFound();
            }
            return View(attribute);
        }

        [HttpPost]//post popoxum e atributi arjeq@
        public ActionResult EditAttributeValue(string value, int id)
        {
            if (value != null)
            {
                AttributValue attributeValue = new AttributValue();
                attributeValue = context.AttributValues.Find(id);
                AttributeXMLTextValueModel atrValue = new AttributeXMLTextValueModel();// texapoxel helper
                atrValue = Helper.Helper.XmlTextValueDeSerialization(attributeValue.AttributValue1);
                atrValue.Value = value;
                System.Xml.Serialization.XmlSerializer atrXml = new System.Xml.Serialization.XmlSerializer(atrValue.GetType());
                StringWriter stringWriter = new StringWriter();
                atrXml.Serialize(stringWriter, atrValue);
                attributeValue.AttributValue1 = stringWriter.ToString();
                MyAttribute attrTemp = Meneger.Meneger.Find(attribute.ID);
                attribute = attrTemp;

                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                }
            }
            else
            {
                Error error = new Error();
                error.Messag = "do not enter an attribute value";
             //   return Error(error);
            }
              
                return RedirectToAction("AttributeAddTextValue");
            
        }

        // GET: Delete/1 Attribute value 
        public ActionResult DeleteAttributeValue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributeXMLTextValueModel attribute = Helper.Helper.XmlTextValueDeSerialization(context.AttributValues.Find(id).AttributValue1);
            if (attribute == null)
            {
                return HttpNotFound();
            }
            return View(attribute);
        }
        // POST: Delete Attribute Value
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAttributeValue(int id)
        {
            AttributValue attribute = context.AttributValues.Find(id);
            context.AttributValues.Remove(attribute);
            context.SaveChanges();
            return RedirectToAction("AttributeAddTextValue");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var attribute = Meneger.Meneger.Find(id);
            attribute.AttributeTextXmlName = Helper.Helper.XmlTextDeSerialization(attribute.AttributName);
            if (attribute == null)
            {
                return HttpNotFound();
            }
            return View(attribute);
        }
        //Get delete Attribut
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                Error error = new Error();
                error.Messag = "Bad request";
               // return Error(error);
            }
            MyAttribute attribute = Meneger.Meneger.Find(id);

            if (attribute == null)
            {
                return HttpNotFound();
            }
            return View(attribute);
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            MyAttribute attribute = Meneger.Meneger.Find(id);
            Meneger.Meneger.Remove(attribute);
            return RedirectToAction("Index");
        }

       
        public ActionResult AttributeValue()//cucadrum e text tipi atributneri arjeqner@
        {
            List<AttributeXMLTextValueModel> atrValueList = new List<AttributeXMLTextValueModel>();
            foreach (var item in context.AttributValues)
            {
            AttributeXMLTextValueModel atrValue = new AttributeXMLTextValueModel();    
            atrValue =  Helper.Helper.XmlTextValueDeSerialization(item.AttributValue1);
            atrValue.AttributeValueID = item.ID;
            atrValueList.Add(atrValue);          
            }
            
            return PartialView("_AttributeAddTextValue", atrValueList);//cucadrum e text tipi antributneri arjeqner@
        }
        public ActionResult AttributeAddTextValue()//cucadrum e text  tipi atributi avelacman ej@ 
        {   
            return View(attribute);
        }
     
        // GET: Add Attribute Value
        public ActionResult AddAttributeValue(int? id) //cucadrum e hamapatasxan tipi atributi arjeqi avelacman ej@
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MyAttribute atr= Meneger.Meneger.Find(id);
            attribute = atr;
            if (attribute == null)
            {
                return HttpNotFound();
            }
            ViewBag.AttributeType = context.AttributesTypes.Find(attribute.TypeID).AttributeType;
            if (ViewBag.AttributeType == "text")
                return RedirectToAction("AttributeAddTextValue");
            return View(atr);
        }

        [HttpPost]
         [ValidateAntiForgeryToken]
        public  ActionResult AddAttributeValue(string value)
        {
            if(Meneger.Meneger.ValidationAttributeValue(value, attribute.ID) == false)
            {
                Error errortemp = new Error();
                errortemp.Name = "AttributeValueValidation";
                errortemp.Messag = "error input: Attribute value of the number of symbols in error";
                error = errortemp;
                return RedirectToAction("Error");
            }
            if (value != null)
            {
                AttributeXMLTextValueModel atrValue = new AttributeXMLTextValueModel();// texapoxel helper
                atrValue.Value = value;
                
                System.Xml.Serialization.XmlSerializer atrXml = new System.Xml.Serialization.XmlSerializer(atrValue.GetType());
                StringWriter stringWriter = new StringWriter();
                atrXml.Serialize(stringWriter, atrValue);
               AttributValue  dbAtrValue = new AttributValue();
                dbAtrValue.AttributID = attribute.ID;
                dbAtrValue.AttributValue1 = stringWriter.ToString();
                MyAttribute attrTemp = Meneger.Meneger.Find(attribute.ID);
                attribute = attrTemp;
                if (ModelState.IsValid)
                {
                    context.AttributValues.Add(dbAtrValue);
                    context.SaveChanges();
                }
                else
                {
                    Error errortemp = new Error();
                    errortemp.Messag = "do not enter an attribute value";
                    error = errortemp; 
                    return RedirectToAction("Error");
                }
            }
            
            return RedirectToAction("AttributeAddTextValue");

        }
        public ActionResult Error()
        {   
            return View(error);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
           
}

    }
}