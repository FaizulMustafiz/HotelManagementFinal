using System.Web.Mvc;

namespace HotelManagementFinal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            CheckIns = new HashSet<CheckIn>();
        }

        public int CustomerId { get; set; }

        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "Please Enter Customer Number")]
        public string CustomerName { get; set; }

        [Display(Name = "National ID Number")]
        [Required(ErrorMessage = "Please Enter Customer National ID Number")]
        [StringLength(int.MaxValue, MinimumLength = 14, ErrorMessage = "Please Enter valid National ID Number")]
        [Remote("CheckNid", "Customers", ErrorMessage = "This Customer Already Exists!")]
        public string CustomerNid { get; set; }

        public string CustomerRegistrationNo { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "PLease Give Customer Phone Number")]
        [RegularExpression(@"^01\d{9}$", ErrorMessage = "Mobile No is not valid")]
        public string CustomerPhoneNo { get; set; }

        [Display(Name = "Customer Address")]
        [Required(ErrorMessage = "Please input Customer Address")]
        [DataType(DataType.MultilineText)]
        public string CustomerAddress { get; set; }

        [Display(Name = "Passport Number")]
        [StringLength(int.MaxValue, MinimumLength = 7, ErrorMessage = "Please Enter Valid Pasport Number")]
        public string CustomerPassportNo { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckIn> CheckIns { get; set; }
    }
}
