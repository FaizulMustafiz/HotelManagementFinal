using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
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

        public ActionResult CheckInInfo()
        {
            var chekIn = db.CheckIns.OrderByDescending(r => r.CheckInId).FirstOrDefault();
            TempData["success"] = "Check In Successfull";
            return View(chekIn);
            //return PartialView("~/Views/Shared/_CheckInInfo.cshtml");
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
        public ActionResult Create([Bind(Include = "CheckInId,ChekInDate,CheckOutDate,Staying,CustomerId,RoomTypeId,RoomId,TotalPrice,Paying,RemainigPrice")] CheckIn checkIn)
        {
            if (ModelState.IsValid)
            {
                var checkInList = db.CheckIns.Where(x => x.RoomId == checkIn.RoomId && x.Room.RoomStatus == true).ToList();

                if (checkInList.Count == 0)
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
                    decimal totalPrice = stayingConvert * price;
                    checkIn.TotalPrice = totalPrice;
                    TempData["TP"] = totalPrice;
                    decimal? paying = checkIn.Paying;
                    decimal? remainingPrice = totalPrice - paying;
                    TempData["RP"] = remainingPrice;
                    aRoom.RoomStatus = true;
                    if (checkIn.RemainigPrice == null)
                    {
                        checkIn.RemainigPrice = checkIn.TotalPrice;
                    }
                    if (paying != null)
                    {
                        checkIn.RemainigPrice = remainingPrice;
                    }
                    else
                    {
                        checkIn.RemainigPrice = checkIn.TotalPrice;
                    }
                    db.CheckIns.Add(checkIn);
                    db.SaveChanges();
                    return RedirectToAction("CheckInInfo");
                }
                else
                {
                    bool status = false;
                    foreach (var setRoom in checkInList)
                    {
                        if ((checkIn.ChekInDate>=setRoom.ChekInDate && checkIn.ChekInDate<setRoom.CheckOutDate)||(checkIn.CheckOutDate>setRoom.ChekInDate && checkIn.CheckOutDate<=setRoom.CheckOutDate)&& checkIn.Room.RoomStatus==true)
                        {
                            status = true;
                        }
                    }
                    if (status==false)
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
                        decimal totalPrice = stayingConvert * price;
                        checkIn.TotalPrice = totalPrice;
                        TempData["TP"] = totalPrice;
                        decimal? paying = checkIn.Paying;
                        decimal? remainingPrice = totalPrice - paying;
                        TempData["RP"] = remainingPrice;
                        aRoom.RoomStatus = true;
                        if (checkIn.RemainigPrice == null)
                        {
                            checkIn.RemainigPrice = checkIn.TotalPrice;
                        }
                        if (paying != null)
                        {
                            checkIn.RemainigPrice = remainingPrice;
                        }
                        else
                        {
                            checkIn.RemainigPrice = checkIn.TotalPrice;
                        }
                        db.CheckIns.Add(checkIn);
                        db.SaveChanges();
                        return RedirectToAction("CheckInInfo");
                    }
                    else
                    {
                        TempData["already"] = "This Room is Already Booked";
                        return RedirectToAction("Create");
                    }
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
            if (roomId != null)
            {
                Room aRoom = db.Rooms.FirstOrDefault(r => r.RoomId == roomId);
                var aCheckIn = db.CheckIns.Where(r => r.Room.RoomId == roomId).OrderByDescending(m => m.CheckInId).FirstOrDefault();
                ViewBag.RoomName = aRoom.RoomName;
                ViewBag.RoomType = aRoom.RoomType.RoomTypeName;
                ViewBag.Price = aRoom.RoomPrice;
                ViewBag.Description = aRoom.RoomDescription;
                ViewBag.Status = aRoom.RoomStatus;
                ViewBag.CheckIn = aCheckIn.ChekInDate.ToShortDateString();
                ViewBag.CheckOut = aCheckIn.CheckOutDate.ToShortDateString();
                ViewBag.CustomerName = aCheckIn.Customer.CustomerName;

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

        public ActionResult ByDateGuestReport()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ByDateGuestReport(CheckIn checkIn)
        {
            DateTime chekInDate = checkIn.ChekInDate.Date;
            DateTime chekOutDate = checkIn.CheckOutDate.Date;
            var aCheckIn = db.CheckIns.Where(x => x.ChekInDate >= chekInDate && x.CheckOutDate <= chekOutDate).ToList();
            return PartialView("~/Views/Shared/_ByDateGestreport.cshtml", aCheckIn);
        }
    }
}
