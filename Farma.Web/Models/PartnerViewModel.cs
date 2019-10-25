using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Farma.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Farma.Web.Models
{
    public class PartnerViewModel:Partner
    {
        [Display(Name="Image")]
      
        public IFormFile ImageFile { get; set; }
        
        [FileExtensions(Extensions = "jpeg,jpg,png")]
        public string FileName
        {
            get
            {
                if (ImageFile != null)
                    return ImageFile.FileName;
                else
                    return "";
            }
        }
    }
}