using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
    public class JustificatoryUploadModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Justificatory")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string file { get; set; }
    }
}