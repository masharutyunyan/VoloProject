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

namespace BooksCatalogue.Controllers
{
    public class AttributesTypesController : Controller
    {
        private BooksCatalogueEntities1 db = new BooksCatalogueEntities1();

        // GET: AttributesTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.AttributesTypes.ToListAsync());
        }

        // GET: AttributesTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributesType attributesType = await db.AttributesTypes.FindAsync(id);
            if (attributesType == null)
            {
                return HttpNotFound();
            }
            return View(attributesType);
        }

        // GET: AttributesTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttributesTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,AttributeType")] AttributesType attributesType)
        {
            if (ModelState.IsValid)
            {
                db.AttributesTypes.Add(attributesType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(attributesType);
        }

        // GET: AttributesTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributesType attributesType = await db.AttributesTypes.FindAsync(id);
            if (attributesType == null)
            {
                return HttpNotFound();
            }
            return View(attributesType);
        }

        // POST: AttributesTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,AttributeType")] AttributesType attributesType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attributesType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(attributesType);
        }

        // GET: AttributesTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributesType attributesType = await db.AttributesTypes.FindAsync(id);
            if (attributesType == null)
            {
                return HttpNotFound();
            }
            return View(attributesType);
        }

        // POST: AttributesTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AttributesType attributesType = await db.AttributesTypes.FindAsync(id);
            db.AttributesTypes.Remove(attributesType);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
