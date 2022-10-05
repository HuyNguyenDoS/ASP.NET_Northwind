using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LtCSDL.Models;

namespace LtCSDL.Controllers
{
    public class ProductController : Controller
    {
        DataClasses1DataContext da = new DataClasses1DataContext();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListProduct()
        {
            var ds = da.Products.Select(s => s).ToList();
            return View(ds);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Product product)
        {
            da.Products.InsertOnSubmit(product);
            da.SubmitChanges();
            return RedirectToAction("ListProducts");
        }


        public ActionResult Details(int id)
        {
            var p = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            //var p = da.Products.FirstOrDefault();
            return View(p);
        }


        public ActionResult Delete(int id)
        {
            var sp = da.Products.First(m => m.ProductID == id);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            da.Products.DeleteOnSubmit(da.Products.First(s => s.ProductID == id));
            da.SubmitChanges();
            return RedirectToAction("ListProduct");
        }
        public ActionResult Edit(int id)
        {
            var p = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection, int id)
        {
            var sp = da.Products.First(m => m.ProductID == id);
            sp.ProductName = collection["ProductName"];
            sp.UnitPrice = decimal.Parse(collection["UnitPrice"]);
            sp.SupplierID = int.Parse(collection["SupplierID"]);
            sp.UnitsInStock = short.Parse(collection["UnitsInStock"]);

            UpdateModel(sp);
            da.SubmitChanges();
            return RedirectToAction("ListProduct");
        }
    }
}
