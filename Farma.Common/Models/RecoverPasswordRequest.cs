using System;
using System.Collections.Generic;
using System.Text;

namespace Farma.Common.Models
{

    using System.ComponentModel.DataAnnotations;

    public class RecoverPasswordRequest
    {
        [Required]
        public string Email { get; set; }
    }

}
