using System.Web.Mvc;

namespace HotelManagementFinal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CheckIn")]
    public partial class CheckIn
    {
        public int CheckInId { get; set; }

        [Display(Name = "Chek In Date")]
        [Required(ErrorMessage = "Please Enter Check In Date")]
        [DataType(DataType.Date)]
        public DateTime ChekInDate { get; set; }


        [Display(Name = "Chek Out Date")]
        [Required(ErrorMessage = "Please Enter Check Out Date")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        public string Staying { get; set; }

        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "Please Select a Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Room Type")]
        [Required(ErrorMessage = "Please Select any Room Type")]
        public int RoomTypeId { get; set; }

        [Display(Name = "Available Rooms")]
        [Required(ErrorMessage = "Please Select any Room")]
        public int RoomId { get; set; }
        [Display(Name = "Total Price")]
        public decimal? TotalPrice { get; set; }
        [Display(Name = "Paid Ammount")]
        public decimal? Paying { get; set; }
        [Display(Name = ("Remaining Price"))]
        public decimal? RemainigPrice { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Room Room { get; set; }

        public virtual RoomType RoomType { get; set; }
    }
}
