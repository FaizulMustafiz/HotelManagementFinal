using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagementFinal.Models;

namespace HotelManagementFinal.Controllers
{
    public class RoomsController : Controller
    {
        private HotelManagementSystemContext db = new HotelManagementSystemContext();

        // GET: Rooms
        public ActionResult Index()
        {
            var rooms = db.Rooms.Include(r => r.RoomType);
            return View(rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,RoomName,RoomTypeId,RoomPrice,RoomDescription,RoomStatus")] Room room)
        {
            if (ModelState.IsValid)
            {
                if (!db.Rooms.Any(x => x.RoomName == room.RoomName))
                {
                    db.Rooms.Add(room);
                    db.SaveChanges();
                    TempData["success"] = "Room Successfully Created";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Room Type Name Already Exists";
                }
            }

            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomId,RoomName,RoomTypeId,RoomPrice,RoomDescription,RoomStatus")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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

        public JsonResult CheckName(string RoomName)
        {
            return Json(!db.Rooms.Any(x => x.RoomName == RoomName), JsonRequestBehavior.AllowGet);
        }


        public ActionResult CheckOut()
        {
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomName");
            return View();
        }

        //public JsonResult CheckOutAll(bool decision)
        //{
        //    var rooms = db.Rooms.Where(m => m.RoomStatus == true).ToList();
        //    if (rooms.Count == 0)
        //    {
        //        return Json(false);
        //    }
        //    else
        //    {
        //        foreach (var room in rooms)
        //        {
        //            room.RoomStatus = false;
        //            db.Rooms.AddOrUpdate(room);
        //            db.SaveChanges();
        //        }
        //        return Json(true);
        //    }

        //}

        public JsonResult CheckOutAll(bool decision, int? roomId)
        {
            var rooms = db.Rooms.Where(m => m.RoomStatus == true && m.RoomId == roomId).ToList();
            if (rooms.Count == 0)
            {
                return Json(false);
            }
            else
            {
                foreach (var room in rooms)
                {
                    room.RoomStatus = false;
                    db.Rooms.AddOrUpdate(room);
                    db.SaveChanges();
                }
                return Json(true);
            }

        }


        public PartialViewResult RoomInfoLoadOnCheckOut(int? roomId)
        {
            if (roomId != null)
            {
                Room aRoom = db.Rooms.FirstOrDefault(r => r.RoomId == roomId);
                CheckIn aCheckIn = db.CheckIns.FirstOrDefault(r => r.Room.RoomId == roomId);

                ViewBag.RoomName = aRoom.RoomName;
                ViewBag.RoomType = aRoom.RoomType.RoomTypeName;
                ViewBag.Price = aRoom.RoomPrice;
                ViewBag.Description = aRoom.RoomDescription;
                ViewBag.Status = aRoom.RoomStatus;

                ViewBag.CustomerName = aCheckIn.Customer.CustomerName;
                ViewBag.CustomerRegId = aCheckIn.Customer.CustomerRegistrationNo;
                ViewBag.CustomerPhone = aCheckIn.Customer.CustomerPhoneNo;
                ViewBag.CustomerNid = aCheckIn.Customer.CustomerNid;

                return PartialView("~/Views/Shared/_RoomInfoLoadOnCheckOut.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_RoomInfoLoadOnCheckOut.cshtml");
            }
        }

    }
}
