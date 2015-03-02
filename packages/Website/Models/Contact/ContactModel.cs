using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Website.Models.Contact
{
    public class ContactModel
    {
        [Key]
        public int ContactID
        { get; set; }

        [Required]
        [StringLength(256)]
        public string FullName { get; set; }

        [StringLength(256)]
        public string Address { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]

        public string Email { get; set; }

        [Required]
        [StringLength(128)]
        public string Phone { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public Boolean Isprocessing { get; set; }

        public Config Config { get; set; }

        public Category Category { get; set; }

    }
}