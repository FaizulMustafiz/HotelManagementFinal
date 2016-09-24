using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagementFinal.Models;

namespace HotelManagementFinal.Controllers
{
    public class CustomersController : Controller
    {
        private HotelManagementSystemContext db = new HotelManagementSystemContext();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.IdentificationTypeId = new SelectList(db.IdentificationTypes, "IdentificationTypeId", "IdentificationTypeName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,CustomerName,CustomerNid,CustomerRegistrationNo,CustomerPhoneNo,CustomerAddress,CustomerPassportNo,IdentificationTypeId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (!db.Customers.Any(x => x.CustomerNid == customer.CustomerNid))
                {
                    customer.CustomerRegistrationNo = CreeateRegistrationNo(customer);
                    var regId = customer.CustomerName + " Registration Id: " + customer.CustomerRegistrationNo;
                    TempData["regId"] = regId;

                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
                else
                {
                    ViewBag.Message = "This Customer Already Exists";
                }
            }
            ViewBag.IdentificationTypeId = new SelectList(db.IdentificationTypes, "IdentificationTypeId", "IdentificationTypeName", customer.IdentificationTypeId);
            return View(customer);
        }


        public string CreeateRegistrationNo(Customer customer)
        {
            int id = db.Customers.Count(c => c.CustomerName == customer.CustomerName) + 1;

            string registrationId = customer.CustomerName + "_";

            string addZero = "";
            int len = 3 - id.ToString().Length;
            for (int i = 0; i < len; i++)
            {
                addZero = "0" + addZero;
            }
            return registrationId + addZero + id + 1;
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,CustomerName,CustomerNid,CustomerRegistrationNo,CustomerPhoneNo,CustomerAddress,CustomerPassportNo")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
