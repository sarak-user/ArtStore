using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Art_SaraKaminer;

namespace Art_SaraKaminer.Controllers
{
    public class OrderItemsController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: OrderItems
        public ActionResult Index(int? OrderId)
        {
            return View(db.OrderItems.Where(x=>x.OrderId == OrderId).ToList());
        }

        // GET: OrderItems/Details/5
        public ActionResult Details(int? id,int? OrderId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // GET: OrderItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderItemId,OrderId,ProductId,Price,Amount,TotalPrice")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.OrderItems.Add(orderItem);
                db.SaveChanges();
                return RedirectToAction("ShoppingCart");
            }

            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public ActionResult Edit(OrderItem orderItem)
        {
            if (orderItem.OrderItemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orderItem = db.OrderItems.Find(orderItem.OrderItemId);
			orderItem.TotalPrice = (orderItem.Amount * orderItem.Price);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
			db.SaveChanges();
			return RedirectToActionPermanent("Index", "OrderItems");
		}

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "OrderItemId,OrderId,ProductId,Price,Amount,TotalPrice")] OrderItem orderItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(orderItem).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(orderItem);
        //}

        // GET: OrderItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderItem orderItem = db.OrderItems.Find(id);
            db.OrderItems.Remove(orderItem);
            db.SaveChanges();
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
