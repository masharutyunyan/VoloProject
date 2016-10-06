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
using System.Web.Mvc.Html;

namespace BooksCatalogue.Controllers
{
    public class BooksController : Controller
    {
        private static string FilePath = "~/Picture/";
        //  Helper.Helper help = new Helper.Helper();
        private static IQueryable<Book> tempSearchBooks;
        private static IQueryable<Book> tempSortBooks;
        private static string NameAction;
        private BooksCatalogueEntities1 db = new BooksCatalogueEntities1();
        
        // GET: Books
        public async Task<ActionResult> Index()
        {
            Book book = new Book();
             var books = db.Books.Include(b => b.Author).Include(b => b.Country);
            //System.Collections.Generic.List <Book> temp = new List<Book>();
            
            foreach (var item in books)
            {
                item.Price += decimal.Parse(item.Country.TelKod);
            }
            NameAction = "Index";
              return View(await books.ToListAsync());
           // return View(Meneger.Meneger.GetBoooks());

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
                    tempSearchBooks = searchBooks;
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
            if (!String.IsNullOrEmpty(sortby) && tempSearchBooks.Count()>0)
            {
                var sortBooks = Helper.Helper.Sort(sortby, tempSearchBooks);
                sortBooks = PriceByCountryTelKod(books);
                sortBooks = sortBooks.OrderBy(k => k.BookName).Skip((page - 1) * 4).Take(4);
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
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FirstName");
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "CountryName");
            return View();
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,BookName,Price,Description,PictureName,PageCount,PublishDate,CountryID,AuthorID")] Book book, HttpPostedFileBase PictureName)
        {
            if (ModelState.IsValid)
            {
                if (PictureName != null)
                {

                    book.PictureName = SavePicture(PictureName);
                    //book.PictureName = help.SavePicture(PictureName);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,BookName,Price,Description,PictureName,PageCount,PublishDate,CountryID,AuthorID")] Book book, HttpPostedFileBase PictureName)
        {  var b = Request.Form["book"];
            
            if (ModelState.IsValid)
            {
                if (PictureName != null)
                    {
                    // help.DeletePhoto(book.PictureName);
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
               // help.DeletePhoto(book.PictureName);
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
