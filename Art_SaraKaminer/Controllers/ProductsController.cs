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
	public class ProductsController : Controller
	{
		private StoreContext db = new StoreContext();

		// GET: Products
		public ActionResult Index(int? OrderId)
		{
			return View(db.Products.ToList());
		}

		// GET: Products/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}


		// GET: Products/Edit/5
		public ActionResult Buy(int? id, int? OrderId)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			if (OrderId.ToString() == String.Empty)
			{
				Order order = new Order()
				{
					OrderDate = DateTime.Now
				};
				Order order1 = db.Orders.Add(order);
				db.SaveChanges();
				OrderId = order1.OrderId;
			}
			OrderItem orderItem1 = db.OrderItems.FirstOrDefault(x=>x.OrderId == OrderId && x.ProductId == id);
			if (orderItem1 == null)
			{
				OrderItem orderItem = new OrderItem()
				{
					ProductId = product.ProductId,
					Title = product.Title,
					ArtDescription = product.ArtDescription,
					Price = product.Price,
					OrderId = Convert.ToInt32(OrderId),
					Amount = 1,
					TotalPrice = product.Price
				};
				db.OrderItems.Add(orderItem);
			}
			else
			{
				orderItem1.Amount += 1;
				orderItem1.TotalPrice = orderItem1.Amount * orderItem1.Price;
			}
			db.SaveChanges();
			//return View("Edit",orderItem);
			return RedirectToActionPermanent("Index", "OrderItems", new { OrderId = OrderId });
		}

		public ActionResult Edit(OrderItem orderItem)
		{
			return RedirectToActionPermanent("Edit", "OrderItems", orderItem);
		}

		public ActionResult End(Order order)
		{
			return RedirectToActionPermanent("Edit", "Orders", order);
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
