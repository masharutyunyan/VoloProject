using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using System.Threading.Tasks;
using System.Data.Entity;
using BooksCatalogue.Models;
namespace BooksCatalogue.Controllers
{
    public class AttributeController : Controller
    {
        BooksCatalogueEntities1 context = new BooksCatalogueEntities1();
        // GET: Attribute
        public async Tesk<ActionResult> Index()
        {
            var attribute =   await Meneger.Meneger.GetAttribbutes();
            

            return View(attribute);
            // return View(Meneger.Meneger.GetBoooks());

        }
        public ActionResult Create()
        {
            ViewBag.TypeID= new SelectList(context.AttributesTypes, "ID", "AttributeType");
            return View();
        }

        [HttpPost]
         [ValidateAntiForgeryToken]
        public  ActionResult Create([Bind(Include = "ID,AttributName,TypeID")] MyAttribute attribute)
        {
            if (ModelState.IsValid)
            {
               Meneger.Meneger.SaveDB(attribute);
                return RedirectToAction("Index");
            }
            ViewBag.TypeID = new SelectList(context.AttributesTypes, "ID", "AttributeType", attribute.TypeID);
            return View(attribute);

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