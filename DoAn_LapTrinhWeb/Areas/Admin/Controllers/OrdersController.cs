using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using PagedList;
using DoAn_LapTrinhWeb.Controllers;
using System.Collections.Generic;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly DbContext db = new DbContext();
      

        // GET: Areas/Orders
        public ActionResult Index(string search, int? size, int? page)
        {
            var pageSize = size ?? 15;
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            ViewBag.countTrash = db.Orders.Where(a => a.status == "0").Count(); //  đếm tổng sp có trong thùng rác
            var list = from a in db.Orders
                       where a.status =="1"|| a.status == "2"||a.status == "3"
                       orderby a.create_at descending
                       select a;
            if (!string.IsNullOrEmpty(search))
            {
                list = from a in db.Orders
                       where a.order_id.ToString().Contains(search)
                       orderby a.create_at descending
                       select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Trash(string search, int? size, int? page)
        {
            var pageSize = size ?? 15;
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            var list = from a in db.Orders
                       where a.status == "0"
                       orderby a.create_at descending
                       select a;
            if (!string.IsNullOrEmpty(search))
            {
                list = from a in db.Orders
                       where a.order_id.ToString().Contains(search)
                       orderby a.create_at descending
                       select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            Order order = db.Orders.FirstOrDefault(m => m.order_id == id);
            ViewBag.ListProduct = db.Oder_Detail.Where(m => m.order_id == order.order_id).ToList();
            ViewBag.OrderHistory = db.Orders.Where(m => m.account_id == order.account_id).OrderByDescending(m=>m.oder_date).Take(10).ToList();
            if (order == null)
            {
                Notification.setNotification1_5s("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }
            return View(order);
        }
        private Tuple<List<Product>, List<int>> GetCart()
        {
            //check null 
            var cart = Request.Cookies.AllKeys.Where(c => c.IndexOf("product_") == 0);
            var productIds = new List<int>();
            var quantities = new List<int>();
            var errorProduct = new List<string>();
            var cValue = "";
            // Lấy mã sản phẩm & số lượng trong giỏ hàng
            foreach (var item in cart)
            {
                var tempArr = item.Split('_');
                if (tempArr.Length != 2)
                {
                    //Nếu không lấy được thì xem như sản phẩm đó bị lỗi
                    errorProduct.Add(item);
                    continue;
                }
                cValue = Request.Cookies[item].Value;
                productIds.Add(Convert.ToInt32(tempArr[1]));
                quantities.Add(Convert.ToInt32(String.IsNullOrEmpty(cValue) ? "0" : cValue));
                if (cValue == "0")
                {
                    Response.Cookies["product_" + tempArr[1]].Expires = DateTime.Now;
                }
            }
            // Select sản phẩm để hiển thị
            var listProduct = new List<Product>();
            foreach (var id in productIds)
            {
                var product = db.Products.SingleOrDefault(p => p.status == "10" && p.product_id == id);
                if (product != null)
                {
                    listProduct.Add(product);
                }
                else
                {
                    // Trường hợp ko chọn được sản phẩm như trong giỏ hàng
                    // Đánh dấu sản phẩm trong giỏ hàng là sản phẩm lỗi
                    errorProduct.Add("product-" + id);
                    quantities.RemoveAt(productIds.IndexOf(id));
                }
            }
            //Xóa sản phẩm bị lỗi khỏi cart
            foreach (var err in errorProduct)
            {
                Response.Cookies[err].Expires = DateTime.Now.AddDays(-1);
            }
            return new Tuple<List<Product>, List<int>>(listProduct, quantities);//lấy id sản phẩm và số lượng
        }

        public JsonResult UpdateOrder(int id,string status)
        {
            string result = "error";
            Order order = db.Orders.FirstOrDefault(m => m.order_id == id);
            Oder_Detail order_detail = db.Oder_Detail.FirstOrDefault(m => m.order_id == id);
            try
            {
                if (order.status != "3")
                {
                    result = "success";
                    order.status = status;
                    //if(order.status=="3")
                    //{
                    //    var cart = this.GetCart();
                    //    var listProduct = new List<Product>();
                    //    for (int i = 0; i < cart.Item1.Count; i++)
                    //    {
                    //        var item = cart.Item1[i];
                    //        var _price = item.price;                         
                        
                    //        var product = db.Products.SingleOrDefault(p => p.product_id == item.product_id);                        
                           
                    //        product.buyturn += cart.Item2[i];
                    //        product.quantity = (Convert.ToInt32(product.quantity ?? "0") - cart.Item2[i]).ToString();
                    //        listProduct.Add(product);
                    //        db.Configuration.ValidateOnSaveEnabled = false;
                    //        db.SaveChanges();
                            
                    //    }


                    //}
                    order.update_at = DateTime.Now;
                    order.update_by = User.Identity.GetEmail();
                    order_detail.status = status;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Entry(order_detail).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                
                else
                {
                    result = "false";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult restoreorder(int id)
        {
            string result = "error";
            Order order = db.Orders.FirstOrDefault(m => m.order_id == id);
            Oder_Detail oder_Detail = db.Oder_Detail.FirstOrDefault(m => m.order_id == id);
            try
            {
                result = "active";
                oder_Detail.Product.quantity = (Convert.ToInt32(oder_Detail.Product.quantity)+oder_Detail.quantity).ToString();
                order.status = "4";
                db.Entry(oder_Detail).State = EntityState.Modified;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch 
            {
                return Json(result, JsonRequestBehavior.AllowGet);

            }

           
        }

        public JsonResult CancleOrder(int id)
        {
            string result = "error";
            Order order = db.Orders.FirstOrDefault(m => m.order_id == id);
            var oder_Detail = db.Oder_Detail.Where(m => m.order_id == order.order_id).ToList();
            //Oder_Detail save = db.Oder_Detail.Where(m => m.order_id == id);

            try
            {
                if (order.status != "3")
                {
                    result = "success";

                    foreach (var item in oder_Detail)
                    {
                        item.Product.quantity = (Convert.ToInt32(item.Product.quantity) + item.quantity).ToString();

                        db.Entry(item.Product).State = EntityState.Modified;
                    }


                    //oder_Detail.Product.quantity = (Convert.ToInt32(oder_Detail.Product.quantity) + oder_Detail.quantity).ToString();
                    order.status = "0";
                    order.update_at = DateTime.Now;
                    order.update_by = User.Identity.GetEmail();
                    //db.Entry(save).State = EntityState.Modified;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "false";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
           
        }
    }
}