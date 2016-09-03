using System.Web.Mvc;

namespace HotelManagementFinal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Room")]
    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            CheckIns = new HashSet<CheckIn>();
        }

        public int RoomId { get; set; }

        [Display(Name = "Room Name")]
        [Required(ErrorMessage = "Please Enter a Room Name")]
        [Remote("CheckName", "Rooms", ErrorMessage = "This Room Name Already Exists")]
        public string RoomName { get; set; }
        [Display(Name = "Room Type Id")]
        [Required(ErrorMessage = "Please Select any Room Type")]
        public int RoomTypeId { get; set; }
        [Display(Name = "Room Price")]
        [Required(ErrorMessage = "You Have To enter Room Price")]
        [RegularExpression(@"^\d{1,5}(\.\d{1,3})?$", ErrorMessage = "Price Can't be Negative Value")]
        public decimal RoomPrice { get; set; }
        [Display(Name = "Room Description")]
        [DataType(DataType.MultilineText)]
        public string RoomDescription { get; set; }

        

        public bool? RoomStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckIn> CheckIns { get; set; }

        public virtual RoomType RoomType { get; set; }
    }
}
