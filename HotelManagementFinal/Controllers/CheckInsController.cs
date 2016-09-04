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
    public class CheckInsController : Controller
    {
        private HotelManagementSystemContext db = new HotelManagementSystemContext();

        // GET: CheckIns
        public ActionResult Index()
        {
            var checkIns = db.CheckIns.Include(c => c.Customer).Include(c => c.Room).Include(c => c.RoomType);
            return View(checkIns.ToList());
        }

        // GET: CheckIns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckIn checkIn = db.CheckIns.Find(id);
            if (checkIn == null)
            {
                return HttpNotFound();
            }
            return View(checkIn);
        }

        // GET: CheckIns/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomName");
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName");
            return View();
        }

        // POST: CheckIns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CheckInId,ChekInDate,CheckOutDate,Staying,CustomerId,RoomTypeId,RoomId,TotalPrice")] CheckIn checkIn)
        {
            if (ModelState.IsValid)
            {
                var checkInList = db.CheckIns.Where(x => x.RoomId == checkIn.RoomId && x.Room.RoomStatus == true).ToList();

                if (checkInList.Count > 0)
                {
                    TempData["already"] = "This Room is Already Booked";
                    return RedirectToAction("Create");
                }
                else
                {
                    Customer aCustomer = db.Customers.FirstOrDefault(c => c.CustomerId == checkIn.CustomerId);
                    Room aRoom = db.Rooms.FirstOrDefault(c => c.RoomId == checkIn.RoomId);

                    DateTime checkInDate = checkIn.ChekInDate.Date;
                    DateTime checkOutDate = checkIn.CheckOutDate.Date;
                    TimeSpan staying = checkOutDate - checkInDate;
                    checkIn.Staying = Convert.ToString(staying.TotalDays);
                    var roomId = db.Rooms.FirstOrDefault(x => x.RoomId == checkIn.RoomId);
                    decimal price = roomId.RoomPrice;
                    decimal stayingConvert = Convert.ToDecimal(checkIn.Staying);
                    decimal totalPrice = stayingConvert*price;
                    checkIn.TotalPrice = totalPrice;
                    aRoom.RoomStatus = true;
                    db.CheckIns.Add(checkIn);
                    ViewBag.TotalPrice = totalPrice;
                    db.SaveChanges();
                    TempData["success"] = "This "+ aRoom.RoomName+" is Checked in by "+aCustomer.CustomerName+" from " + checkIn.ChekInDate.Date+ " to "+checkIn.CheckOutDate.Date;
                    return RedirectToAction("Create");
                }
                
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", checkIn.CustomerId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomName", checkIn.RoomId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", checkIn.RoomTypeId);
            return View(checkIn);
        }

        // GET: CheckIns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckIn checkIn = db.CheckIns.Find(id);
            if (checkIn == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", checkIn.CustomerId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomName", checkIn.RoomId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", checkIn.RoomTypeId);
            return View(checkIn);
        }

        // POST: CheckIns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CheckInId,ChekInDate,CheckOutDate,Staying,CustomerId,RoomTypeId,RoomId,TotalPrice")] CheckIn checkIn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkIn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", checkIn.CustomerId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomName", checkIn.RoomId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", checkIn.RoomTypeId);
            return View(checkIn);
        }

        // GET: CheckIns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckIn checkIn = db.CheckIns.Find(id);
            if (checkIn == null)
            {
                return HttpNotFound();
            }
            return View(checkIn);
        }

        // POST: CheckIns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckIn checkIn = db.CheckIns.Find(id);
            db.CheckIns.Remove(checkIn);
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


        public PartialViewResult CustomerInfoLoad(int? customerId)
        {
            if (customerId != null)
            {
                Customer aCustomer = db.Customers.FirstOrDefault(c => c.CustomerId == customerId);
                ViewBag.RegId = aCustomer.CustomerRegistrationNo;
                ViewBag.CustomerName = aCustomer.CustomerName;
                ViewBag.Nid = aCustomer.CustomerNid;
                ViewBag.Phone = aCustomer.CustomerPhoneNo;
                ViewBag.Address = aCustomer.CustomerAddress;
                ViewBag.Passport = aCustomer.CustomerPassportNo;

                return PartialView("~/Views/Shared/_CustomerInfoLoad.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_CustomerInfoLoad.cshtml");
            }
        }


        public PartialViewResult RoomInfoLoad(int? roomId)
        {
            if (roomId!= null)
            {
                Room aRoom = db.Rooms.FirstOrDefault(r => r.RoomId == roomId);
                ViewBag.RoomName = aRoom.RoomName;
                ViewBag.RoomType = aRoom.RoomType.RoomTypeName;
                ViewBag.Price = aRoom.RoomPrice;
                ViewBag.Description = aRoom.RoomDescription;
                ViewBag.Status = aRoom.RoomStatus;

                return PartialView("~/Views/Shared/_RoomInfoLoad.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_RoomInfoLoad.cshtml");
            }
        }

        public ActionResult LoadRoom(int? roomTypeId)
        {
            var roomList = db.Rooms.Where(r => r.RoomTypeId == roomTypeId).ToList();
            ViewBag.RoomId = new SelectList(roomList, "RoomId", "RoomName");
            return PartialView("~/Views/Shared/_FillteredRooms.cshtml");
        }


    }
}
