using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Entity;
using System.IO;
using BooksCatalogue.Models;
using System.Web.Mvc.Html;

namespace BooksCatalogue.Controllers
{
    public class BooksController : Controller
    {
        private static string FilePath = "~/Picture/";
        private static IQueryable<Book> tempSortBooks;
        private static string NameAction;
        private static Book bookTemp;
        private static int?  bookID;
        static BooksOfAttributesViewModel bookAttr;

        private BooksCatalogueEntities1 db = new BooksCatalogueEntities1();
        
        // GET: Books
        public async Task<ActionResult> Index()
        {
            Book book = new Book();
             var books = db.Books.Include(b => b.Author).Include(b => b.Country);
            
            foreach (var item in books)
            {
                item.Price += decimal.Parse(item.Country.TelKod);
            }
            NameAction = "Index";
              return View(await books.ToListAsync());

        }

        public async Task<ActionResult> IndexGrid( string search,string sortby,int page = 1)
        {
            NameAction = "IndexGrid";

            var books = db.Books.Include(b => b.Author).Include(b => b.Country);
            if(String.IsNullOrEmpty(search) && String.IsNullOrEmpty(sortby))
            {

                books = PriceByCountryTelKod(books);
                books = books.OrderBy(k => k.BookName).Skip((page - 1) * 4).Take(4);
                ViewBag.Page = page;
                ViewBag.PageSize = 4;
                ViewBag.Count = books.Count();
                return View(await books.ToListAsync());
            }
            if (!String.IsNullOrEmpty(search))
                {
                    var searchBooks = Helper.Helper.Search(search, books);
                    searchBooks = PriceByCountryTelKod(searchBooks);
                    searchBooks = searchBooks.OrderBy(k => k.BookName).Skip((page - 1) * 4).Take(4);
                    ViewBag.Page = page;
                    ViewBag.Count = searchBooks.Count();
                    ViewBag.PageSize = 4;
                if(searchBooks.Count()==0)
                {
                    ViewBag.searchBooks = "null"; 
                }
                    return View(await searchBooks.ToListAsync());

            }
            if (!String.IsNullOrEmpty(sortby))

            {
               var sortBooks = PriceByCountryTelKod(books);
                 sortBooks = Helper.Helper.Sort(sortby,sortBooks);
                tempSortBooks = sortBooks;
                sortBooks = sortBooks.Skip((page - 1) * 4).Take(4);
                ViewBag.Page = page;
                ViewBag.Count = sortBooks.Count();
                ViewBag.PageSize = 4;
                return View(await sortBooks.ToListAsync());
            }
           
            PriceByCountryTelKod(books);
           books = books.OrderBy(k=>k.BookName).Skip((page - 1) * 4).Take(4);
            ViewBag.Page = page;
            ViewBag.PageSize = 4;
            return View(await books.ToListAsync());
            
        }
      
        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            Book book = new Book();
            
            book.Atributes = db.Attributes.ToList();
            List<AttributeXMLTextModel> attrList = new List<AttributeXMLTextModel>();
           
            foreach (var item in book.Atributes)
            {
                AttributeXMLTextModel atrxml = new AttributeXMLTextModel();
                atrxml = Helper.Helper.XmlTextDeSerialization(item.AttributName);
                attrList.Add(atrxml);
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FirstName");
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "CountryName");
            ViewBag.Atributes = new SelectList(attrList, "ID", "Name");

