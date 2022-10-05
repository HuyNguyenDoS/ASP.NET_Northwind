using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using LtCSDL.Models;

namespace LtCSDL.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private DataClasses1DataContext dt = new DataClasses1DataContext();

        public List<Cart> GetListCart()  //Lay DS gio hang
        {
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts == null)
            {
                carts = new List<Cart>();
                Session["Cart"] = carts;
            }
            return carts;
        }

        public ActionResult ListCarts()  //hien thi gio hang
        {
            List<Cart> carts = GetListCart();
            if(carts.Count==0)  //gio hang chua co san pham
            {
                return RedirectToAction("ListProduct", "Product");
            }
            ViewBag.CountProduct = Count();
            ViewBag.Total = Total();

            return View(carts);
        }

        public ActionResult AddCart(int id)
        {
            List<Cart> carts = GetListCart(); //lay DSGH
            Cart c = carts.Find(s => s.ProductID == id);
            if(c == null)
            {
                c = new Cart(id); //tao SPGH moi
                carts.Add(c);
            }
            else
            {
                c.Quantity++;
            }
            return RedirectToAction("ListCarts");
        }

        public ActionResult Delete(int id)
        {
            List<Cart> carts = GetListCart();
            Cart c = carts.Find( s => s.ProductID == id);

            if(c!=null)
            {
                carts.RemoveAll(s => s.ProductID == id);
                return RedirectToAction("ListProduct");
            }
            if (carts.Count == 0)
            {
                return RedirectToAction("ListProduct", "Product");
            }
            return RedirectToAction("ListCarts");
        }

        private int Count()
        {
            int n = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if(carts != null)
            {
                n = carts.Sum(s => s.Quantity);
            }
            return n;
        }

        private decimal? Total()
        {
            decimal? total = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts != null)
            {
                total = carts.Sum(s => s.Total);

            }
            return total;
        }

        public ActionResult OrderProduct (FormCollection fCollection)
        {
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    Order order = new Order();
                    List<Cart> carts = GetListCart();  //lay gio hang
                    order.OrderDate = DateTime.Now;
                    dt.Orders.InsertOnSubmit(order);
                    dt.SubmitChanges();
                    //order = dt.Orders.OrderByDescending(s => s.OrderID).Take(1).SingleOrDefault();
                    foreach (var item in carts)
                    {
                        Order_Detail d = new Models.Order_Detail();
                        d.OrderID = order.OrderID;
                        d.ProductID = item.ProductID;
                        d.Quantity = short.Parse(item.Quantity.ToString());
                        d.UnitPrice = (decimal)item.UnitPrice;
                        d.Discount = 0;

                        dt.Order_Details.InsertOnSubmit(d);
                    }
                    dt.SubmitChanges();
                    tranScope.Complete();
                    Session["Cart"] = null;
                }
                catch (Exception)
                {
                    tranScope.Dispose();
                    return RedirectToAction("ListCarts");
                }
            }
            return RedirectToAction("OrderDetailList", "Cart");
        }

        public ActionResult OrderDetailList()
        {
            var p = dt.Order_Details.OrderByDescending(s => s.OrderID).Select(s => s).ToList();
            return View(p);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}