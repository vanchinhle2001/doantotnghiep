using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
// using DoAn_LapTrinhWeb.Models;


namespace DoAn_LapTrinhWeb.Controllers
{
    public class HomeController : Controller
    {
        private DbContext db = new DbContext();
       
        //Trang chủ
        public ActionResult Index()
        {
           
            //test
            ViewBag.AvgFeedback = db.Feedbacks.ToList();
            ViewBag.HotProduct = db.Products.Where(item => item.status == "1" && item.quantity != "0").OrderByDescending(item => item.buyturn + item.view).Take(8).ToList();
            ViewBag.NewProduct = db.Products.Where(item => item.status == "1" && item.quantity != "0").OrderByDescending(item => item.create_at).Take(8).ToList();
            ViewBag.Laptop = db.Products.Where(item => item.status == "1" && item.type == 1 && item.quantity != "0").OrderByDescending(item => item.buyturn + item.view).Take(8).ToList();
            ViewBag.Accessory = db.Products.Where(item => item.status == "1" && item.type == 2 && item.quantity != "0").OrderByDescending(item => item.buyturn + item.view).Take(8).ToList();
            ViewBag.OrderDetail = db.Oder_Detail.ToList();
            return View();       
        }
        public ActionResult test()
        {
            return View("");
        }
        public ActionResult Present()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Contact(/*string name,string email,string phone,string content,*/Contact model)
        {
            string result = "false";
            try
            {

                Contact contact = new Contact();
                Account account = new Account();
                contact.name = model.name;
                contact.phone = model.phone;
                contact.email = model.email;
                contact.content = model.content;
                contact.create_at = DateTime.Now;
                contact.update_at = DateTime.Now;
                contact.status = "1";
                contact.update_by = "a";
                contact.create_by = "a";
                db.Contacts.Add(contact);
                db.SaveChanges();
                result = "success";
                Notification.setNotification1_5s("Gửi thành công!", "success");
            }
            catch
            {

                Notification.setNotification1_5s("Vui lòng nhập đủ thông tin", "error");
            }
            return View("ProcessContact");

        }
        public ActionResult ProcessContact()
        {

            return View();
        }
        //PageNotFound
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}
