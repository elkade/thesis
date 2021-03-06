﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityWebsite.Api.Model.Users
{
    public class PostUserVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(32)]
        public string FirstName { get; set; }
        [MaxLength(32)]
        public string LastName { get; set; }
        [StringLength(6, MinimumLength = 6)]
        public string IndexNumber { get; set; }
        [StringLength(11, MinimumLength = 11)]
        public string Pesel { get; set; }
        public string Role { get; set; }
    }
}