using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace MvcPersonalSite.Models
{
    public class ContactData
    {
        [Required(ErrorMessage = "Your Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$",
            ErrorMessage = "Check the Email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Content Required")]
        [UIHint("MultilineText")]
        public string Comments { get; set; }

    }
}