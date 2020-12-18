using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public JobOffer Offer { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Contact Phone")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Contact Email")]
        public string EmailAddress { get; set; }
        [Display(Name = "Contact Agreement")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Accepting our Terms is mandatory!")]
        public bool ContactAgreement { get; set; }
        [Display(Name = "CV Url")]
        public string CvUrl { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
        public string Description { get; set; }
    }
}