            return View();
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,BookName,Price,Description,PictureName,PageCount,PublishDate,CountryID,AuthorID,Books_of_Attributes")] Book book, HttpPostedFileBase PictureName)
        {
            if (ModelState.IsValid)
            {
                if (PictureName != null)
                {

                    book.PictureName = SavePicture(PictureName);
                }
                    db.Books.Add(book);

                    await db.SaveChangesAsync();//try catch
                    return RedirectToAction(NameAction);
                
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FirstName", book.AuthorID);
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "CountryName", book.CountryID);
            return View(book);

        }
    
        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FirstName", book.AuthorID);
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "CountryName", book.CountryID);
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,BookName,Price,Description,PictureName,PageCount,PublishDate,CountryID,AuthorID")] Book book, HttpPostedFileBase PictureName)
        {  var b = Request.Form["book"];
            
            if (ModelState.IsValid)
            {
                if (PictureName != null)
                    {
                    DeletePhoto(book.PictureName);
                    book.PictureName = SavePicture(PictureName);
                       }

                else
                    {
                        book.PictureName = book.PictureName;
                    }
                    db.Entry(book).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction(NameAction);
                }            
                ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FirstName", book.AuthorID);
                ViewBag.CountryID = new SelectList(db.Countries, "ID", "CountryName", book.CountryID);
                return View(book);
            
        } 

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);

            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
      

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Book book = await db.Books.FindAsync(id);
            if (book.PictureName != null)
            {
                DeletePhoto(book.PictureName);
            }
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return RedirectToAction(NameAction);
        }

        private string SavePicture(HttpPostedFileBase PictureName)
        {
            FileInfo file = new FileInfo(PictureName.FileName);
            string GuIdName = Guid.NewGuid().ToString() + file.Extension;// stugel vor miayn picture tipi filer pahi 
            var path = Path.Combine(HttpContext.Server.MapPath(FilePath), GuIdName);
            PictureName.SaveAs(path);
            return GuIdName;
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCountries()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void DeletePhoto(string photoFileName)
        {
            string fullPath = Request.MapPath(FilePath + photoFileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);

            }

        }
        private IQueryable<Book>  PriceByCountryTelKod(IQueryable<Book> books)
        {
            foreach (var item in books)
            {
                item.Price += decimal.Parse(item.Country.TelKod);
            }

            return books;
        }
        public ActionResult ShowAttributeList()
        {
            return PartialView("_AppendAttribute", Meneger.Meneger.GetBooksOfAttributeList(bookID));//cucadrum e hamaptsaxan  grqi arjeqner@
        }
        [HttpGet]

        public ActionResult AppendAttributeMeneger(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bookID = id;                   // grqi avelacan ev heracman hamar
            return RedirectToAction("AppendAttribute");
        }

        public ActionResult AppendAttribute()
        {
            Book book = db.Books.Find(bookID);
            book.Atributes = db.Attributes.ToList();
            book.Books_of_Attributes = db.Books_of_Attributes.Where(x => x.BooksID == book.ID).ToList();
            List<SelectListItem> selctedAttributes = new List<SelectListItem>();
            List<AttributeXMLTextModel> attrList = new List<AttributeXMLTextModel>();
            foreach (var item in book.Atributes)
            {
                AttributeXMLTextModel atrxml = new AttributeXMLTextModel();
                atrxml = Helper.Helper.XmlTextDeSerialization(item.AttributName);
                atrxml.ID = item.ID;
                attrList.Add(atrxml);
            }
            attrList.ForEach(x => {
                selctedAttributes.Add(new SelectListItem
                { Text = x.Name, Value = x.ID.ToString() });
            });
            book.SelectAttributeList = selctedAttributes;
            return View(book);

        }

        [HttpPost]
        public ActionResult AppendAttribute(string ID, string AttributeValueID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(bookID);
            int id = Convert.ToInt32(ID);
            int AttributeValueid = Convert.ToInt32(AttributeValueID);
            List<SelectListItem> selctedAttributes = new List<SelectListItem>(); 
            book.Atributes = db.Attributes.ToList();
            if (!string.IsNullOrEmpty(ID))
            {
                book.AttributeValues = db.AttributValues.Where(x=>x.AttributID == id).ToList();
            }
         
            // attribute list@ 
            List<AttributeXMLTextModel> attrList = new List<AttributeXMLTextModel>(); 
            foreach (var item in book.Atributes)
            {
                AttributeXMLTextModel atrxml = new AttributeXMLTextModel();
                atrxml = Helper.Helper.XmlTextDeSerialization(item.AttributName);
                atrxml.ID = item.ID;
                attrList.Add(atrxml);
            }
            attrList.ForEach(x => {
                selctedAttributes.Add(new SelectListItem
                { Text = x.Name, Value = x.ID.ToString()});
            });
            book.SelectAttributeList = selctedAttributes;

            //atributi arejeq
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

            // bazayum grel
            if (!string.IsNullOrEmpty(AttributeValueID)&& AttributeValueid !=0) // grancum e bazayum book- hamapatasxan atribut@
            {
                Entity.Books_of_Attributes booksAttribute = new Books_of_Attributes();   
                if (Meneger.Meneger.IsBooksAttribut(book.ID,AttributeValueid ) == false)
                {
                    booksAttribute.AttributesID = AttributeValueid;
                    booksAttribute.BooksID = book.ID;
                    db.Books_of_Attributes.Add(booksAttribute);
                    db.SaveChanges();
                }
                    var AttributeValue = db.AttributValues.Find(AttributeValueid);
                    ViewBag.AttributValue = Helper.Helper.XmlTextValueDeSerialization(AttributeValue.AttributValue1).Value;
                    ViewBag.AttributName = Helper.Helper.XmlTextDeSerialization(db.Attributes.Find(AttributeValue.AttributID).AttributName).Name;
            }

            return View(book);
        }
        public ActionResult EditAppendAttribute(int id)

        {
            bookAttr = Meneger.Meneger.GetBooksOfAttribute(bookID, id);
            return View(bookAttr);

        }
        [HttpPost]
        public ActionResult EditAppendAttribute(string ID, string AttributeValueID)
        {

            if (string.IsNullOrEmpty(ID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int id = Convert.ToInt32(ID);
            int AttributeValueid = Convert.ToInt32(AttributeValueID);
            bookAttr.SelectAttributeValueList = Meneger.Meneger.GetAttributeValueByAttributeID(id);
            bookAttr.AttributeValues = db.AttributValues.Where(x => x.AttributID == id).ToList();;
            if (!string.IsNullOrEmpty(AttributeValueID) && AttributeValueid != 0) // grancum e bazayum book- hamapatasxan atribut@
            {
                Entity.Books_of_Attributes booksAttribute = Meneger.Meneger.FindBookOfAttribute(bookID, id);
                if (Meneger.Meneger.IsBooksAttribut(bookID, AttributeValueid) == false)
                {
                    booksAttribute.AttributesID = AttributeValueid;
                    db.SaveChanges();
                }
                return RedirectToAction("AppendAttribute");
            }

            return View(bookAttr);
        }


        [HttpGet]
        public ActionResult DeleteAppendAttribute(int id)
        {
            return View(Meneger.Meneger.GetBooksOfAttribute(bookID, id));
        }
        
        public ActionResult DeleteAppendAttributePost(int id )
        {
             Meneger.Meneger.DeleteBooksOfAttribute(bookID, id);
            return RedirectToAction("AppendAttribute");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
