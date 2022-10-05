using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LtCSDL.Models
{
    public class Cart
    {
        private DataClasses1DataContext dt = new DataClasses1DataContext();

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal? UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal? Total { get { return UnitPrice * Quantity; } }

        public Cart(int productID)
        {
            this.ProductID = productID;
            Product p = dt.Products.Single(n => n.ProductID == productID);
            ProductName = p.ProductName;
            UnitPrice = p.UnitPrice;
            Quantity = 1;
        }
    }

}