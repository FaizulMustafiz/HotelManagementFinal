using System.Web.Mvc;

namespace HotelManagementFinal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoomType")]
    public partial class RoomType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoomType()
        {
            CheckIns = new HashSet<CheckIn>();
            Rooms = new HashSet<Room>();
        }

        public int RoomTypeId { get; set; }

        [Display(Name = "Room Type Name")]
        [Required(ErrorMessage = "Please Enter Room Type Name")]
        [Remote("CheckName", "RoomTypes", ErrorMessage = "This Room Type Name Already Exists!")]
        public string RoomTypeName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckIn> CheckIns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
