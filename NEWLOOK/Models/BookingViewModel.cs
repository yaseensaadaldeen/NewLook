using System;
using System.ComponentModel.DataAnnotations;
namespace NEWLOOK.Models
{
    public class BookingViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        [Display(Name = "Full Name")]
        public string CustmName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string CustmEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Phone Number")]
        public string CustmPhone { get; set; }

        [Display(Name = "Address")]
        public string CustmAddress { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [BirthDateValidation(ErrorMessage = "Birth date must be in the past.")]
        public DateTime BirthDt { get; set; }

        [Required(ErrorMessage = "Booking date is required.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Booking Date")]
        public DateTime BookDate { get; set; }
    }
}
