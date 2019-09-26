using System;
using System.Collections.Generic;
using System.Text;

namespace Farma.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
